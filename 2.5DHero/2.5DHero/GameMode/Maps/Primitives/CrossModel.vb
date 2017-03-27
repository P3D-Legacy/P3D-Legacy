Namespace GameModes.Maps.Primitives

    Class CrossModel

        Inherits PrimitiveModel

        Public Sub New()
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), Vector3.Backward, New Vector2(0, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)))

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0), Vector3.Backward, New Vector2(1, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1)))

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, -0.5), Vector3.Backward, New Vector2(0, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, -0.5), Vector3.Backward, New Vector2(0, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, 0), Vector3.Backward, New Vector2(1, 1)))

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, 0), Vector3.Backward, New Vector2(1, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, 0), Vector3.Backward, New Vector2(1, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, -0.5), Vector3.Backward, New Vector2(0, 0)))

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, 0), Vector3.Backward, New Vector2(0, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, 0), Vector3.Backward, New Vector2(0, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, 0.5), Vector3.Backward, New Vector2(1, 1)))

            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, -0.5, 0.5), Vector3.Backward, New Vector2(1, 1)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, 0.5), Vector3.Backward, New Vector2(1, 0)))
            vertexData.Add(New VertexPositionNormalTexture(New Vector3(0, 0.5, 0), Vector3.Backward, New Vector2(0, 0)))

            Setup()
        End Sub

    End Class

End Namespace
