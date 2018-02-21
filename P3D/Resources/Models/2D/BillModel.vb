Public Class BillModel

    Inherits BaseModel

    Public Sub New()
        Me.ID = 3

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, 0.5, 0), Vector3.Backward, New Vector2(0, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, 0.5, 0), Vector3.Backward, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0), Vector3.Backward, New Vector2(1, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0), Vector3.Backward, New Vector2(0, 1))
        }

        Setup(vertexData)
    End Sub

End Class
