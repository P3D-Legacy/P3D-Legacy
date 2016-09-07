Public Class CubeModel

    Inherits BaseModel

    Public Sub New()
        vertexData.Clear()

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0))) 'h
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0))) 'c

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0))) 'c
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0))) 'd

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0))) 'c
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 0.0))) 'd
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 1.0))) 'b

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 1.0))) 'b
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 0.0))) 'd
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.0))) 'g

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 1.0))) 'h
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0))) 'a

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0))) 'a
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.0))) 'f
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.0))) 'e

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0))) 'f
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 1.0))) 'a
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0))) 'b

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0))) 'b
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 0.0))) 'g
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0))) 'f

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), Vector3.Up, New Vector2(0, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), Vector3.Up, New Vector2(0, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), Vector3.Up, New Vector2(1, 0)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), Vector3.Up, New Vector2(1, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0.5), Vector3.Up, New Vector2(1, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0.5), Vector3.Up, New Vector2(0, 1)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Down, New Vector2(0, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Down, New Vector2(0, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Down, New Vector2(1, 0)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Down, New Vector2(1, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Down, New Vector2(1, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Down, New Vector2(0, 1)))

        Me.ID = 12

        Setup()
    End Sub

End Class