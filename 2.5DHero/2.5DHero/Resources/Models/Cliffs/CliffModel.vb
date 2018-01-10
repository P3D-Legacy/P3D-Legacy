Public Class CliffModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 9

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Up, New Vector2(0, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Up, New Vector2(1, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.25), Vector3.Forward, New Vector2(0.0, 1.0)), 'h
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), Vector3.Forward, New Vector2(0.0, 0.0)), 'e
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.25), Vector3.Forward, New Vector2(1.0, 1.0)), 'c
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.25), Vector3.Forward, New Vector2(1.0, 1.0)), 'c
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), Vector3.Forward, New Vector2(0.0, 0.0)), 'e
            New VertexPositionNormalTexture(New Vector3(0.5, -0.25, 0.5), Vector3.Forward, New Vector2(1.0, 0.0)) 'd
        }

        Setup(vertexData)
    End Sub

End Class
