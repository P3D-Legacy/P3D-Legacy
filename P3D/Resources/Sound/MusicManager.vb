Imports Microsoft.VisualBasic
Public Class MusicManager

    Private Const NO_MUSIC As String = "*nomusic*" ' contains * as character, which cannot be in a filename
    Private Const DEFAULT_FADE_SPEED As Single = 0.02F

    Private Shared _songs As Dictionary(Of String, SongContainer) = New Dictionary(Of String, SongContainer)()
    Private Shared _volume As Single = 1.0F
    Private Shared _lastVolume As Single = 1.0F
    Private Shared _muted As Boolean = False

    ' currently playing song
    Private Shared _currentSongName As String = NO_MUSIC
    ' if the song in _currentSong is an actual existing song
    Private Shared _currentSongExists As Boolean = False
    Private Shared _currentSong As SongContainer = Nothing

    ' time until music playback is paused for sound effect
    Private Shared _pausedUntil As Date
    Private Shared _isPausedForSound As Boolean = False

    ' time until the intro of a song plays
    Private Shared _introEndTime As Date
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
                MediaPlayer.IsMuted = value

                If _muted = True Then
                    MediaPlayer.Pause()
                    Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_off"), 12, FontManager.MainFont, Color.White)
                Else
                    ResumePlayback()
                    Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_on"), 12, FontManager.MainFont, Color.White)
                End If
            End If
        End Set
    End Property

    Public Shared Sub Setup()
        MasterVolume = 1.0F
        MediaPlayer.Volume = MasterVolume
        _volume = 1.0F
        _nextSong = ""
        _fadeSpeed = DEFAULT_FADE_SPEED
        _isFadingOut = False
        _isFadingIn = False
        _muted = False
        MediaPlayer.IsRepeating = True
    End Sub

    Public Shared Sub Clear()
        _songs.Clear()
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
                _isPausedForSound = False
                ResumePlayback()
            End If
        Else

            ' fading
            If _isFadingOut Then
                _volume -= _fadeSpeed

                If _volume <= 0F Then

                    _volume = 0F
                    _isFadingOut = False
                    _isFadingIn = True

                    Dim song = GetSong(_nextSong)

                    If Not song Is Nothing Then

                        Play(song)
                        _nextSong = ""

                        If _fadeIntoIntro Then
                            _fadeIntoIntro = False
                            _introEndTime = Date.Now.AddSeconds(-1) + song.Duration
                            _isIntroStarted = True
                            MediaPlayer.IsRepeating = False
                        Else
                            MediaPlayer.IsRepeating = True
                        End If

                    Else

                        ' no song found, do not fade into anything
                        _fadeIntoIntro = False
                        ClearCurrentlyPlaying()
                        _isFadingIn = False
                        _volume = 1.0F

                    End If

                End If

            ElseIf _isFadingIn Then

                _volume += _fadeSpeed

                If _volume >= 1.0F Then
                    _volume = 1.0F
                    _isFadingIn = False
                End If

            End If

            ' intro
            If _isIntroStarted Then

                If Date.Now >= _introEndTime Then

                    Dim song = GetSong(_introContinueSong)
                    MediaPlayer.IsRepeating = True
                    _isIntroStarted = False
                    Play(song)

                End If

            End If

        End If

        If Core.GameInstance.IsActive AndAlso _lastVolume <> (_volume * MasterVolume) Then
            UpdateVolume()
        End If
    End Sub

    Public Shared Sub UpdateVolume()
        MediaPlayer.Volume = _volume * MasterVolume
        _lastVolume = _volume * MasterVolume
    End Sub

    Public Shared Sub PauseForSound(ByVal sound As SoundEffect)
        _isPausedForSound = True
        _pausedUntil = Date.Now + sound.Duration
        MediaPlayer.Pause()
    End Sub

    Public Shared Sub Pause()
        MediaPlayer.Pause()
    End Sub

    Public Shared Sub ResumePlayback()
        If Not _currentSong Is Nothing Then

            ' if an intro was playing while the media player was paused, calc its end time
            If MediaPlayer.State = MediaState.Paused AndAlso _isIntroStarted Then

                _introEndTime = Date.Now + (_currentSong.Duration - MediaPlayer.PlayPosition)

            End If

            MediaPlayer.Resume()

        End If
    End Sub

    Private Shared Sub Play(song As SongContainer)
        MediaPlayer.Stop()

        If Not song Is Nothing Then
            Logger.Debug($"Play song [{song.Name}]")

            MediaPlayer.Play(song.Song)

            If MediaPlayer.IsMuted Then
                MediaPlayer.Pause()
            End If

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

    Public Shared Function Play(song As String, playIntro As Boolean) As SongContainer
        Return Play(song, playIntro, DEFAULT_FADE_SPEED)
    End Function

    Public Shared Function Play(song As String, playIntro As Boolean, fadeSpeed As Single) As SongContainer

        Dim playedSong As SongContainer = Nothing

        ' get the current song, only play if it's different
        Dim currentSong = GetCurrentSong().ToLowerInvariant()
        Dim songName = GetSongName(song)

        If currentSong = NO_MUSIC OrElse currentSong <> songName Then

            If playIntro Then

                Dim introSong = GetSong("intro\" + songName)
                If Not introSong Is Nothing Then

                    ' play the intro
                    ' setup the continue song
                    _introContinueSong = songName
                    ' do not repeat media player, do not want intro to loop
                    MediaPlayer.IsRepeating = False

                    If fadeSpeed > 0F Then
                        _isIntroStarted = False
                        _fadeIntoIntro = True
                        FadeInto(introSong, fadeSpeed)
                    Else
                        _isIntroStarted = True
                        _introEndTime = Date.Now.AddSeconds(-1) + introSong.Duration
                        Play(introSong)
                    End If

                    playedSong = introSong

                    ' load the next song so the end of the intro doesn't lag
                    GetSong(song)

                End If

            End If

            ' intro was not requested or does not exist
            If Not _isIntroStarted AndAlso Not _fadeIntoIntro Then

                Dim nextSong = GetSong(song)
                If fadeSpeed > 0F Then
                    FadeInto(nextSong, fadeSpeed)
                Else
                    Play(nextSong)
                End If
                MediaPlayer.IsRepeating = True
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

        Dim iSong As SongContainer = Nothing
        If Not _songs.TryGetValue(key, iSong) Then

            Dim songFilePath = Path.Combine(GameController.GamePath, "Content", "Songs", key + ".mp3")

            If File.Exists(songFilePath) Then
                Dim duration = GetSongDuration(songFilePath)
                iSong = New SongContainer(Song.FromUri(key, New Uri(songFilePath)), key, duration)
                _songs.Add(key, iSong)
            End If

        End If

        Return iSong

    End Function

    Private Shared Function GetSongDuration(fileName As String) As TimeSpan

        Dim duration As Double = 0
        Dim sampleFrequency = 0

        Using stream = New FileStream(fileName, FileMode.Open)
            Dim frame = NAudio.Wave.Mp3Frame.LoadFromStream(stream)
            If Not frame Is Nothing Then

                sampleFrequency = frame.SampleRate

            End If

            While Not frame Is Nothing

                duration += frame.SampleCount / sampleFrequency
                frame = NAudio.Wave.Mp3Frame.LoadFromStream(stream)

            End While

        End Using

        Dim seconds = CType(duration, Integer)
        Dim milliseconds = CType((duration - seconds) * 1000, Integer)

        Return New TimeSpan(0, 0, 0, seconds, milliseconds)

    End Function

    Private Shared Function GetSongName(song As String) As String
        Dim key = song.ToLowerInvariant()
        Dim aliasMap = SongAliasMap
        If aliasMap.ContainsKey(key) Then
            key = aliasMap(key).ToLowerInvariant()
        End If
        Return key
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
