Public Class DiagonalWallModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 17

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Backward, New Vector2(0, 1)), 'B
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), Vector3.Backward, New Vector2(0, 0)), 'D
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), Vector3.Backward, New Vector2(1, 0)), 'G
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), Vector3.Backward, New Vector2(1, 0)), 'G
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Backward, New Vector2(1, 1)), 'F
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Backward, New Vector2(0, 1)) 'B
        }

        Setup(vertexData)
    End Sub

End Class
