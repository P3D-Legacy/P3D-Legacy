Namespace GameModes.Maps.Primitives

    ''' <summary>
    ''' A model composed of vertices.
    ''' </summary>
    MustInherit Class PrimitiveModel

        Public vertexData As New List(Of VertexPositionNormalTexture)
        Public vertexBuffer As VertexBuffer

        ''' <summary>
        ''' Fills the vertex buffer with the vertex data.
        ''' </summary>
        Protected Sub Setup()
            vertexBuffer = New VertexBuffer(GameCore.State.GameController.GraphicsDevice, GetType(VertexPositionNormalTexture), vertexData.Count, BufferUsage.WriteOnly)
            vertexBuffer.SetData(vertexData.ToArray())

            vertexData.Clear()
        End Sub

    End Class

End Namespace
