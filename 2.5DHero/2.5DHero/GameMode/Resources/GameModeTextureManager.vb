Namespace GameModes.Resources

    ''' <summary>
    ''' A class that loads and manages textures for a GameMode.
    ''' </summary>
    Class GameModeTextureManager

        Inherits Game.Resources.TextureManager

        Implements IGameModeComponent

        Public Sub Activated(ByVal GameMode As GameMode) Implements IGameModeComponent.Activated
            _contentManager = New ContentManager(GameCore.State.GameController.Services, GameMode.TexturePath)
            _textures = New Dictionary(Of String, Texture2D)()
        End Sub

        Public Sub FreeResources() Implements IGameModeComponent.FreeResources
            For i = 0 To _textures.Count - 1
                _textures.Values(i).Dispose()
            Next

            _contentManager.Dispose()

            _textures.Clear()
        End Sub

        ''' <summary>
        ''' Returns a texture from a resource path.
        ''' </summary>
        ''' <param name="resource">The resource path (relative to the Texture folder of the GameMode.</param>
        Public Function GetTexture(ByVal resource As String) As Texture2D
            Return InternalGetTexture(resource, GameMode.Active.ContentFilePath(resource, GameModeContentFile.TextureFile), Nothing)
        End Function

        ''' <summary>
        ''' Returns part of a texture described by a resource path and a source rectangle.
        ''' </summary>
        ''' <param name="resource">The texture path (relative to the Texture folder in the GameMode.</param>
        ''' <param name="rectangle">The source rectangle of the texture.</param>
        Public Function GetTexture(ByVal resource As String, ByVal rectangle As Rectangle) As Texture2D
            Return InternalGetTexture(resource, GameMode.Active.ContentFilePath(resource, GameModeContentFile.TextureFile), rectangle)
        End Function

        ''' <summary>
        ''' Returns a texture described by a <see cref="DataModel.Json.TextureSourceModel"/>.
        ''' </summary>
        Public Function GetTexture(ByVal resourceModel As DataModel.Json.TextureSourceModel) As Texture2D
            With resourceModel
                Return InternalGetTexture(.Source, GameMode.Active.ContentFilePath(.Source, GameModeContentFile.TextureFile), .Rectangle.GetRectangle())
            End With
        End Function

    End Class

End Namespace
