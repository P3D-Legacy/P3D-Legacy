Public Class HalfDiagonalWallModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 18

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)), 'AB
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), New Vector3(0, 0, 1), New Vector2(0.0, 0.0)), 'CD
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)), 'AB
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)), 'F
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)), 'AB
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), New Vector3(0, 0, -1), New Vector2(0.0, 0.0)), 'CD
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)), 'AB
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)), 'H
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 1.0)) 'F
        }

        Setup(vertexData)
    End Sub

End Class
