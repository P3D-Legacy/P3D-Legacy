Public Class SoundManager

    Const POKEMON_CRY_VOLUME_MULTIPLIER As Single = 1.0F

    Shared _sounds As Dictionary(Of String, SoundContainer) = New Dictionary(Of String, SoundContainer)

    Public Shared Volume As Single = 1.0F
    Public Shared Muted As Boolean = False

    Private Declare Function GetAudioOutputDevices Lib "winmm.dll" Alias "waveOutGetNumDevs" () As Integer
    Private Shared Function HasOutputDeviceAvailable() As Boolean
        Return GetAudioOutputDevices() > 0
    End Function

    Private Shared Function AddSound(ByVal Name As String, ByVal forceReplace As Boolean) As Boolean
        Try
            Dim cContent As ContentManager = ContentPackManager.GetContentManager("Sounds\" & Name, ".xnb,.wav")

            Dim loadSound As Boolean = False
            Dim removeSound As Boolean = False

            If _sounds.ContainsKey(Name.ToLower()) = False Then
                loadSound = True
            ElseIf forceReplace = True And _sounds(Name.ToLower()).IsStandardSound = True Then
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
                        _sounds.Remove(Name.ToLower())
                    End If
                    _sounds.Add(Name.ToLower(), New SoundContainer(sound, cContent.RootDirectory))
                End If
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: File at ""Sounds\" & Name & """ is not a valid sound file. They have to be a PCM wave file, mono or stereo, 8 or 16 bit and have to have a sample rate between 8k and 48k Hz.")
            Return False
        End Try
        Return True
    End Function

    Public Shared Sub Clear()
        _sounds.Clear()
        LoadSounds(False)
    End Sub

    Public Shared Sub PlaySound(soundFile As String)
        PlaySound(soundFile, 0.0F, 0.0F, Volume, False)
    End Sub

    Public Shared Sub PlaySound(soundFile As String, stopMusic As Boolean)
        PlaySound(soundFile, 0.0F, 0.0F, Volume, stopMusic)
    End Sub

    Public Shared Sub PlaySound(soundFile As String, pitch As Single, pan As Single, volume As Single, stopMusic As Boolean)

        If Not Muted Then

            Dim key = soundFile.ToLowerInvariant()
            Dim sound As SoundContainer = Nothing
            sound = GetSoundEffect(key)

            If sound IsNot Nothing Then

                If HasOutputDeviceAvailable() Then

                    Logger.Debug("SoundEffect [" & soundFile & "]")

                    sound.Sound.Play(volume, pitch, pan)

                    If stopMusic = True Then
                        MusicManager.PauseForSound(sound.Sound)
                    End If

                Else

                    Logger.Log(Logger.LogTypes.ErrorMessage, "Failed to play sound: no audio device available.")

                End If

            End If

        End If

    End Sub

    Public Shared Sub PlayPokemonCry(pokemonId As Integer)
        PlaySound("Cries\" + pokemonId.ToString(), 0F, 0F, Volume * POKEMON_CRY_VOLUME_MULTIPLIER, False)
    End Sub

    Public Shared Sub PlayPokemonCry(pokemonId As Integer, pitch As Single, pan As Single)
        PlaySound("Cries\" + pokemonId.ToString(), pitch, pan, Volume * POKEMON_CRY_VOLUME_MULTIPLIER, False)
    End Sub

    Public Shared Sub PlayPokemonCry(pokemonId As Integer, pitch As Single, pan As Single, volume As Single)
        PlaySound("Cries\" + pokemonId.ToString(), pitch, pan, volume * POKEMON_CRY_VOLUME_MULTIPLIER, False)
    End Sub

    Public Shared Sub LoadSounds(ByVal forceReplace As Boolean)
        For Each soundfile As String In System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Sounds\")
            If soundfile.EndsWith(".wav") = True Then
                soundfile = System.IO.Path.GetFileNameWithoutExtension(soundfile)
                AddSound(soundfile, forceReplace)
            End If
        Next
        If Core.GameOptions.ContentPackNames.Count > 0 Then
            For Each c As String In Core.GameOptions.ContentPackNames
                Dim path As String = GameController.GamePath & "\ContentPacks\" & c & "\Sounds\"

                If System.IO.Directory.Exists(path) = True Then
                    For Each soundfile As String In System.IO.Directory.GetFiles(path, "*.*", IO.SearchOption.AllDirectories)
                        If soundfile.EndsWith(".wav") = True Then
                            soundfile = System.IO.Path.GetFileNameWithoutExtension(soundfile)
                            AddSound(soundfile, forceReplace)
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Shared Function GetSoundEffect(ByVal Name As String) As SoundContainer

        If _sounds.ContainsKey(Name.ToLower()) = True Then
            Return _sounds(Name.ToLower())
        Else
            If TryAddGameModeSound(Name) = True Then
                Return _sounds(Name.ToLower())
            Else
                Logger.Log(Logger.LogTypes.Warning, "SoundManager.vb: Cannot find sound file """ & Name & """. Return nothing.")
                Return Nothing
            End If
        End If
    End Function

    Private Shared Function TryAddGameModeSound(ByVal Name As String) As Boolean
        Dim defaultSoundFilePath As String = GameController.GamePath & "\Content\" & "Sounds\" & Name & ".wav"
        Dim soundFilePath As String = GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Sounds\" & Name & ".wav"
        If System.IO.File.Exists(soundFilePath) = True Then
            Return AddSound(Name, False)
        Else
            If System.IO.File.Exists(defaultSoundFilePath) = True Then
                Return AddSound(Name, False)
            End If
        End If
            Return False
    End Function

End Class