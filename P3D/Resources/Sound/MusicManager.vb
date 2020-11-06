Imports Microsoft.VisualBasic
Imports NAudio.Wave
Imports NAudio.Wave.SampleProviders
Public Class LoopStream
    Inherits WaveStream
    Private _sourceStream As WaveStream

    Public Sub New(ByVal sourceStream As WaveStream, Optional ByVal Looping As Boolean = True)
        Me._sourceStream = sourceStream
        Me._enableLooping = Looping
    End Sub

    Public Property _enableLooping As Boolean

    Public Overrides ReadOnly Property WaveFormat As WaveFormat
        Get
            Return _sourceStream.WaveFormat
        End Get
    End Property

    Public Overrides ReadOnly Property Length As Long
        Get
            Return _sourceStream.Length
        End Get
    End Property

    Public Overrides Property Position As Long
        Get
            Return _sourceStream.Position
        End Get
        Set(ByVal value As Long)
            _sourceStream.Position = value
        End Set
    End Property

    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
        Dim totalBytesRead As Integer = 0

        While totalBytesRead < count
            Dim bytesRead As Integer = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead)

            If bytesRead = 0 Then

                If _enableLooping Then
                    _sourceStream.Position = 0
                Else
                    If Not _sourceStream.Position = 0 Then
                        Exit While
                    End If
                End If
            End If
            totalBytesRead += bytesRead
        End While

        Return totalBytesRead
    End Function
