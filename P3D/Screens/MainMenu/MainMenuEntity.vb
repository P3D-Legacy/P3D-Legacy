Namespace Screens.MainMenu

    Public Class MainMenuEntity

        Inherits GameDevCommon.Rendering.Base3DObject(Of VertexPositionNormalTexture)

        Public Position As Vector3
        Public Rotation As Vector3
        Public ToBeRemoved As Boolean

        Public Sub New(position As Vector3)
            Me.Position = position
            Me.Rotation = Vector3.Zero
        End Sub

        Protected Overrides Sub CreateWorld()
            World = Matrix.CreateRotationX(Rotation.X) *
                Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) *
                Matrix.CreateTranslation(Position)
        End Sub

    End Class

End Namespace
