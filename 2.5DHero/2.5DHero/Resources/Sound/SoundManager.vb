Public Class SoundManager

#Region "Resource Management"

    Private Shared _sounds As New Dictionary(Of String, SoundEffect)

    ''' <summary>
    ''' Returns a sound object from the sound's name.
    ''' </summary>
    ''' <param name="soundName">The sound name.</param>
    ''' <returns></returns>
    Public Shared Function GetSound(ByVal soundName As String) As SoundEffect
        Dim cContent As ContentManager = ContentPackManager.GetContentManager("Sounds\" & soundName, ".xnb,.wav")

        Dim tKey As String = cContent.RootDirectory & "\Sounds\" & soundName

        If _sounds.ContainsKey(tKey.ToLower()) = False Then
            Dim s As SoundEffect = Nothing

            If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & soundName & ".xnb") = False Then
                If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & soundName & ".wav") = True Then
                    Using stream As IO.Stream = IO.File.Open(GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & soundName & ".wav", IO.FileMode.OpenOrCreate)
                        s = SoundEffect.FromStream(stream)
                    End Using
                Else
                    Logger.Log("263", Logger.LogTypes.Warning, "SoundManager.vb: Sound at """ & GameController.GamePath & "\" & cContent.RootDirectory & "\Sounds\" & soundName & """ was not found!")
                    Return Nothing
                End If
            Else
                s = cContent.Load(Of SoundEffect)("Sounds\" & soundName)
            End If

            _sounds.Add(tKey.ToLower(), s)
        End If

        Return _sounds(tKey.ToLower())
    End Function

    ''' <summary>
    ''' Clears loaded sounds.
    ''' </summary>
    Public Shared Sub Clear()
        _sounds.Clear()
    End Sub

#End Region

    Public Shared Property Volume() As Single = 1.0F

    Private Shared _muted As Boolean = False

    Public Shared Sub PlaySound(ByVal sound As String)
        InternalPlay(sound, 0F, 0F, _Volume, False)
    End Sub

    Public Shared Sub PlaySound(ByVal sound As String, ByVal stopMusic As Boolean)
        InternalPlay(sound, 0F, 0F, _Volume, stopMusic)
    End Sub

    Public Shared Sub PlaySound(ByVal sound As String, ByVal pitch As Single, ByVal pan As Single, ByVal volume As Single, ByVal stopMusic As Boolean)
        InternalPlay(sound, pitch, pan, volume, stopMusic)
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal number As Integer)
        InternalPlay("Cries\" & number.ToString(), 0F, 0F, _Volume * 0.6F, False)
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal number As Integer, ByVal pitch As Single, ByVal pan As Single)
        InternalPlay("Cries\" & number.ToString(), pitch, pan, _Volume * 0.6F, False)
    End Sub

    Public Shared Sub PlayPokemonCry(ByVal number As Integer, ByVal pitch As Single, ByVal pan As Single, ByVal volume As Single)
        InternalPlay("Cries\" & number.ToString(), pitch, pan, volume * 0.6F, False)
    End Sub

    Private Shared Sub InternalPlay(ByVal sound As String, ByVal pitch As Single, ByVal pan As Single, ByVal volume As Single, ByVal stopMusic As Boolean)
        If _muted = False Then
            Dim s As SoundEffect = GetSound(sound)
            If s IsNot Nothing Then
                If CanPlaySound() = True Then
                    s.Play(volume, pitch, pan)

                    If stopMusic = True Then
                        MusicPlayer.GetInstance().PauseForSound(s)
                    End If
                Else
                    Logger.Log("267", Logger.LogTypes.Warning, "SoundManager.vb: Failed to play sound: No audio devices found.")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Mutes the sound player.
    ''' </summary>
    ''' <param name="muted">The mute state.</param>
    Public Shared Sub Mute(ByVal muted As Boolean)
        _muted = muted
    End Sub

#Region "Can play sound"

    Private Declare Function GetAudioOutputDevices Lib "winmm.dll" Alias "waveOutGetNumDevs" () As Integer

    Private Shared Function CanPlaySound() As Boolean
        Return GetAudioOutputDevices() > 0
    End Function

#End Region

End Class