End Class
Public Class MusicManager

    Private Const NO_MUSIC As String = "*nomusic*" ' contains * as character, which cannot be in a filename
    Private Const DEFAULT_FADE_SPEED As Single = 0.02F

    Private Shared _songs As Dictionary(Of String, SongContainer) = New Dictionary(Of String, SongContainer)()
    Public Shared Property Volume As Single = 1.0F
    Private Shared _lastVolume As Single = 1.0F
    Private Shared _muted As Boolean = False
    Private Shared _paused As Boolean = False
    Public Shared _isLooping As Boolean = False

    ' currently playing song
    Private Shared _currentSongName As String = NO_MUSIC
    ' if the song in _currentSong is an actual existing song
    Private Shared _currentSongExists As Boolean = False
    Private Shared _currentSong As SongContainer = Nothing

    ' time until music playback is paused for sound effect
    Private Shared _pausedUntil As Date
    Private Shared _isPausedForSound As Boolean = False

    ' time until the intro of a song plays
    Private Shared _introMuteTime As Date
    Public Shared _introEndTime As Date
    Private Shared _isIntroStarted As Boolean = False
    ' song that gets played after the intro finished
    Private Shared _introContinueSong As String

    ' song that plays after fading is finished
    Private Shared _nextSong As String
    ' speeds that get added/subtracted from the volume to fade the song
    Private Shared _fadeSpeed As Single = DEFAULT_FADE_SPEED
    ' if the song that gets played after fading completed is an intro to another song
    Private Shared _fadeIntoIntro As Boolean = False
    Private Shared _isFadingOut As Boolean = False
    Private Shared _isFadingIn As Boolean = False
    ' NAudio properties
    Public Shared outputDevice As WaveOutEvent
    Public Shared audioFile As VorbisWaveReader
    Public Shared _stream As WaveChannel32

    Public Shared Property MasterVolume As Single = 1.0F
    Public Shared ReadOnly Property CurrentSong As SongContainer
        Get
            Return _currentSong
        End Get
    End Property

    Public Shared Property Muted As Boolean
        Get
            Return _muted
        End Get
        Set(value As Boolean)
            If _muted <> value Then
                _muted = value

                If _muted = True Then
                    If outputDevice IsNot Nothing Then
                        Volume = 0.0F
                        Core.GameMessage.ShowMessage(Localization.GetString("game_message_audio_off"), 12, FontManager.MainFont, Color.White)
                    End If

                Else
                    If outputDevice IsNot Nothing Then
                        If _isPausedForSound = True Then
                            _muted = True
                            Volume = 0.0F
                        Else
                            Volume = 1.0F
                            Core.GameMessage.ShowMessage(Localization.GetString("game_message_audio_on"), 12, FontManager.MainFont, Color.White)
                        End If
                    End If
                End If
            End If
        End Set
    End Property
    Public Shared Property Paused As Boolean
        Get
            Return _paused
        End Get
        Set(value As Boolean)
            If _paused <> value Then
                _paused = value

                If _paused = True Then
                    If outputDevice IsNot Nothing Then
                        outputDevice.Pause()
                        _introMuteTime = Date.Now
                    End If
                Else
                    ResumePlayback()
                End If
            End If
        End Set
    End Property
    Public Shared Sub Setup()
        MasterVolume = 1.0F
        If Muted = True Then
            Volume = 0.0F
        Else
            Volume = 1.0F
        End If
        _nextSong = ""
        _fadeSpeed = DEFAULT_FADE_SPEED
        _isFadingOut = False
        _isFadingIn = False
        _isLooping = True
    End Sub

    Public Shared Sub Clear()
        _songs.Clear()
        LoadMusic(False)
    End Sub

    Public Shared Sub ClearCurrentlyPlaying()
        ' cleans all remains of currently playing songs
        _currentSongExists = False
        _currentSong = Nothing
        _currentSongName = NO_MUSIC
        _isIntroStarted = False
    End Sub

    Public Shared Sub PlayNoMusic()
        ' fades out current track and sets to NO_MUSIC
        Play(NO_MUSIC)
    End Sub

    Public Shared Sub Update()
        If _isPausedForSound Then
            If Date.Now >= _pausedUntil Then
                If MusicManager.Paused = True Then
                    _isPausedForSound = False
                    Paused = False
                End If
            End If
        Else

            ' fading
            If _isFadingOut Then
                Volume -= _fadeSpeed

                If Volume <= 0F Then

                    Volume = 0F
                    _isFadingOut = False
                    _isFadingIn = True

                    Dim song = GetSong(_nextSong)

                    If Not song Is Nothing Then

                        Play(song)
                        _nextSong = ""

                        If _fadeIntoIntro Then
                            _fadeIntoIntro = False
                            _introEndTime = Date.Now + song.Duration
                            _isIntroStarted = True
                        Else
                            _isLooping = True
                        End If

                    Else

                        ' no song found, do not fade into anything
                        _fadeIntoIntro = False
                        ClearCurrentlyPlaying()
                        _isFadingIn = False
                        If Muted = True Then
                            Volume = 0.0F
                        Else
                            Volume = 1.0F
                        End If
                        If _nextSong = NO_MUSIC Then
                            _nextSong = ""
                            MusicManager.Stop()
                        End If

                    End If

                End If

            ElseIf _isFadingIn Then
                If Muted = True Then
                    Volume = 0.0F
                    _isFadingIn = False
                Else
                    Volume += _fadeSpeed
                    If Volume >= 1.0F Then
                        Volume = 1.0F
                        _isFadingIn = False
                    End If
                End If
            End If

            ' intro
            If _isIntroStarted Then
                If Paused = False Then
                    If Date.Now >= _introEndTime Then
                        Dim song = GetSong(_introContinueSong)
                        _isLooping = True
                        _isIntroStarted = False
                        Play(song)
                    End If
                End If
            End If
        End If

        If Core.GameInstance.IsActive AndAlso _lastVolume <> (Volume * MasterVolume) Then
            UpdateVolume()
        End If
    End Sub

    Public Shared Sub UpdateVolume()
        _lastVolume = Volume * MasterVolume
        If Not _stream Is Nothing Then
            _stream.Volume = Volume * MasterVolume
        End If
    End Sub

    Public Shared Sub PauseForSound(ByVal sound As SoundEffect)
        _isPausedForSound = True
        _pausedUntil = Date.Now + sound.Duration
        MusicManager.Pause()
    End Sub

    Public Shared Sub Pause()
        MusicManager.Paused = True
    End Sub

    Public Shared Sub [Stop]()
        If Not outputDevice Is Nothing Then
            outputDevice.Stop()
            outputDevice.Dispose()
        End If
        _isIntroStarted = False
    End Sub

    Public Shared Sub ResumePlayback()
        If Not _currentSong Is Nothing Then

            If outputDevice IsNot Nothing Then
                ' if an intro was playing while the music player was paused, calculate its end time
                If outputDevice.PlaybackState = PlaybackState.Paused AndAlso _isIntroStarted Then
                    Dim pauseTime As TimeSpan = Date.Now.Subtract(_introMuteTime)
                    _introEndTime = _introEndTime + pauseTime

                End If
                outputDevice.Play()
            End If
        End If

    End Sub

    Private Shared Sub Play(song As SongContainer)
        If Not song Is Nothing Then
            Logger.Debug($"Play song [{song.Name}]")
            If Not outputDevice Is Nothing Then
                outputDevice.Dispose()
            End If
            outputDevice = New WaveOutEvent()
            audioFile = New VorbisWaveReader(song.Song)
            _stream = New NAudio.Wave.WaveChannel32(New LoopStream(audioFile, _isLooping))
            outputDevice.Init(_stream)
            If Paused = False Then
                outputDevice.Play()
            End If
            _stream.Volume = Volume * MasterVolume
            _currentSongExists = True
            _currentSongName = song.Name
            _currentSong = song
        Else
            _currentSongExists = False
            _currentSongName = NO_MUSIC
            _currentSong = Nothing
        End If

    End Sub

    Private Shared Sub FadeInto(song As SongContainer, fadeSpeed As Single)
        _isFadingOut = True
        If Not song Is Nothing Then
            _nextSong = song.Name
        Else
            _nextSong = NO_MUSIC
        End If
        _fadeSpeed = fadeSpeed
    End Sub

    Public Shared Function Play(song As String) As SongContainer
        Return Play(song, False, DEFAULT_FADE_SPEED)
    End Function

    Public Shared Function Play(song As String, playIntro As Boolean, Optional loopSong As Boolean = True) As SongContainer
        Return Play(song, playIntro, DEFAULT_FADE_SPEED)
    End Function

    Public Shared Function Play(song As String, playIntro As Boolean, fadeSpeed As Single, Optional loopSong As Boolean = True) As SongContainer

        Dim playedSong As SongContainer = Nothing

        ' get the current song, only play if it's different
        Dim currentSong = GetCurrentSong().ToLowerInvariant()
        Dim songName = GetSongName(song)

        If currentSong = NO_MUSIC OrElse currentSong <> songName Then

            If playIntro = True Then
                _isLooping = False
                Dim introSong = GetSong("intro\" + songName)
                If Not introSong Is Nothing Then
                    If introSong.Origin = GetSong(songName).Origin Then

                        ' play the intro
                        ' setup the continue song
                        _introContinueSong = songName
                        ' do not repeat media player, do not want intro to loop
                        '_isLooping = False

                        If fadeSpeed > 0F Then
                            _isIntroStarted = False
                            _fadeIntoIntro = True
                            FadeInto(introSong, fadeSpeed)
                        Else
                            _isIntroStarted = True
                            _introEndTime = Date.Now + introSong.Duration
                            Play(introSong)
                        End If

                        playedSong = introSong

                        ' load the next song so the end of the intro doesn't lag
                        GetSong(song)
                    End If
                Else
                    _isIntroStarted = False
                    _fadeIntoIntro = False
                End If
            Else
                _isIntroStarted = False
                _fadeIntoIntro = False
            End If

            ' intro was not requested or does not exist
            If Not _isIntroStarted AndAlso Not _fadeIntoIntro Then
                If loopSong = True Then
                    _isLooping = True
                Else
                    _isLooping = False
                End If
                Dim nextSong = GetSong(song)
                If fadeSpeed > 0F Then
                    FadeInto(nextSong, fadeSpeed)
                Else
                    Play(nextSong)
                End If

                playedSong = nextSong

            End If

        End If

        Return playedSong

    End Function

    Public Shared Function SongExists(songName As String) As Boolean
        Return Not GetSong(songName) Is Nothing
    End Function

    Private Shared Function GetCurrentSong() As String
        ' if the currently playing song is an intro, return the song that plays after the intro
        ' this prevents the same song from replaying if it starts playing while the intro is still in effect

        ' but if it's already fading, do not play the song if the song we are fading into is the same.
        ' also if we are fading into an intro, get the song that the intro would continue to
        If _isFadingOut Then
            If _fadeIntoIntro Then
                Return _introContinueSong
            Else
                Return _nextSong
            End If
        Else
            If _isIntroStarted Then
                Return _introContinueSong
            Else
                Return _currentSongName
            End If
        End If
    End Function

    Private Shared Function GetSong(songName As String) As SongContainer
        Dim key = GetSongName(songName)

        If _songs.ContainsKey(key) = True Then
            Return _songs(key)
        Else
            Dim defaultSongFilePath = GameController.GamePath & "\Content\" & "Songs\" & key & ".ogg"
            Dim songFilePath = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\" & key & ".ogg"
            If File.Exists(songFilePath) Then
                If AddSong(key, False) = True Then
                    Return _songs(key)
                End If
            ElseIf File.Exists(defaultSongFilePath) Then
                If AddSong(key, False) = True Then
                    Return _songs(key)
                End If
            Else
                Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Cannot find music file """ & songName & """. Return nothing.")
            End If
            Return Nothing
        End If

    End Function


    Private Shared Function GetSongName(song As String) As String
        Dim key = song.ToLowerInvariant()
        Dim aliasMap = SongAliasMap
        If aliasMap.ContainsKey(key) Then
            key = aliasMap(key).ToLowerInvariant()
        End If
        Return key
    End Function

    Private Shared Function AddSong(ByVal Name As String, ByVal forceReplace As Boolean) As Boolean
        Try
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & Name, ".ogg")

            Dim loadSong As Boolean = False
            Dim removeSong As Boolean = False

            If _songs.ContainsKey(GetSongName(Name)) = False Then
                loadSong = True
            ElseIf forceReplace = True And _songs(GetSongName(Name)).IsStandardSong = True Then
                removeSong = True
                loadSong = True
            End If

            If loadSong = True Then
                Dim songFilePath As String = Nothing

                If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".ogg") = True Then
                    songFilePath = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".ogg"
                Else
                    Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Song at """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & """ was not found!")
                    Return False
                End If

                If Not songFilePath Is Nothing Then
                    If removeSong = True Then
                        _songs.Remove(GetSongName(Name))
                    End If
                    Dim Duration = GetSongDuration(songFilePath)
                    _songs.Add(GetSongName(Name), New SongContainer(songFilePath, Name, Duration, cContent.RootDirectory))
                End If
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: File at ""Songs\" & Name & """ is not a valid song file!")
            Return False
        End Try
        Return True
    End Function

    Public Shared Sub LoadMusic(ByVal forceReplace As Boolean)
        For Each musicFile As String In System.IO.Directory.GetFiles(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\", "*.*", IO.SearchOption.AllDirectories)
            If musicFile.EndsWith(".ogg") = True Then
                Dim isIntro As Boolean = False
                If musicFile.Contains("\Songs\intro\") = True Then
                    isIntro = True
                End If

                If isIntro = False Then
                    musicFile = System.IO.Path.GetFileNameWithoutExtension(musicFile)
                Else
                    musicFile = "intro\" & System.IO.Path.GetFileNameWithoutExtension(musicFile)
                End If

                AddSong(musicFile, forceReplace)
            End If
        Next
        If Core.GameOptions.ContentPackNames.Count > 0 Then
            For Each c As String In Core.GameOptions.ContentPackNames
                Dim path As String = GameController.GamePath & "\ContentPacks\" & c & "\Songs\"
                If System.IO.Directory.Exists(path) = True Then
                    For Each musicFile As String In System.IO.Directory.GetFiles(path, "*.*", IO.SearchOption.AllDirectories)
                        If musicFile.EndsWith(".ogg") = True Then
                            Dim isIntro As Boolean = False
                            If musicFile.Contains("\Songs\intro\") = True Then
                                isIntro = True
                            End If

                            If isIntro = False Then
                                musicFile = System.IO.Path.GetFileNameWithoutExtension(musicFile)
                            Else
                                musicFile = "intro\" & System.IO.Path.GetFileNameWithoutExtension(musicFile)
                            End If
                            AddSong(musicFile, forceReplace)
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Shared Function GetSongDuration(songFilePath As String) As TimeSpan

        Dim DurationFile As VorbisWaveReader = New VorbisWaveReader(songFilePath)
        Dim SongDuration = DurationFile.TotalTime
        DurationFile.Dispose()
        Return SongDuration

    End Function

    Private Shared ReadOnly Property SongAliasMap As Dictionary(Of String, String)
        Get
            Return New Dictionary(Of String, String)() From
            {
                {"welcome", "RouteMusic1"},
                {"battle", "johto_wild"},
                {"batleintro", "battle_intro"},
                {"johto_battle_intro", "battle_intro"},
                {"darkcave", "dark_cave"},
                {"showmearound", "show_me_around"},
                {"sprouttower", "sprout_tower"},
                {"johto_rival_intro", "johto_rivalintro"},
                {"johto_rival_appear", "johto_rival_encounter"},
                {"ilex_forest", "IlexForest"},
                {"union_cave", "IlexForest"},
                {"mt_mortar", "IlexForest"},
                {"whirlpool_islands", "IlexForest"},
                {"tohjo_falls", "IlexForest"}
            }
        End Get
    End Property

End Class
