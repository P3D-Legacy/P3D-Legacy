Public Class CliffModel

    Inherits BaseModel

    Public Sub New()
        vertexData.Clear()

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Up, New Vector2(0, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Up, New Vector2(1, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.25), Vector3.Forward, New Vector2(0.0, 1.0))) 'h
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), Vector3.Forward, New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.25), Vector3.Forward, New Vector2(1.0, 1.0))) 'c

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.25), Vector3.Forward, New Vector2(1.0, 1.0))) 'c
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), Vector3.Forward, New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.25, 0.5), Vector3.Forward, New Vector2(1.0, 0.0))) 'd

        Me.ID = 9

        Setup()
    End Sub

End Class