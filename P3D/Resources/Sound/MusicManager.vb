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

                If _enableLooping And MusicManager.EnableLooping = True Then
                    _sourceStream.Position = 0
                Else
                    If Not _sourceStream.Position = 0 Then
                        If MusicManager._isCurrentlyFading = False Then
                            If MusicManager.Playlist.Count > 1 Then
                                MusicManager.Playlist.RemoveAt(0)
                            ElseIf MusicManager.EnableLooping = False AndAlso MusicManager.Playlist.Count > 0 Then
                                MusicManager.Playlist.RemoveAt(0)
                            End If
                            Dim NextSong As SongContainer = Nothing
                            If MusicManager.Playlist.Count > 0 Then
                                NextSong = MusicManager.Playlist(0)
                            End If
                            If NextSong IsNot Nothing Then
                                Logger.Debug($"Play song [{NextSong.Name}]")
                                _sourceStream.Dispose()

                                If NextSong.AudioType = ".ogg" Then
                                    _sourceStream = New VorbisWaveReader(NextSong.Song)
                                ElseIf NextSong.AudioType = ".mp3" Then
                                    _sourceStream = New Mp3FileReader(NextSong.Song)
                                ElseIf NextSong.AudioType = ".wma" Then
                                    _sourceStream = New MediaFoundationReader(NextSong.Song)
                                End If
                                _sourceStream.Position = 0
                                MusicManager._currentSongName = NextSong.Name
                                MusicManager._currentSong = NextSong
                                _enableLooping = NextSong.IsLoop
                            Else
                                _sourceStream.Dispose()
                                If MusicManager.GetSong("silence").AudioType = ".ogg" Then
                                    _sourceStream = New VorbisWaveReader(MusicManager.GetSong("silence").Song)
                                ElseIf MusicManager.GetSong("silence").AudioType = ".mp3" Then
                                    _sourceStream = New Mp3FileReader(MusicManager.GetSong("silence").Song)
                                ElseIf MusicManager.GetSong("silence").AudioType = ".wma" Then
                                    _sourceStream = New MediaFoundationReader(MusicManager.GetSong("silence").Song)
                                End If
                                _enableLooping = True
                                _sourceStream.Position = 0
                                MusicManager._currentSong = Nothing
                                MusicManager._currentSongName = "silence"
                                Logger.Debug($"Play song [silence]")
                            End If
                        End If
                    Else
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

    Private Const DEFAULT_FADE_SPEED As Single = 0.5F

    Private Shared _songs As Dictionary(Of String, SongContainer) = New Dictionary(Of String, SongContainer)()
    Public Shared Property Volume As Single = 1.0F
    Private Shared _lastVolume As Single = 1.0F
    Private Shared _muted As Boolean = False
    Private Shared _paused As Boolean = False

    Public Shared ForceMusic As String = ""
    Public Shared ReadOnly Property IsLooping As Boolean
        Get
            If Playlist IsNot Nothing AndAlso Playlist.Count <= 1 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Shared Playlist As List(Of SongContainer)
    ' currently playing song
    Public Shared _currentSongName As String = "Silence"
    ' if the song in _currentSong is an actual existing song
    Public Shared _currentSong As SongContainer = Nothing

    ' time until music playback is paused for sound effect
    Private Shared _pausedUntil As Date
    Private Shared _isPausedForSound As Boolean = False

    ' time until the intro of a song plays
    Private Shared _introMuteTime As Date
    Public Shared _introEndTime As Date
    Public Shared _isIntroStarted As Boolean = False
    ' song that gets played after the intro finished
    Public Shared _introContinueSong As String

    ' speeds that get added/subtracted from the volume to fade the song
    Private Shared _fadeSpeed As Single = DEFAULT_FADE_SPEED
    ' if the song that gets played after fading completed is an intro to another song
    Public Shared _isFadingIn As Boolean = False
    Private Shared _isFadingOut As Boolean = False
    Public Shared _isCurrentlyFading As Boolean = False
    ' NAudio properties
    Public Shared outputDevice As WaveOutEvent
    Public Shared _stream As WaveChannel32
    Public Shared EnableLooping As Boolean = True

    Public Shared Property PauseVolume As Single = 1.0F
    Public Shared Property MasterVolume As Single = 1.0F
    Public Shared ReadOnly Property CurrentSong As SongContainer
        Get
            If Playlist.Count > 0 AndAlso Playlist(0) IsNot Nothing Then
                Return Playlist(0)
            Else
                Return MusicManager.GetSong("silence")
            End If
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
        Playlist = New List(Of SongContainer)
        _fadeSpeed = DEFAULT_FADE_SPEED
        _isFadingOut = False
    End Sub

    Public Shared Sub Clear()
        _songs.Clear()
        LoadMusic(False)
    End Sub

    Public Shared Sub ClearCurrentlyPlaying()
        ' cleans all remains of currently playing songs
        Playlist.Clear()
        _currentSong = Nothing
        _currentSongName = "Silence"
        _isIntroStarted = False
        If Muted = True Then
            Volume = 0.0F
        Else
            Volume = 1.0F
        End If
        _isFadingOut = False
        Play("Silence", True, 0.0F)
    End Sub

    Public Shared Sub PlayNoMusic()
        ' fades out current track and sets to "Silence"
        Play("Silence", True, 0.01F)
    End Sub

    Public Shared Sub Update()
        If _isPausedForSound Then
            If Date.Now >= _pausedUntil Then
                If Paused = True Then
                    _isPausedForSound = False
                    Paused = False
                End If
            End If
        Else

            ' fading
            If _isFadingOut Then
                _isCurrentlyFading = True
                Volume -= _fadeSpeed

                If Volume <= 0F Then

                    Volume = 0F
                    _isFadingOut = False

                    Dim song = Playlist(0)

                    If Not song Is Nothing Then

                        Play(song)

                        If _isFadingIn Then
                            _isFadingIn = False
                            _introEndTime = Date.Now + song.Duration
                            _isIntroStarted = True
                        End If
                        If Muted = True Then
                            Volume = 0.0F
                        Else
                            Volume = 1.0F
                        End If
                        _isCurrentlyFading = False
                    Else

                        ' no song found, do not fade into anything
                        _isFadingIn = False
                        _isCurrentlyFading = False
                        ClearCurrentlyPlaying()
                        If Muted = True Then
                            Volume = 0.0F
                        Else
                            Volume = 1.0F
                        End If

                    End If

                End If

                ' intro
                '  If _isIntroStarted Then
                '      If Paused = False Then
                'If Date.Now >= _introEndTime Then
                'Dim song = GetSong(_introContinueSong)
                '_isLooping = True
                '_isIntroStarted = False
                'Play(song)
                'End If
                'End If
                'End If
            Else
                If _isFadingIn = False Then
                    _isCurrentlyFading = False
                End If
            End If
        End If

        If Core.GameInstance.IsActive AndAlso _lastVolume <> (Volume * PauseVolume * MasterVolume) Then
            UpdateVolume()
        End If
    End Sub

    Public Shared Sub UpdateVolume()
        _lastVolume = Volume * PauseVolume * MasterVolume
        If Not _stream Is Nothing Then
            _stream.Volume = Volume * PauseVolume * MasterVolume
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
        ClearCurrentlyPlaying()
    End Sub

    Public Shared Sub ResumePlayback()
        If Not CurrentSong Is Nothing Then

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

            Logger.Debug($"Play song [{song.song}]")
            If outputDevice IsNot Nothing Then
                outputDevice.Dispose()
            End If
            outputDevice = New WaveOutEvent()
            If _stream IsNot Nothing Then
                _stream.Dispose()
            End If

            If song.AudioType = ".ogg" Then
                _stream = New NAudio.Wave.WaveChannel32(New LoopStream(New VorbisWaveReader(song.Song), song.IsLoop))
            ElseIf song.AudioType = ".mp3" Then
                _stream = New NAudio.Wave.WaveChannel32(New LoopStream(New Mp3FileReader(song.Song), song.IsLoop))
            ElseIf song.AudioType = ".wma" Then
                _stream = New NAudio.Wave.WaveChannel32(New LoopStream(New MediaFoundationReader(song.Song), song.IsLoop))
            End If
            Try
                outputDevice.Init(_stream)
            Catch ex As Exception
                Logger.Log(Logger.LogTypes.ErrorMessage, "No usable audio device")
                outputDevice = Nothing
            End Try
            If outputDevice IsNot Nothing Then
                If Paused = False Then
                    outputDevice.Play()
                End If
            End If
            _stream.Volume = Volume * MasterVolume
            _currentSongName = song.Name
            _currentSong = song
        Else
            _currentSongName = "Silence"
            _currentSong = Nothing
        End If

    End Sub

    Private Shared Sub FadeInto(song As SongContainer, fadeSpeed As Single)
        _isFadingOut = True
        If Not song Is Nothing Then
            Playlist.Add(song)
        Else
            Playlist.Add(GetSong("silence"))
        End If
        _fadeSpeed = fadeSpeed
    End Sub

    Public Shared Function Play(song As String) As SongContainer
        Return Play(song, True, DEFAULT_FADE_SPEED)
    End Function

    Public Shared Function Play(song As String, playIntro As Boolean, Optional loopSong As Boolean = True) As SongContainer
        Return Play(song, playIntro, DEFAULT_FADE_SPEED, loopSong)
    End Function

    Public Shared Function Play(song As String, playIntro As Boolean, fadeSpeed As Single, Optional loopSong As Boolean = True, Optional AfterBattleIntroSong As String = "") As SongContainer

        Dim playedSong As SongContainer = Nothing

        ' get the current song, only play if it's different

        Dim currentSong = GetCurrentSong().ToLowerInvariant()
        Dim songName = GetSongName(song)
        Dim AfterBattleIntroSongName As String = GetSongName(AfterBattleIntroSong)

        If currentSong = "silence" OrElse currentSong <> songName Then
            If AfterBattleIntroSongName <> "" Then
                Dim battleIntroSong = GetSong(songName)
                Dim regularIntroSong = GetSong("intro\" + AfterBattleIntroSongName)
                Dim regularLoopSong = GetSong(AfterBattleIntroSongName)
                If battleIntroSong IsNot Nothing AndAlso battleIntroSong.Origin = regularLoopSong.Origin Then
                    Playlist.Clear()
                    Playlist.Add(battleIntroSong)
                    If regularIntroSong IsNot Nothing AndAlso regularIntroSong.Origin = regularLoopSong.Origin Then
                        Playlist.Add(regularIntroSong)
                    End If
                    If regularLoopSong IsNot Nothing Then
                        Playlist.Add(regularLoopSong)
                    End If
                    Play(battleIntroSong)
                    playedSong = battleIntroSong
                Else
                    If regularIntroSong IsNot Nothing AndAlso regularIntroSong.Origin = regularLoopSong.Origin Then
                        Playlist.Clear()
                        Playlist.Add(regularIntroSong)
                        If regularLoopSong IsNot Nothing Then
                            Playlist.Add(regularLoopSong)
                        End If
                        Play(regularIntroSong)
                        playedSong = regularIntroSong
                    ElseIf regularLoopSong IsNot Nothing Then
                        Playlist.Clear()
                        Playlist.Add(regularLoopSong)
                        Play(regularLoopSong)
                        playedSong = regularLoopSong
                    End If
                End If

            ElseIf playIntro = True Then
                Dim introSong = GetSong("intro\" + songName)
                Dim nextSong = GetSong(songName)
                If Not introSong Is Nothing Then
                    If introSong.Origin = nextSong.Origin Then
                        Playlist.Clear()
                        ' play the intro
                        If fadeSpeed > 0F Then
                            _isIntroStarted = False
                            _isFadingIn = True
                            FadeInto(introSong, fadeSpeed)
                            Playlist.Add(nextSong)
                        Else
                            _isIntroStarted = True
                            _introEndTime = Date.Now + introSong.Duration
                            Play(introSong)
                            Playlist.AddRange({introSong, nextSong})
                        End If
                        playedSong = introSong
                    Else
                        _isIntroStarted = False
                        _isFadingIn = False
                    End If
                Else
                    _isIntroStarted = False
                    _isFadingIn = False
                End If
            Else
                _isIntroStarted = False
                _isFadingIn = False
            End If
            EnableLooping = loopSong
            ' intro was not requested or does not exist
            If Not _isIntroStarted AndAlso Not _isFadingIn AndAlso AfterBattleIntroSongName = "" Then
                Playlist.Clear()
                Dim nextSong = GetSong(song)
                If fadeSpeed > 0F Then
                    FadeInto(nextSong, fadeSpeed)
                Else
                    Play(nextSong)
                    Playlist.Add(nextSong)
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
        If Playlist IsNot Nothing Then
            If Playlist.Count > 1 Then
                If _isFadingOut Then
                    If _isFadingIn Then
                        Return Playlist(0).Name
                    Else
                        If Playlist(1) IsNot Nothing Then
                            Return Playlist(1).Name
                        Else
                            Return Playlist(0).Name
                        End If
                    End If
                Else
                    If _isIntroStarted Then
                        If Playlist(1) IsNot Nothing Then
                            Return Playlist(1).Name
                        Else
                            Return Playlist(0).Name
                        End If
                    Else
                        Return Playlist(0).Name
                    End If
                End If
            ElseIf Playlist.Count = 1 Then
                Return Playlist(0).Name
            Else
                Return "Silence"
            End If
        Else
            Return "Silence"
        End If
    End Function

    Public Shared Function GetSong(songName As String) As SongContainer
        Dim key = GetSongName(songName)
        Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & key, ".ogg,.mp3,.wma")
        Dim contentSongFilePath = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & key

        Dim gamemodeSongFilePath = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\" & key
        Dim defaultSongFilePath = GameController.GamePath & "\Content\" & "Songs\" & key
        Dim audiotype = ""

        If _songs.ContainsKey(key) = True Then
            Return _songs(key)
        Else
            If System.IO.File.Exists(contentSongFilePath & ".ogg") = True OrElse System.IO.File.Exists(gamemodeSongFilePath & ".ogg") = True Or System.IO.File.Exists(defaultSongFilePath & ".ogg") = True Then
                audiotype = ".ogg"
            ElseIf System.IO.File.Exists(contentSongFilePath & ".mp3") = True OrElse System.IO.File.Exists(gamemodeSongFilePath & ".mp3") = True Or System.IO.File.Exists(defaultSongFilePath & ".mp3") = True Then
                audiotype = ".mp3"
            ElseIf System.IO.File.Exists(contentSongFilePath & ".wma") = True OrElse System.IO.File.Exists(gamemodeSongFilePath & ".wma") = True Or System.IO.File.Exists(defaultSongFilePath & ".wma") = True Then
                audiotype = ".wma"
            End If
        End If
        If File.Exists(contentSongFilePath & audiotype) Then
            If AddSong(key, False) = True Then
                Return _songs(key)
            End If
        ElseIf File.Exists(gamemodeSongFilePath & audiotype) Then
            If AddSong(key, False) = True Then
                Return _songs(key)
            End If
        ElseIf File.Exists(defaultSongFilePath & audiotype) Then
            If AddSong(key, False) = True Then
                Return _songs(key)
            End If
        Else
            If GameController.IS_DEBUG_ACTIVE = True Then
                Logger.Debug("MusicManager.vb: Cannot find music file """ & songName & """. Return nothing.")
            ElseIf songName.Contains("intro\") = False Then
                Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Cannot find music file """ & songName & """. Return nothing.")
            End If
        End If
        Return Nothing

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
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & Name, ".ogg,.mp3,.wma")

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
                Dim audioType As String = Nothing

                If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".ogg") = True Then
                    audioType = ".ogg"
                ElseIf System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".mp3") = True Then
                    audioType = ".mp3"
                ElseIf System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".wma") = True Then
                    audioType = ".wma"
                Else
                    Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Song at """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & """ was not found!")
                    Return False
                End If

                If Not audioType Is Nothing Then
                    songFilePath = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & audioType
                    If removeSong = True Then
                        _songs.Remove(GetSongName(Name))
                    End If
                    Dim Duration = GetSongDuration(songFilePath)
                    _songs.Add(GetSongName(Name), New SongContainer(songFilePath, Name, Duration, cContent.RootDirectory, audioType))
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
            If musicFile.EndsWith(".ogg") = True Or musicFile.EndsWith(".mp3") = True Or musicFile.EndsWith(".wma") = True Then
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
                        If musicFile.EndsWith(".ogg") = True Or musicFile.EndsWith(".mp3") = True Or musicFile.EndsWith(".wma") = True Then
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
        Dim DurationOGG As VorbisWaveReader = Nothing
        Dim DurationMP3 As Mp3FileReader = Nothing
        Dim DurationWMA As MediaFoundationReader = Nothing
        Dim SongDuration As TimeSpan = Nothing
        If songFilePath.Contains(".ogg") Then
            DurationOGG = New VorbisWaveReader(songFilePath)
            SongDuration = DurationOGG.TotalTime
        ElseIf songFilePath.Contains(".mp3") Then
            DurationMP3 = New Mp3FileReader(songFilePath)
            SongDuration = DurationMP3.TotalTime
        ElseIf songFilePath.Contains(".wma") Then
            DurationWMA = New MediaFoundationReader(songFilePath)
            SongDuration = DurationWMA.TotalTime
        End If

        If DurationOGG IsNot Nothing Then
            DurationOGG.Dispose()
        End If
        If DurationMP3 IsNot Nothing Then
            DurationMP3.Dispose()
        End If
        If DurationWMA IsNot Nothing Then
            DurationWMA.Dispose()
        End If
        Return SongDuration

    End Function

    Private Shared ReadOnly Property SongAliasMap As Dictionary(Of String, String)
        Get
            Return New Dictionary(Of String, String)() From
            {
                {"welcome", "RouteMusic1"},
                {"battle", "johto_wild"},
                {"batleintro", "johto_wild_intro"},
                {"johto_battle_intro", "johto_wild_intro"},
                {"darkcave", "dark_cave"},
                {"showmearound", "show_me_around"},
                {"sprouttower", "sprout_tower"},
                {"johto_rival_appear", "johto_rival_encounter"},
                {"ilex_forest", "IlexForest"},
                {"union_cave", "IlexForest"},
                {"mt_mortar", "IlexForest"},
                {"whirlpool_islands", "IlexForest"},
                {"tohjo_falls", "IlexForest"},
                {"no_music", "Silence"}
            }
        End Get
    End Property

End Class
