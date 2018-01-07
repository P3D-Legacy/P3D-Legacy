Public Class StairsModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 16

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.0)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.5)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.5))
        }

        Setup(vertexData)
    End Sub

End Class
