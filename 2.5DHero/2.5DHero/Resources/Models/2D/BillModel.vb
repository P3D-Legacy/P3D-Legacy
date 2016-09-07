Public Class BillModel

    Inherits BaseModel

    Public Sub New()
        vertexData.Clear()

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), Vector3.Backward, New Vector2(0, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)))

        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0), Vector3.Backward, New Vector2(1, 1)))
        vertexData.Add(New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1)))

        Me.ID = 3

        Setup()
    End Sub

End Class