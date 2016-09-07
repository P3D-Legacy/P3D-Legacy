Public Class StepModel

    Inherits BaseModel

    Public Sub New()
        vertexData.Clear()

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0))) 'h
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0))) 'c

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0))) 'c
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.25, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.0))) 'd

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0))) 'c
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.25, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 0.0))) 'd
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 1.0))) 'b

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 1.0))) 'h
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0))) 'a

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(0, 0, -1), New Vector2(1, 0))) 'e
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1, 1))) 'a
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0, 1))) 'b

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0, 1))) 'b
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.25, 0.5), New Vector3(0, 0, -1), New Vector2(0, 0))) 'd
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.25, 0.5), New Vector3(0, 0, -1), New Vector2(1, 0))) 'e

        Me.ID = 7

        Setup()
    End Sub

End Class