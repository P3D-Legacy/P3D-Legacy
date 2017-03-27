Namespace Resources

    ''' <summary>
    ''' A basic texture manager to load textures from.
    ''' </summary>
    MustInherit Class TextureManager

        ' An instance of the default texture that gets inserted if a texture cannot be loaded to prevent user code to crash.
        Private Shared _defaultTexture As Texture2D
        Private Const DEFAULT_TEXTURE_PATH As String = "no_texture"

        ''' <summary>
        ''' The local ContentManager for all textures.
        ''' </summary>
        Protected _contentManager As ContentManager
        Protected _textures As Dictionary(Of String, Texture2D)

        ''' <summary>
        ''' Loads a texture from a resource and crops it to a rectangle.
        ''' </summary>
        ''' <param name="resource">The path to the resource (relative to basePath) without extension.</param>
        ''' <param name="basePath">The absolute path to the base directory of the textures.</param>
        ''' <param name="rectangle">The rectangle cropped from the textures. If null, the full texture will be returned.</param>
        Protected Function InternalGetTexture(ByVal resource As String, ByVal basePath As String, ByVal rectangle As Rectangle?) As Texture2D
            ' Build an identifier for the resource file & rectangle:
            Dim identifier As String = resource
            If rectangle.HasValue Then
                identifier &= rectangle.Value.X & "," &
                              rectangle.Value.Y & "," &
                              rectangle.Value.Width & "," &
                              rectangle.Value.Height
            End If

            Dim texture As Texture2D = Nothing

            If Not _textures.Keys.Contains(identifier) Then

                ' We don't have the cropped texture in the storage.
                ' But there might be a chance we have the full texture loaded.
                ' So, search for that now.

                Try

                    If rectangle.HasValue And _textures.Keys.Contains(resource) Then
                        texture = _textures(resource)
                    Else
                        '/////////////////////////////////////// Load order ///////////////////////////////////////
                        '/// Look up if an .xnb file exists.                                                    ///
                        '/// If so, load with the default ContentManager.Load method.                           ///
                        '/// If no .xnb file is found, try looking for a .png file instead and try to load that.///
                        '//////////////////////////////////////////////////////////////////////////////////////////

                        Dim filePathWithoutExtension = IO.Path.Combine({basePath, resource})

                        If IO.File.Exists(filePathWithoutExtension & ".xnb") Then
                            texture = _contentManager.Load(Of Texture2D)(resource)
                        Else
                            If IO.File.Exists(filePathWithoutExtension & ".png") Then
                                Using stream As IO.Stream = IO.File.Open(filePathWithoutExtension & ".png", IO.FileMode.Open)
                                    texture = Texture2D.FromStream(GameCore.State.GameController.GraphicsDevice, stream)
                                End Using
                            End If
                        End If

                        _textures.Add(resource, texture)
                    End If

                    If rectangle.HasValue Then

                        Dim croppedTexture = GetTextureRectangle(texture, rectangle.Value)
                        _textures.Add(identifier, croppedTexture)

                    End If

                Catch ex As Exception

                    'If anything goes wrong during loading, add the default texture for this identifier:
                    _textures.Add(identifier, GetDefaultTexture())

                End Try

            End If

            texture = _textures(identifier)
            Return texture
        End Function

        ''' <summary>
        ''' This method returns a subregion of a texture.
        ''' </summary>
        Protected Function GetTextureRectangle(ByVal texture As Texture2D, ByVal rectangle As Rectangle) As Texture2D
            Dim dataLength As Integer = rectangle.Width * rectangle.Height
            Dim colorData(dataLength - 1) As Color

            texture.GetData(0, rectangle, colorData, 0, dataLength)

            Dim croppedTexture As New Texture2D(GameCore.State.GameController.GraphicsDevice, rectangle.Width, rectangle.Height)
            croppedTexture.SetData(colorData)

            Return croppedTexture
        End Function

        Private Function GetDefaultTexture() As Texture2D
            If _defaultTexture Is Nothing Then
                'Attemp to load the default texture.
                'we use the game's main content manager here, because the default texture instance is shared across all texture managers.
                'If this fails, we crash.

                Dim texturePath As String = IO.Path.Combine({GameCore.FileSystem.PATH_TEXTURES, DEFAULT_TEXTURE_PATH})
                _defaultTexture = GameCore.State.GameController.Content.Load(Of Texture2D)(texturePath)
            End If

            Return _defaultTexture
        End Function

    End Class

End Namespace
