Public Class BAPlaySound

    Inherits BattleAnimation3D

    Dim soundfile As String
    Dim stopMusic As Boolean
    Dim IsPokemon As Boolean
    Dim CrySuffix As String = ""


    Public Sub New(ByVal sound As String, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal stopMusic As Boolean = False, Optional ByVal IsPokemon As Boolean = False, Optional ByVal CrySuffix As String = "")
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.Scale = New Vector3(1.0F)
        soundfile = sound
        Me.Visible = False
        Me.stopMusic = stopMusic
        Me.IsPokemon = IsPokemon
        Me.CrySuffix = CrySuffix

        AnimationType = AnimationTypes.Sound
    End Sub

    Public Overrides Sub DoActionActive()
        If IsPokemon = True Then
            SoundManager.PlayPokemonCry(CInt(soundfile), Me.CrySuffix)
        Else
            SoundManager.PlaySound(soundfile, stopMusic)
        End If
        Me.Ready = True
    End Sub
End Class