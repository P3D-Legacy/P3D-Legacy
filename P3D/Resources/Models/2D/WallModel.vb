Public Class WallModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 20

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Backward, New Vector2(0, 1)), 'B
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), Vector3.Backward, New Vector2(0, 0)), 'D
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), Vector3.Backward, New Vector2(1, 0)), 'H
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), Vector3.Backward, New Vector2(1, 0)), 'H
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Backward, New Vector2(1, 1)), 'E
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Backward, New Vector2(0, 1)) 'B
        }

        Setup(vertexData)
    End Sub

End Class
