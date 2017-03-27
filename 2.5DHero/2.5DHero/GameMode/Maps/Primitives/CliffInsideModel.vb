Namespace GameModes.Maps.Primitives

    Class CliffInsideModel

        Inherits PrimitiveModel

        Public Sub New()
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

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.0))) 'e
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.25, -0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 1.0))) 'h
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.25, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0))) 'a

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.25, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0))) 'a
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.0))) 'f
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.0))) 'e

            Setup()
        End Sub

    End Class

End Namespace
