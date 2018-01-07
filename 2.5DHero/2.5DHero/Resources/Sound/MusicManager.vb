Public Class MusicManager

    Private Shared SongFiles As New Dictionary(Of String, CSong)

    Private Shared currentSong As String = ""

    Public Shared SongList As New List(Of String)

    Public Shared Sub Setup()
        MasterVolume = 1.0F
        MediaPlayer.Volume = MasterVolume
        Volume = 1.0F
        NextSong = ""
        FadeInSpeed = 0.0F
        FadeOutSpeed = 0.0F
        FadeOut = False
        FadeIn = False
        MediaPlayer.IsRepeating = False
    End Sub

    Class CSong

        Private _song As Song
        Private _origin As String

        Public Sub New(ByVal Song As Song, ByVal Origin As String)
            Me._song = Song
            Me._origin = Origin
        End Sub

        Public Property Song() As Song
            Get
                Return Me._song
            End Get
            Set(value As Song)
                Me._song = value
            End Set
        End Property

        Public Property Origin() As String
            Get
                Return Me._origin
            End Get
            Set(value As String)
                Me._origin = value
            End Set
        End Property

        Public ReadOnly Property IsStandardSong() As Boolean
            Get
                Return (Me.Origin = "Content")
            End Get
        End Property

    End Class

    Private Shared Function AddSong(ByVal Name As String, ByVal forceReplace As Boolean) As Boolean
        Try
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Songs\" & Name, ".xnb,.mp3,.ogg")

            Dim loadSong As Boolean = False
            Dim removeSong As Boolean = False

            If SongFiles.ContainsKey(Name.ToLower()) = False Then
                loadSong = True
            ElseIf forceReplace = True And SongFiles(Name.ToLower()).IsStandardSong = True Then
                removeSong = True
                loadSong = True
            End If

            If loadSong = True Then
                Dim song As Song = Nothing

                If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".xnb") = False Then
                    If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".mp3") = True Then
                        Dim ctor = GetType(Song).GetConstructor(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance, Nothing, {GetType(String), GetType(String), GetType(Integer)}, Nothing)
                        Dim filePath As String = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".mp3"
                        song = CType(ctor.Invoke({Name, filePath, 0}), Song)
                    ElseIf System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".ogg") = True Then
                        Dim ctor = GetType(Song).GetConstructor(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance, Nothing, {GetType(String), GetType(String), GetType(Integer)}, Nothing)
                        Dim filePath As String = GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & ".ogg"
                        song = CType(ctor.Invoke({Name, filePath, 0}), Song)
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Song at """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & """ was not found!")
                        Return False
                    End If
                Else
                    song = cContent.Load(Of Song)("Songs\" & Name)
                End If

                If Not song Is Nothing Then
                    If removeSong = True Then
                        SongFiles.Remove(Name.ToLower())
                    End If
                    SongFiles.Add(Name.ToLower(), New CSong(song, cContent.RootDirectory))
                End If
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: File at ""Songs\" & Name & """ is not a valid song file!")
            Return False
        End Try
        Return True
    End Function

    Public Shared Function GetSong(ByVal Name As String, ByVal LogErrors As Boolean) As CSong
        'Exceptions due to old definitions:
        Select Case Name.ToLower()
            Case "welcome"
                Name = "RouteMusic1"
            Case "battle"
                Name = "johto_wild"
            Case "battleintro", "johto_battle_intro"
                Name = "battle_intro"
            Case "darkcave"
                Name = "dark_cave"
            Case "showmearound"
                Name = "show_me_around"
            Case "sprouttower"
                Name = "sprout_tower"
            Case "johto_rival_intro"
                Name = "johto_rivalintro"
            Case "johto_rival_appear"
                Name = "johto_rival_encounter"
            Case "ilex_forest", "union_cave", "mt_mortar", "whirlpool_islands", "tohjo_falls"
                Name = "IlexForest"
        End Select

        If SongFiles.ContainsKey(Name.ToLower()) = True Then
            Return SongFiles(Name.ToLower())
        Else
            If TryAddGameModeMusic(Name) = True Then
                Return SongFiles(Name.ToLower())
            End If
            If Name.ToLower() <> "nomusic" Then
                Logger.Log(Logger.LogTypes.Warning, "MusicManager.vb: Cannot find music file """ & Name & """. Return nothing.")
            End If
            Return Nothing
        End If
    End Function

    Private Shared Function TryAddGameModeMusic(ByVal Name As String) As Boolean
        Dim musicfileXNB As String = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\" & Name & ".xnb"
        Dim musicfileMP3 As String = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\" & Name & ".mp3"
        Dim musicfileOGG As String = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Songs\" & Name & ".ogg"

        If System.IO.File.Exists(musicfileXNB) OrElse System.IO.File.Exists(musicfileMP3) OrElse System.IO.File.Exists(musicfileMP3) Then
            Return AddSong(Name, False)
        End If
        Return False
    End Function

    Public Shared Sub LoadMusic(ByVal forceReplace As Boolean)
        MediaPlayer.IsRepeating = True

        For Each musicFile As String In System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Songs\", "*.*", IO.SearchOption.AllDirectories)
            If musicFile.EndsWith(".xnb") = True Then
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
                        If musicFile.EndsWith(".xnb") = True Then
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

    Private Shared SongExistFlag As Boolean = True

    Public Shared Sub PlayMusic(ByVal Song As String)
        PlayMusic(Song, 0.0F, 0.0F)
    End Sub

    Public Shared Sub PlayMusic(ByVal Song As String, ByVal SearchForIntro As Boolean)
        PlayMusic(Song, SearchForIntro, 0.02F, 0.02F)
    End Sub

    Public Shared Sub PlayNoMusic()
        MediaPlayer.Stop()
        currentSong = "nomusic"
    End Sub
    Public Shared Sub IgnoreLastSong()
        currentSong = "nomusic"
        SongList.Add("nomusic")
        IntroStarted = False
    End Sub
    Public Shared Sub PlayMusic(ByVal Song As String, ByVal SearchForIntro As Boolean, ByVal NewFadeInSpeed As Single, ByVal NewFadeOutSpeed As Single)
        Dim lastSong As String = "nomusic"

        If IntroStarted = True Then
            lastSong = IntroContinue
        Else
            If SongList.Count > 0 Then
                lastSong = SongList(SongList.Count - 1)
            End If
        End If
        MediaPlayer.IsRepeating = True
        If SearchForIntro = True And lastSong.ToLower() <> Song.ToLower() Then
            If SongFiles.Keys.Contains("intro\" & Song.ToLower()) = True Then
                If SongFiles("intro\" & Song.ToLower()).Origin = SongFiles(Song.ToLower()).Origin Then
                    IntroEndTime = Date.Now.AddSeconds(-1) + SongFiles("intro\" & Song.ToLower()).Song.Duration

                    PlayMusic("intro\" & Song.ToLower(), NewFadeInSpeed, NewFadeOutSpeed)
                    MediaPlayer.IsRepeating = False
                    IntroContinue = Song
                    IntroStarted = True
                    If NewFadeInSpeed > 0.0F And NewFadeOutSpeed > 0.0F Then
                        FadeIntoIntro = True
                    End If
                Else
                    FadeIntoIntro = False
                    PlayMusic(Song, NewFadeInSpeed, NewFadeOutSpeed)
                End If
            Else
                FadeIntoIntro = False
                PlayMusic(Song, NewFadeInSpeed, NewFadeOutSpeed)
            End If
        Else
            If Not (SearchForIntro = True And IntroStarted = True And IntroContinue.ToLower() = Song.ToLower()) Then
                FadeIntoIntro = False
                PlayMusic(Song, NewFadeInSpeed, NewFadeOutSpeed)
            End If
        End If
    End Sub

    Public Shared Sub PlayMusic(ByVal Song As String, ByVal NewFadeInSpeed As Single, ByVal NewFadeOutSpeed As Single)
        If FadeOut = True Then
            If currentSong.ToLower() = Song.ToLower() And NextSong.ToLower() <> Song.ToLower() Then
                NextSong = Song.ToLower()
                Exit Sub
            End If
        End If

        If currentSong = "" Or Song.ToLower() <> currentSong.ToLower() Then
            If NewFadeInSpeed > 0.0F And NewFadeOutSpeed > 0.0F Then
                If currentSong = "" Then
                    FadeInSpeed = NewFadeInSpeed
                    FadeIn = True
                    Volume = 0.0F
                    PlayTrack(Song)
                Else
                    NextSong = Song
                    FadeInSpeed = NewFadeInSpeed
                    FadeOutSpeed = NewFadeOutSpeed
                    FadeOut = True
                    FadeIn = False
                End If
            Else
                Volume = 1.0F
                FadeIn = False
                FadeOut = False
                PlayTrack(Song)
            End If
        End If
    End Sub

    Private Shared Sub PlayTrack(ByVal song As String)
        Dim s As CSong = Nothing
        s = GetSong(song, True)

        MediaPlayer.Stop()

        Logger.Debug("Play [" & song & "]")

        IntroStarted = False

        If Not s Is Nothing Then
            MediaPlayer.Play(s.Song)

            If MediaPlayer.IsMuted = True Then
                MediaPlayer.Pause()
            End If

            SongList.Add(song)
            SongExistFlag = True
        Else
            SongList.Add("")
            SongExistFlag = False
        End If

        currentSong = song
    End Sub

    Public Shared Sub Mute(ByVal mute As Boolean)
        If MediaPlayer.IsMuted <> mute Then
            MediaPlayer.IsMuted = mute

            If MediaPlayer.IsMuted = True Then
                MediaPlayer.Pause()
                Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_off"), 12, FontManager.MainFont, Color.White)
            Else
                If SongExistFlag = True Then
                    MediaPlayer.Resume()
                End If
                Core.GameMessage.ShowMessage(Localization.GetString("game_message_music_on"), 12, FontManager.MainFont, Color.White)
            End If
        End If
    End Sub

    Public Shared Sub ReloadMusic()
        SongFiles.Clear()
        LoadMusic(False)
    End Sub

    'Intro:
    Shared StartTime As Date
    Shared PausedForSound As Boolean = False
    Shared IntroEndTime As Date
    Shared IntroStarted As Boolean = False
    Shared IntroContinue As String = ""

    'Fading:
    Shared NextSong As String

    Shared FadeInSpeed As Single
    Shared FadeOutSpeed As Single

    Shared FadeOut As Boolean = False
    Shared FadeIn As Boolean = False
    Shared FadeIntoIntro As Boolean = False

    Shared Volume As Single = 1.0F
    Public Shared MasterVolume As Single = 1.0F 'Actual volume people can change

    Public Shared Sub Update()
        If PausedForSound = True Then
            If Date.Now >= StartTime Then
                PausedForSound = False
                If MediaPlayer.IsMuted = False And SongExistFlag = True Then
                    MediaPlayer.Resume()
                End If
            End If
        Else
            UpdateFade()

            If IntroStarted = True Then
                If Date.Now >= IntroEndTime Then
                    PlayTrack(IntroContinue)  'Plays the loop that follows the intro
                    MediaPlayer.IsRepeating = True
                End If
            End If
        End If
    End Sub

    Shared LastVolume As Single = -1.0F

    Private Shared Sub UpdateFade()
        If FadeIn = True Then
            Volume += FadeInSpeed
            If Volume >= 1.0F Then
                Volume = 1.0F
                FadeIn = False
            End If
        End If
        If FadeOut = True Then
            Volume -= FadeOutSpeed
            If Volume <= 0.0F Then
                Volume = 0.0F
                FadeOut = False
                FadeIn = True

                PlayTrack(NextSong)

                If FadeIntoIntro = True Then
                    IntroStarted = True
                    IntroEndTime = Date.Now.AddSeconds(-1) + SongFiles(NextSong).Song.Duration
                    FadeIntoIntro = False
                End If

                currentSong = NextSong
                NextSong = ""
            End If
        End If

        If Core.GameInstance.IsActive = True And LastVolume <> (Volume * MasterVolume) Then
            ForceVolumeUpdate()
        End If
    End Sub

    Public Shared Sub PauseForSound(ByVal s As SoundEffect)
        StartTime = Date.Now + s.Duration
        PausedForSound = True
        MediaPlayer.Pause()
    End Sub

    Public Shared Sub StopMusic()
        MediaPlayer.Stop()
        IntroStarted = False
    End Sub

    Public Shared Sub Pause()
        MediaPlayer.Pause()
        IntroStarted = False
    End Sub

    Public Shared Sub ResumeMusic()
        MediaPlayer.Resume()
    End Sub

    Public Shared ReadOnly Property GetCurrentSong() As String
        Get
            Return SongList(SongList.Count - 1)
        End Get
    End Property

    Public Shared Function SongExists(ByVal songName As String) As Boolean
        Dim s As CSong = GetSong(songName, False)
        Return s IsNot Nothing
    End Function

    Public Shared Sub ForceVolumeUpdate()
        MediaPlayer.Volume = Volume * MasterVolume
        LastVolume = Volume * MasterVolume
    End Sub

End Class