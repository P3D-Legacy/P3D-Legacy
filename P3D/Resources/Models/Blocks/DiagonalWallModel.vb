Public Class DiagonalWallModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 17

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)), 'A
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.0)), 'C
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)), 'A
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)), 'F
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)), 'A
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, -1), New Vector2(0.0, 0.0)), 'C
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)), 'A
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 1.0)) 'F
        }

        Setup(vertexData)
    End Sub

End Class
