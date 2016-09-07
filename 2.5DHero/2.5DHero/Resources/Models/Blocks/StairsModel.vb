Public Class StairsModel

    Inherits BaseModel

    Public Sub New()
        vertexData.Clear()

        'Lower stair, front:
        'left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)))
        'right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)))

        'Lower stair, top:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)))

        'Upper stair, front:
        'left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(0.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)))
        'right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 0, 1), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(1.0, 1.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(0, 0, 1), New Vector2(0.0, 1.0)))

        'Upper stair, top:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(0.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 1, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(0, 1, 0), New Vector2(0.0, 0.5)))

        'Back:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(1.0, 1.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(0, 0, -1), New Vector2(0.0, 1.0)))

        'Left side, lower:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), New Vector3(-1, 0, 0), New Vector2(1.0, 1.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 1.0)))

        'Left side, upper:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, 0), New Vector3(-1, 0, 0), New Vector2(0.5, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0, -0.5), New Vector3(-1, 0, 0), New Vector2(0.0, 0.5)))

        'Right side, lower:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 1.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), New Vector3(1, 0, 0), New Vector2(0.0, 1.0)))

        'Right side, upper:
        'Left vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.5)))
        'Right vertex:
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, -0.5), New Vector3(1, 0, 0), New Vector2(1.0, 0.5)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0, 0), New Vector3(1, 0, 0), New Vector2(0.5, 0.5)))

        Me.ID = 16

        Setup()
    End Sub

End Class