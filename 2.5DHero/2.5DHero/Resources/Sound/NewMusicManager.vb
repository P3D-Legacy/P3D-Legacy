''' <summary>
''' Manages song resources.
''' </summary>
Public Class NewMusicManager

    'This music needs to be loaded, in this order:
    'Active GameMode
    'All active ContentPacks
    '
    'If a music file gets found in a ContentPack that also exists in the GameMode or in another ContentPack, it overwrites the already added file.
    'Intros to songs are not required, but have the default folder called "intro", which songs do not get pulled from unless they have a song in the corresponding main folder.
    'Intros only play if the corresponding song was loaded from the same origin (GameMode/ContentPack).
    '
    'Accepted file formats are .xnb (with .wma file) and .ogg.

    'This maps the song files.
    Private Shared _songs As New Dictionary(Of String, Song)

    ''' <summary>
    ''' Returns if a song exists based on its name.
    ''' </summary>
    ''' <param name="songName">The song name.</param>
    ''' <returns></returns>
    Public Shared Function SongExists(ByVal songName As String) As Boolean
        Return Not IsNothing(GetSong(songName))
    End Function

    ''' <summary>
    ''' Returns a Song object from a song's name.
    ''' </summary>
    ''' <param name="songName">The name of the song.</param>
    ''' <returns></returns>
    Public Shared Function GetSong(ByVal songName As String) As Song
        Try
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & songName, ".xnb,.ogg")

            Dim tKey As String = cContent.RootDirectory & "\Songs\" & songName

            If _songs.ContainsKey(tKey.ToLower()) = False Then
                Dim s As Song = Nothing

                If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & songName & ".xnb") = False AndAlso System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & songName & ".wma") = False Then
                    If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & songName & ".ogg") = True Then
                        Dim ctor = GetType(Song).GetConstructor(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance, Nothing, {GetType(String), GetType(String), GetType(Integer)}, Nothing)
                        Dim filePath As String = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & songName & ".ogg"
                        s = CType(ctor.Invoke({songName, filePath, 0}), Song)
                    Else
                        Logger.Log("296", Logger.LogTypes.ErrorMessage, "NewMusicManager.vb: Song """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & songName & """ was not found!")
                        Return Nothing
                    End If
                Else
                    s = cContent.Load(Of Song)("Songs\" & songName)
                End If

                _songs.Add(tKey.ToLower(), s)
            End If

            Return _songs(tKey.ToLower())
        Catch ex As Exception
            Logger.Log("269", Logger.LogTypes.Warning, "NewMusicManager.vb: File at ""Songs\" & songName & """ is not a valid song file!")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Returns if there is a corresponding intro song to a song name.
    ''' </summary>
    ''' <param name="songName">The song name to search an intro for.</param>
    ''' <returns></returns>
    Public Shared Function HasIntro(ByVal songName As String) As Boolean
        'To check if an intro exists, we grab the ContentManager of the root song first.
        'Then, we check if the intro file exists relative to the ContentManager's RootDirectory.

        Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & songName, ".xnb,.ogg")

        Return (System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\intro\" & songName & ".xnb") = True AndAlso
            (System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\intro\" & songName & ".ogg")) OrElse
            System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\intro\" & songName & ".wma"))
    End Function

    ''' <summary>
    ''' Clears the Music Manager song enumeration.
    ''' </summary>
    Public Shared Sub Clear()
        _songs.Clear()
    End Sub

End Class

''' <summary>
''' Disc Jockey of the Pokemon3D.
''' </summary>
Public Class MusicPlayer

#Region "Singleton Handler"

    Private Shared _singleton As MusicPlayer = Nothing

    ''' <summary>
    ''' Returns the active instance of the MusicPlayer.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetInstance() As MusicPlayer
        If _singleton Is Nothing Then
            _singleton = New MusicPlayer()
        End If
        Return _singleton
    End Function

    Private Sub New()
        MediaPlayer.IsRepeating = True
    End Sub

#End Region

    'If PlayMusic is called, with fade in/out, then set the song as _fadeFollowUp, set _fadeOut to True.
    'If an intro for this song should be played, then set the intro to _fadeFollowUp and the real song as _introFollowUp.
    'Update until volume is 0, then set _fadeIn to True.

    Private _currentSong As String = ""

    Private _pausedForSound As Boolean = False 'If the music player is pausing and waiting for a sound to play.
    Private _waitForSound As TimeSpan

    'Intro controlling:
    Private _introRemaining As TimeSpan 'The amount of time the intro is still playing for.
    Private _introPlaying As Boolean = False
    Private _introFollowUp As String = "" 'The song that will play once the intro is over.

    'Fade controlling:
    Private _fadeFollowUp As String = "" 'The song that will play once the fading volume reached 0 and will increase again.
    Private _fadeOutSpeed As Single
    Private _fadeInSpeed As Single
    Private _fadeOut As Boolean = False
    Private _fadeIn As Boolean = False
    Private _fadeFollowUpIntro As Boolean = False 'If the follow up song also needs an intro played.

    'Volume controlling:
    Private _volume As Single = 1.0F 'internal volume to control fading.
    Private _masterVolume As Single = 1.0F 'master volume

    Private _currentVolume As Single = -1.0F 'To not call MediaPlayer.Volume each frame, we store a temporary value that is up to date with the current volume.

    ''' <summary>
    ''' The master volume of the music player.
    ''' </summary>
    ''' <returns></returns>
    Public Property MasterVolume() As Single
        Get
            Return _masterVolume
        End Get
        Set(value As Single)
            _masterVolume = value
        End Set
    End Property

    ''' <summary>
    ''' The song currently played. If no song is played, this value is empty.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CurrentSong() As String
        Get
            Return _currentSong
        End Get
    End Property

    ''' <summary>
    ''' Updates the music player.
    ''' </summary>
    ''' <param name="gameTime">The game time that has elapsed.</param>
    Public Sub Update(ByVal gameTime As GameTime)
        If _pausedForSound = False Then
            UpdateIntro(gameTime)

            UpdateFading()
        Else
            _waitForSound -= gameTime.ElapsedGameTime
            If _waitForSound.TotalMilliseconds() <= 0 Then
                _pausedForSound = False
                MediaPlayer.Resume()
            End If
        End If

        If Core.GameInstance.IsActive = True And _currentVolume <> (_volume * _masterVolume) Then
            ForceVolumeUpdate()
        End If
    End Sub

    Private Sub UpdateIntro(ByVal gameTime As GameTime)
        If _introPlaying = True Then
            _introRemaining -= gameTime.ElapsedGameTime
            If _introRemaining.TotalMilliseconds() <= 0 Then
                _introPlaying = False

                PlayTrack(_introFollowUp, False)
            End If
        End If
    End Sub

    Private Sub UpdateFading()
        If _fadeIn = True Then
            _volume += _fadeInSpeed
            If _volume >= 1.0F Then
                _volume = 1.0F
                _fadeIn = False
            End If
        End If
        If _fadeOut = True Then
            _volume -= _fadeOutSpeed
            If _volume <= 0F Then
                _volume = 0F
                _fadeOut = False

                PlayTrack(_fadeFollowUp, _fadeFollowUpIntro)

                'If the fade in speed is larger than 0, fade in, otherwise play at full volume.
                If _fadeInSpeed > 0F Then
                    _fadeIn = True
                Else
                    _volume = 1.0F
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Forces a volume update to the Media Player.
    ''' </summary>
    Public Sub ForceVolumeUpdate()
#If WINDOWS Then
        Try
            MediaPlayer.Volume = _volume * _masterVolume
        Catch ex As NullReferenceException
            ' song changed while changing volume
        End Try
#Else
        MediaPlayer.Volume = _volume * _masterVolume
#End If
        _currentVolume = _volume * _masterVolume
    End Sub

    Private Sub PlayTrack(ByVal Song As String, ByVal SearchForIntro As Boolean)
        Dim hasIntro As Boolean = False
        If SearchForIntro = True Then
            If NewMusicManager.HasIntro(Song) = True Then
                hasIntro = True
            End If
        End If

        If hasIntro = True Then
            Dim s As Song = NewMusicManager.GetSong("intro\" & Song)

            _introFollowUp = Song
            _introPlaying = True
            _introRemaining = s.Duration - TimeSpan.FromSeconds(1)

            PlayTrack(s, Song)
        Else
            Dim s As Song = NewMusicManager.GetSong(Song)

            _introPlaying = False

            PlayTrack(s, Song)
        End If
    End Sub

    Private Sub PlayTrack(ByVal Song As Song, ByVal songName As String)
        If CanPlayMusic() = True And Song IsNot Nothing Then
            MediaPlayer.Play(Song)

            _currentSong = songName

            'This will also change the play state of the player, not just switch the song.
            'If we are waiting for a sound to finish right now, pause the player.,
            If _pausedForSound = True Then
                MediaPlayer.Pause()
            End If
        Else
            'This song cannot be played, stop audio playback:
            [Stop]()
        End If
    End Sub

    ''' <summary>
    ''' Plays a song.
    ''' </summary>
    ''' <param name="Song">The song name.</param>
    Public Sub Play(ByVal Song As String)
        Play(Song, False, 0F, 0F)
    End Sub

    ''' <summary>
    ''' Plays a song.
    ''' </summary>
    ''' <param name="Song">The song name.</param>
    ''' <param name="PlayIntro">If an intro should be played when available.</param>
    Public Sub Play(ByVal Song As String, ByVal PlayIntro As Boolean)
        Play(Song, PlayIntro, 0.02F, 0.02F)
    End Sub

    ''' <summary>
    ''' Plays a song with set fade speeds and intro settings.
    ''' </summary>
    ''' <param name="Song">The song to play</param>
    ''' <param name="PlayIntro">If an intro should be played when available.</param>
    ''' <param name="NewFadeInSpeed">The fade in speed.</param>
    ''' <param name="NewFadeOutSpeed">The fade out speed.</param>
    Public Sub Play(ByVal Song As String, ByVal PlayIntro As Boolean, ByVal NewFadeInSpeed As Single, ByVal NewFadeOutSpeed As Single)
        If _fadeOut = True Then
            If _fadeFollowUp.ToLower() <> Song.ToLower() Then
                _fadeFollowUp = Song
                _fadeFollowUpIntro = PlayIntro
            End If
        Else
            If _currentSong.ToLower() <> Song.ToLower() Then
                If NewFadeOutSpeed > 0F Then
                    _fadeOutSpeed = NewFadeOutSpeed
                    _fadeInSpeed = NewFadeInSpeed
                    _fadeOut = True
                    _fadeIn = False
                    _fadeFollowUp = Song
                    _fadeFollowUpIntro = PlayIntro
                Else
                    'Directly play song:
                    PlayTrack(Song, PlayIntro)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' This stops all music playback.
    ''' </summary>
    Public Sub [Stop]()
        MediaPlayer.Stop()
        _currentSong = ""
        _introPlaying = False
        _fadeIn = False
        _fadeOut = False
    End Sub

    ''' <summary>
    ''' Pauses the media player.
    ''' </summary>
    Public Sub Pause()
        MediaPlayer.Pause()
    End Sub

    ''' <summary>
    ''' Resumes the media player.
    ''' </summary>
    Public Sub [Resume]()
        MediaPlayer.Resume()
    End Sub

    ''' <summary>
    ''' Pauses the music for a sound effect to play.
    ''' </summary>
    ''' <param name="s">The sound effect.</param>
    Public Sub PauseForSound(ByVal s As SoundEffect)
        _waitForSound = s.Duration
        _pausedForSound = True
        MediaPlayer.Pause()
    End Sub

    ''' <summary>
    ''' Mutes the music player.
    ''' </summary>
    ''' <param name="mute"></param>
    Public Sub Mute(ByVal mute As Boolean)
        If MediaPlayer.IsMuted <> mute Then
            MediaPlayer.IsMuted = mute

            If MediaPlayer.IsMuted = True Then
                MediaPlayer.Pause()
                Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_off"), 12, FontManager.MainFont, Color.White)
            Else
                If _currentSong <> "" Then
                    MediaPlayer.Resume()
                End If
                Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_on"), 12, FontManager.MainFont, Color.White)
            End If
        End If
    End Sub

#Region "Can play music"

    Private Declare Function GetAudioOutputDevices Lib "winmm.dll" Alias "waveOutGetNumDevs" () As Integer

    ''' <summary>
    ''' Returns if Pokémon3D is able to play music on the computer it is running on.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanPlayMusic() As Boolean
        Dim errorMessage As String = ""

        Dim audioDeviceCount As Integer = GetAudioOutputDevices()
        If audioDeviceCount > 0 Then
            Try
                Dim r As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MediaPlayer\PlayerUpgrade", "PlayerVersion", Nothing).ToString()
                If r <> "" And r.Contains(",") = True Then
                    Dim version As String = r.Remove(r.IndexOf(","))
                    If IsNumeric(version) = True Then
                        If CInt(version) >= 11 Then
                            Return True
                        Else
                            errorMessage = "The installed version of the WindowsMediaPlayer (" & r & ") is smaller than 12."
                        End If
                    Else
                        errorMessage = "The registry string doesn't start with a numeric value."
                    End If
                Else
                    errorMessage = "The registry string doesn't contain "","" or is empty."
                End If
            Catch
                errorMessage = "Unknown error"
            End Try
        Else
            errorMessage = "No audio output device is connected to the computer."
        End If

        Logger.Log("271", Logger.LogTypes.Warning, "MusicManager.vb: An error occurred trying to play music: " & errorMessage)

        If Core.GameOptions.ForceMusic = True Then
            Logger.Log("272", Logger.LogTypes.Message, "MusicManager.vb: Forced music to play and ignore the error occuring in music validation. Set ForceMusic to ""0"" in the options file to disable this.")
            Return True
        End If

        Return False
    End Function

#End Region

End Class