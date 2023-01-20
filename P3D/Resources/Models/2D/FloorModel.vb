Public Class FloorModel

    Inherits BaseModel

    Public Sub New()
        ID = 0

        Dim vertexData = New VertexPositionNormalTexture() {
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Up, New Vector2(0, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Up, New Vector2(1, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1))
        }

        Setup(vertexData)
    End Sub
    
    Protected Overrides Sub CreateModel(entity As Entity)
        Dim vertexData = New List(Of VertexPositionNormalTexture)
        
        Dim textureIndex0 = If(entity.TextureIndex.Length > 0, entity.TextureIndex(0), 0)
        Dim textureIndex1 = If(entity.TextureIndex.Length > 1, entity.TextureIndex(1), 0)
        
        If textureIndex0 <> -1 Then
            vertexData.AddRange({
                New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex0).Left) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex0).Top) / CSng(entity.Texture.Height))),
                New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex0).Left) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex0).Bottom) / CSng(entity.Texture.Height))),
                New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex0).Right) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex0).Bottom) / CSng(entity.Texture.Height)))
            })
        End If
        
        If textureIndex1 <> -1 Then
            vertexData.AddRange({
                New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex1).Right) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex1).Bottom) / CSng(entity.Texture.Height))),
                New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex1).Right) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex1).Top) / CSng(entity.Texture.Height))),
                New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(CSng(entity.TextureRectangles(textureIndex1).Left) / CSng(entity.Texture.Width), CSng(entity.TextureRectangles(textureIndex1).Top) / CSng(entity.Texture.Height)))
            })
        End If
        
        Setup(vertexData.ToArray())
    End Sub

End Class
