Public Class SoundManager

    Const POKEMON_CRY_VOLUME_MULTIPLIER As Single = 0.6F

    Shared _sounds As Dictionary(Of String, SoundEffect) = New Dictionary(Of String, SoundEffect)

    Public Shared Volume As Single = 1.0F
    Public Shared Muted As Boolean = False

    Private Declare Function GetAudioOutputDevices Lib "winmm.dll" Alias "waveOutGetNumDevs" () As Integer
    Private Shared Function HasOutputDeviceAvailable() As Boolean
        Return GetAudioOutputDevices() > 0
    End Function

    Public Shared Sub Clear()
        _sounds.Clear()
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
            Dim sound As SoundEffect = Nothing
            If Not _sounds.TryGetValue(key, sound) Then

                ' load sound
                Dim filePath = Path.Combine(GameController.GamePath, "Content\Sounds", soundFile & ".wav")
                If File.Exists(filePath) Then
                    Using stream As New FileStream(filePath, FileMode.OpenOrCreate)
                        Try
                            sound = SoundEffect.FromStream(stream)
                            _sounds.Add(key, sound)
                        Catch ex As Exception
                            Logger.Log(Logger.LogTypes.ErrorMessage, "Failed to load sound at """ & soundFile & """: " & ex.Message)
                        End Try
                    End Using
                End If

            End If

            If sound IsNot Nothing Then

                If HasOutputDeviceAvailable() Then

                    Logger.Debug("SoundEffect [" & soundFile & "]")

                    sound.Play(volume, pitch, pan)

                    If stopMusic = True Then
                        MusicManager.PauseForSound(sound)
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

End Class