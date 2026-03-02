Public Class BAPlaySound

    Inherits BattleAnimation3D

    Dim SoundFile As String
    Dim StopMusic As Boolean
    Dim IsPokemon As Boolean
    Dim CrySuffix As String = ""


    Public Sub New(ByVal Sound As String, ByVal StartDelay As Single, ByVal EndDelay As Single, Optional ByVal StopMusic As Boolean = False, Optional ByVal IsPokemon As Boolean = False, Optional ByVal CrySuffix As String = "")
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), StartDelay, EndDelay)
        Me.Scale = New Vector3(1.0F)
        SoundFile = Sound
        Me.Visible = False
        Me.StopMusic = StopMusic
        Me.IsPokemon = IsPokemon
        Me.CrySuffix = CrySuffix

        AnimationType = AnimationTypes.Sound
    End Sub

    Public Overrides Sub DoActionActive()
        If IsPokemon = True Then
            SoundManager.PlayPokemonCry(CInt(SoundFile), Me.CrySuffix)
        Else
            SoundManager.PlaySound(SoundFile, StopMusic)
        End If
        Me.Ready = True
    End Sub
End Class