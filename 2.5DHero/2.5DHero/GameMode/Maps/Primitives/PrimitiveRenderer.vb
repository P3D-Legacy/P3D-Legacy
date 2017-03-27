Namespace GameModes.Maps.Primitives

    ''' <summary>
    ''' Renders primitive models attached to entities.
    ''' </summary>
    Class PrimitiveRenderer

        Private Shared _primitiveModels As List(Of PrimitiveModel)

        Private Shared Sub InitializePrimitiveModels()
            'Once, load all models and store them statically:
            _primitiveModels = New List(Of PrimitiveModel)()

            _primitiveModels.Add(New FloorModel()) '0
            _primitiveModels.Add(New BlockModel()) '1
            _primitiveModels.Add(New SlideModel()) '2
            _primitiveModels.Add(New BillModel()) '3
            _primitiveModels.Add(New SignModel()) '4
            _primitiveModels.Add(New CornerModel()) '5
            _primitiveModels.Add(New InsideCornerModel()) '6
            _primitiveModels.Add(New StepModel()) '7
            _primitiveModels.Add(New InsideStepModel()) '8
            _primitiveModels.Add(New CliffModel()) '9
            _primitiveModels.Add(New CliffInsideModel()) '10
            _primitiveModels.Add(New CliffCornerModel()) '11
            _primitiveModels.Add(New CubeModel()) '12
            _primitiveModels.Add(New CrossModel()) '13
            _primitiveModels.Add(New DoubleFloorModel()) '14
            _primitiveModels.Add(New PyramidModel()) '15
            _primitiveModels.Add(New StairsModel()) '16
        End Sub

        ''' <summary>
        ''' Renders an <see cref="Entity"/> with its primitive model visualization.
        ''' </summary>
        Public Shared Sub Render(ByVal entity As Entity)
            If _primitiveModels Is Nothing Then
                InitializePrimitiveModels()
            End If

            If _primitiveModels.Count() >= entity.DataModel.RenderMode.PrimitiveModelId Then
                Dim tempDiffuseColor As Vector3 = Screen.Effect.DiffuseColor

                Screen.Effect.World = entity.World
                Screen.Effect.Alpha = entity.Opacity

                Screen.Effect.DiffuseColor *= entity.DiffuseColor
                ' If the level is dark, then apply a darker shade to the diffuse color:
                If Screen.Level.IsDark Then
                    Screen.Effect.DiffuseColor *= New Vector3(0.5F)
                End If

                Dim model As PrimitiveModel = _primitiveModels(entity.DataModel.RenderMode.PrimitiveModelId)
                GameCore.State.GameController.GraphicsDevice.SetVertexBuffer(model.vertexBuffer)

                Dim triangleCount As Integer = CInt(model.vertexBuffer.VertexCount / 3)

                ' Adjust the entity's texture index if it's too small:
                If triangleCount > entity.DataModel.RenderMode.TextureIndex.Length Then
                    Dim newTextureIndex(triangleCount - 1) As Integer
                    entity.DataModel.RenderMode.TextureIndex.CopyTo(newTextureIndex, 0)
                    For i As Integer = entity.DataModel.RenderMode.TextureIndex.Length To triangleCount - 1
                        newTextureIndex(i) = 0
                    Next
                    entity.DataModel.RenderMode.TextureIndex = newTextureIndex
                End If

                Dim textureIndex = entity.DataModel.RenderMode.TextureIndex

                'When all vertices of the model are using the same texture, just use one draw call:
                If textureIndex.All(Function(x) x = textureIndex(0)) Then
                    If textureIndex(0) > -1 Then
                        ApplyTexture(entity.Textures(textureIndex(0)))

                        Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleCount)
                        DebugDisplay.DrawnVertices += 1
                    End If
                Else
                    'When they are using different textures, try to group the draw calls for consecutive textures:
                    Dim startedTextureIndex As Integer = 0
                    Dim index As Integer = 0
                    For index = 0 To textureIndex.Length - 1
                        If textureIndex(index) <> textureIndex(startedTextureIndex) Then
                            If textureIndex(startedTextureIndex) > -1 Then
                                ApplyTexture(entity.Textures(textureIndex(startedTextureIndex)))
                                GameCore.State.GameController.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, startedTextureIndex * 3, index - startedTextureIndex)
                                DebugDisplay.DrawnVertices += 1
                            End If

                            startedTextureIndex = index
                        End If
                    Next

                    'Render the last batch that wasn't rendered in the for loop above:
                    If startedTextureIndex < textureIndex.Length - 1 AndAlso textureIndex(startedTextureIndex) > -1 Then
                        ApplyTexture(entity.Textures(textureIndex(startedTextureIndex)))
                        GameCore.State.GameController.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, startedTextureIndex * 3, textureIndex.Length - startedTextureIndex)
                        DebugDisplay.DrawnVertices += 1
                    End If

                End If

                ' Set the effect's diffuse color for the next render:
                Screen.Effect.DiffuseColor = tempDiffuseColor
            End If
        End Sub

        ''' <summary>
        ''' Applies a texture to the active effect.
        ''' </summary>
        Private Shared Sub ApplyTexture(ByVal texture As Texture2D)
            Screen.Effect.Texture = texture
            Screen.Effect.CurrentTechnique.Passes(0).Apply()
        End Sub

    End Class

End Namespace
