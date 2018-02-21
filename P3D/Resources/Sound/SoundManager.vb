Public Class SoundManager

    Shared Muted As Boolean = False

    Private Shared SoundFiles As New Dictionary(Of String, CSound)
    Public Shared Volume As Single = 1.0F

    Class CSound

        Private _sound As SoundEffect
        Private _origin As String

        Public Sub New(ByVal Sound As SoundEffect, ByVal Origin As String)
            Me._sound = Sound
            Me._origin = Origin
        End Sub

        Public Property Sound() As SoundEffect
            Get
                Return Me._sound
            End Get
            Set(value As SoundEffect)
                Me._sound = value
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

    Private Shared Function AddSound(ByVal Name As String, ByVal forceReplace As Boolean) As Boolean
        Try
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Sounds\" & Name, ".xnb,.wav")

            Dim loadSound As Boolean = False
            Dim removeSound As Boolean = False

            If SoundFiles.ContainsKey(Name.ToLower()) = False Then
                loadSound = True
            ElseIf forceReplace = True And SoundFiles(Name.ToLower()).IsStandardSong = True Then
                removeSound = True
                loadSound = True
            End If

            If loadSound = True Then
                Dim sound As SoundEffect = Nothing

                If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & Name & ".xnb") = False Then
                    If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & Name & ".wav") = True Then
                        Using stream As System.IO.Stream = System.IO.File.Open(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & Name & ".wav", IO.FileMode.OpenOrCreate)
                            sound = SoundEffect.FromStream(stream)
                        End Using
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: Sound at """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Songs\" & Name & """ was not found!")
                        Return False
                    End If
                Else
                    sound = cContent.Load(Of SoundEffect)("Sounds\" & Name)
                End If

                If Not sound Is Nothing Then
                    If removeSound = True Then
                        SoundFiles.Remove(Name.ToLower())
                    End If
                    SoundFiles.Add(Name.ToLower(), New CSound(sound, cContent.RootDirectory))
                End If
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: File at ""Sounds\" & Name & """ is not a valid sound file. They have to be a PCM wave file, mono or stereo, 8 or 16 bit and have to have a sample rate between 8k and 48k Hz.")
            Return False
        End Try
        Return True
    End Function

    Private Shared Function GetSoundEffect(ByVal Name As String) As SoundEffect
        Select Case Name.ToLower()
            Case "healing"
                Name = "pokemon_heal"
        End Select

        If SoundFiles.ContainsKey(Name.ToLower()) = True Then
            Return SoundFiles(Name.ToLower()).Sound
        Else
            If TryAddGameModeSound(Name) = True Then
                Return SoundFiles(Name.ToLower()).Sound
            Else
                Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: Cannot find sound file """ & Name & """. Return nothing.")
                Return Nothing
            End If
        End If
    End Function

    Private Shared Function TryAddGameModeSound(ByVal Name As String) As Boolean
        Dim soundfile As String = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Sounds\" & Name & ".xnb"
        If System.IO.File.Exists(soundfile) = True Then
            Return AddSound(Name, False)
        End If
        Return False
    End Function

    Public Shared Sub LoadSounds(ByVal forceReplace As Boolean)
        For Each soundfile As String In System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Sounds\")
            If soundfile.EndsWith(".xnb") = True Then
                soundfile = System.IO.Path.GetFileNameWithoutExtension(soundfile)
                AddSound(soundfile, forceReplace)
            End If
        Next
        If Core.GameOptions.ContentPackNames.Count > 0 Then
            For Each c As String In Core.GameOptions.ContentPackNames
                Dim path As String = GameController.GamePath & "\ContentPacks\" & c & "\Sounds\"

                If System.IO.Directory.Exists(path) = True Then
                    For Each soundfile As String In System.IO.Directory.GetFiles(path, "*.*", IO.SearchOption.AllDirectories)
                        If soundfile.EndsWith(".xnb") = True Then
                            soundfile = System.IO.Path.GetFileNameWithoutExtension(soundfile)
                            AddSound(soundfile, forceReplace)
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Public Shared Sub PlaySound(ByVal Sound As String, ByVal Pitch As Single, ByVal Pan As Single, ByVal Volume As Single, ByVal stopMusic As Boolean)
        If Muted = False Then
            Dim s As SoundEffect = Nothing

            s = GetSoundEffect(Sound)

            If Not s Is Nothing Then
                Logger.Debug("SoundEffect [" & Sound & "]")

                If CanPlaySound() = True Then
                    s.Play(Volume, Pitch, Pan)

                    If stopMusic = True Then
                        MusicManager.PauseForSound(s)
                    End If
                Else
                    Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: Failed to play sound: No audio devices found.")
                End If
            End If
        End If
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal Number As Integer)
        PlayPokemonCry(Number, 0.0F, 0F, Volume)
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal Number As Integer, ByVal Pitch As Single, ByVal Pan As Single)
        PlayPokemonCry(Number, Pitch, Pan, Volume)
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal Number As Integer, ByVal Pitch As Single, ByVal Pan As Single, ByVal Volume As Single)
        If Muted = False Then
            Dim soundfile As String = "Cries\" & Number & ".xnb"
            If GameModeManager.ContentFileExists("Sounds\" & soundfile) = True Then
                AddSound("Cries\" & Number, False)

                Dim s As SoundEffect = GetSoundEffect("Cries\" & Number.ToString())

                If CanPlaySound() = True Then
                    If Not s Is Nothing Then
                        s.Play(Volume * 0.6F, Pitch, Pan)
                    End If
                Else
                    Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: Failed to play sound: No audio devices found.")
                End If
            End If
        End If
    End Sub

    Public Shared Sub Mute(ByVal mute As Boolean)
        Muted = mute
    End Sub

    Public Shared Sub PlaySound(ByVal Sound As String)
        PlaySound(Sound, 0.0F, 0.0F, Volume, False)
    End Sub

    Public Shared Sub PlaySound(ByVal Sound As String, ByVal stopMusic As Boolean)
        PlaySound(Sound, 0.0F, 0.0F, Volume, stopMusic)
    End Sub

    Public Shared Sub ReloadSounds()
        SoundFiles.Clear()
        LoadSounds(False)
    End Sub

    Private Declare Function GetAudioOutputDevices Lib "winmm.dll" Alias "waveOutGetNumDevs" () As Integer

    Private Shared Function CanPlaySound() As Boolean
        Return GetAudioOutputDevices() > 0
    End Function

End Class