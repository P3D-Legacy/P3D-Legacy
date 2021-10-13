Public Class BAEntityTextureChange

    Inherits BattleAnimation3D

    Public Texture As Texture2D
    Public TargetEntity As Entity

    Public Sub New(ByVal Entity As Entity, Texture As Texture2D, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

        Me.TargetEntity = Entity
        Me.Texture = Texture
        Me.AnimationType = AnimationTypes.Texture
    End Sub

    Public Overrides Sub DoActionActive()
        TargetEntity.Textures = {Me.Texture}
        Me.Ready = True
    End Sub

End Class