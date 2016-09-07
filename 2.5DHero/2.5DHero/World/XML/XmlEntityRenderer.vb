Namespace XmlLevel

    Public Class XmlEntityRenderer

        Public XmlEntity As XmlEntity

        Private Shared newRasterizerState As RasterizerState
        Private Shared oldRasterizerState As RasterizerState

        Public Shared drawBoudingBox As Boolean = False
        Public Shared drawViewBox As Boolean = False

        Private _baseModel As BaseModel = Nothing
        Private _renderModel As Model = Nothing

        Public Sub New(ByVal XmlEntityReference As XmlEntity)
            Me.XmlEntity = XmlEntityReference

            'Load shared rasterizer states, if not done already:
            If newRasterizerState Is Nothing Then
                newRasterizerState = New RasterizerState()
                oldRasterizerState = New RasterizerState()

                newRasterizerState.CullMode = CullMode.None
                oldRasterizerState.CullMode = CullMode.CullCounterClockwiseFace
            End If
        End Sub

        Public Sub Render()
            Me.LoadModel(Me.XmlEntity.RenderType)
            Select Case Me.XmlEntity.RenderType.ToLower()
                Case "basemodel"
                    If Not Me._baseModel Is Nothing Then
                        If Me.XmlEntity.GetPropertyValue(Of Boolean)("culling") = True Then
                            Core.GraphicsDevice.RasterizerState = newRasterizerState
                        End If

                        Me.RenderBaseModel()

                        If Me.XmlEntity.GetPropertyValue(Of Boolean)("culling") = True Then
                            Core.GraphicsDevice.RasterizerState = oldRasterizerState
                        End If
                    End If
                Case "model"
                    If Not Me._renderModel Is Nothing Then
                        Me.ApplyRenderModelEffect()

                        Me.RenderRenderModel()
                    End If
            End Select

            Me.RenderBoundingBoxes()
        End Sub

        Private Sub RenderBoundingBoxes()
            If drawBoudingBox = True Then
                BoundingBoxRenderer.Render(XmlEntity.BoundingBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, Color.White)
            End If
            If drawViewBox = True Then
                BoundingBoxRenderer.Render(XmlEntity.ViewBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, Color.LightCoral)
            End If
        End Sub

        Private Sub RenderBaseModel()
            Dim effectDiffuseColor As Vector3 = Screen.Effect.DiffuseColor

            Screen.Effect.World = Me.XmlEntity.World
            Screen.Effect.TextureEnabled = True
            Screen.Effect.Alpha = Me.XmlEntity.Opacity

            Screen.Effect.DiffuseColor = effectDiffuseColor * Me.XmlEntity.Shader

            If Screen.Level.IsDark = True Then
                Screen.Effect.DiffuseColor *= New Vector3(0.5, 0.5, 0.5)
            End If

            Core.GraphicsDevice.SetVertexBuffer(Me._baseModel.vertexBuffer)

            If CInt(Me._baseModel.vertexBuffer.VertexCount / 3) > Me.XmlEntity.TextureIndex.Count Then
                Dim newTextureIndex(CInt(Me._baseModel.vertexBuffer.VertexCount / 3)) As Integer
                For i = 0 To CInt(Me._baseModel.vertexBuffer.VertexCount / 3)
                    If Me.XmlEntity.TextureIndex.Count - 1 >= i Then
                        newTextureIndex(i) = Me.XmlEntity.TextureIndex(i)
                    Else
                        newTextureIndex(i) = 0
                    End If
                Next
                Me.XmlEntity.TextureIndex = newTextureIndex
            End If

            Dim isEqual As Boolean = True
            Dim contains As Integer = Me.XmlEntity.TextureIndex(0)
            For index = 1 To Me.XmlEntity.TextureIndex.Length - 1
                If contains <> Me.XmlEntity.TextureIndex(index) Then
                    isEqual = False
                    Exit For
                End If
            Next

            If isEqual = True Then
                If Me.XmlEntity.TextureIndex(0) > -1 Then
                    Screen.Effect.Texture = Me.XmlEntity.Textures(Me.XmlEntity.TextureIndex(0))
                    Screen.Effect.CurrentTechnique.Passes(0).Apply()

                    Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, CInt(Me._baseModel.vertexBuffer.VertexCount / 3))

                    DebugDisplay.DrawnVertices += CInt(Me._baseModel.vertexBuffer.VertexCount / 3)
                End If
            Else
                For i = 0 To Me._baseModel.vertexBuffer.VertexCount - 1 Step 3
                    If Me.XmlEntity.TextureIndex(CInt(i / 3)) > -1 Then
                        Screen.Effect.Texture = Me.XmlEntity.Textures(Me.XmlEntity.TextureIndex(CInt(i / 3)))
                        Screen.Effect.CurrentTechnique.Passes(0).Apply()

                        Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, i, 1)
                        DebugDisplay.DrawnVertices += 1
                    End If
                Next
            End If

            Screen.Effect.DiffuseColor = effectDiffuseColor

            If DebugDisplay.MaxDistance < Me.XmlEntity.CameraDistance Then DebugDisplay.MaxDistance = CInt(Me.XmlEntity.CameraDistance)
        End Sub

        Private Sub RenderRenderModel()
            If Not Me._renderModel Is Nothing Then
                Me._renderModel.Draw(Me.XmlEntity.World, Screen.Camera.View, Screen.Camera.Projection)
            End If
        End Sub

        Private loadedBaseModel As Boolean = False
        Private loadedRenderModel As Boolean = False

        Private Sub LoadModel(ByVal RenderType As String)
            Select Case RenderType.ToLower()
                Case "basemodel"
                    If Me.loadedBaseModel = False Then
                        Me._baseModel = net.Pokemon3D.Game.BaseModel.getModelbyID(Me.XmlEntity.GetPropertyValue(Of Integer)("basemodel"))
                        Me.loadedBaseModel = True
                    End If
                Case "model"
                    If loadedRenderModel = False Then
                        Dim modelPath As String = Me.XmlEntity.GetPropertyValue(Of String)("model")
                        If ModelManager.ModelExist(modelPath) = True Then
                            Me._renderModel = ModelManager.GetModel(modelPath)
                            Me.loadedRenderModel = True
                        End If
                    End If
            End Select
        End Sub

        Dim defaultDiffuseColor As Vector3 = Nothing 'Stores the default diffuse color for the model.
        Dim setDefaultDiffuseColor As Boolean = False 'checks if the default diffuse color has been set already.

        ''' <summary>
        ''' This sub applies all the shading effects done by weather and daytime to the model, if rendered.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ApplyRenderModelEffect() 'Call this function before the model gets rendered.
            For Each mesh As ModelMesh In Me._renderModel.Meshes
                For Each part As ModelMeshPart In mesh.MeshParts
                    If part.Effect.GetType().Name.ToLower() = Screen.Effect.GetType().Name.ToLower() Then
                        Lighting.UpdateLighting(CType(part.Effect, BasicEffect), True)

                        If setDefaultDiffuseColor = False Then
                            setDefaultDiffuseColor = True
                            defaultDiffuseColor = CType(part.Effect, BasicEffect).DiffuseColor
                        End If

                        CType(part.Effect, BasicEffect).DiffuseColor = Screen.Effect.DiffuseColor * defaultDiffuseColor

                        If Screen.Level.IsDark = True Then
                            CType(part.Effect, BasicEffect).DiffuseColor = New Vector3(0.5, 0.5, 0.5) * defaultDiffuseColor
                        End If

                        CType(part.Effect, BasicEffect).FogEnabled = True
                        CType(part.Effect, BasicEffect).FogColor = Screen.Effect.FogColor
                        CType(part.Effect, BasicEffect).FogEnd = Screen.Effect.FogEnd
                        CType(part.Effect, BasicEffect).FogStart = Screen.Effect.FogStart
                    End If
                Next
            Next
        End Sub

        Public ReadOnly Property BaseModel() As BaseModel
            Get
                Return Me._baseModel
            End Get
        End Property

        Public ReadOnly Property RenderModel() As Model
            Get
                Return Me._renderModel
            End Get
        End Property

    End Class

End Namespace