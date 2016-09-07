Public Class ModelManager

    Shared ModelList As New Dictionary(Of String, Model)

    Public Shared Function GetModel(ByVal Path As String) As Model
        Dim cContent As ContentManager = ContentPackManager.GetContentManager(Path, ".xnb")

        Dim tKey As String = cContent.RootDirectory & "\" & Path

        If ModelList.Keys.Contains(tKey) = False Then
            Dim m As Model = cContent.Load(Of Model)(Path)

            ModelList.Add(tKey, CreateShallowCopy(m))
        End If

        Return ModelList(tKey)
    End Function

    ''' <summary>
    ''' This method creates a shallow copy of the Model class.
    ''' And because there's no other way to properly do this, we access its Protected method MemberwiseClone (inherited from Object) via reflection.
    ''' </summary>
    ''' <param name="m">The model to copy.</param>
    ''' <returns></returns>
    Private Shared Function CreateShallowCopy(ByVal m As Model) As Model
        Dim memberWiseCloneMethod As Reflection.MethodInfo = m.GetType().GetMethod("MemberwiseClone", Reflection.BindingFlags.NonPublic Or 'I AM SO SORRY!!!
                                                                                              Reflection.BindingFlags.Instance Or
                                                                                              Reflection.BindingFlags.FlattenHierarchy)

        Return CType(memberWiseCloneMethod.Invoke(m, {}), Model) 'invokes the method on the passed in Model instance to create a shallow copy.
    End Function

    Public Shared Function ModelExist(ByVal Path As String) As Boolean
        Dim cContent As ContentManager = ContentPackManager.GetContentManager(Path, ".xnb")

        If Path = "" Then
            Return False
        End If

        If System.IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Path & ".xnb") = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub Clear()
        ModelList.Clear()
    End Sub

    Public Shared Function DrawModelToTexture(ByVal modelName As String, ByVal texSize As Vector2, ByVal modelPosition As Vector3, ByVal cameraPosition As Vector3, ByVal cameraRotation As Vector3, ByVal Scale As Single, ByVal enableLight As Boolean) As Texture2D
        Dim renderTarget As RenderTarget2D = New RenderTarget2D(Core.GraphicsDevice, CInt(texSize.X), CInt(texSize.Y), False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        Core.GraphicsDevice.SetRenderTarget(renderTarget)
        Core.GraphicsDevice.Clear(Color.Transparent)

        Core.GraphicsDevice.BlendState = BlendState.Opaque
        Core.GraphicsDevice.SamplerStates(0) = Core.sampler

        Dim m As Model = ModelManager.GetModel(modelName)

        If enableLight = True Then
            For Each mesh As ModelMesh In m.Meshes
                For Each Effect As BasicEffect In mesh.Effects
                    Lighting.UpdateLighting(Effect, True)

                    Effect.DirectionalLight0.DiffuseColor = New Vector3(0.7)
                    Effect.DirectionalLight1.DiffuseColor = New Vector3(0.7)
                    Effect.DirectionalLight2.DiffuseColor = New Vector3(0.7)

                    Effect.DirectionalLight0.Direction = New Vector3(0, 1, 0)
                    Effect.DirectionalLight1.Direction = New Vector3(1, 0, 0)
                    Effect.DirectionalLight2.Direction = New Vector3(0, 0, 1)

                    Effect.DirectionalLight0.Enabled = True
                    Effect.DirectionalLight1.Enabled = True
                    Effect.DirectionalLight2.Enabled = True
                Next
            Next
        End If

        m.Draw(Matrix.CreateFromYawPitchRoll(cameraRotation.X, cameraRotation.Y, cameraRotation.Z) * Matrix.CreateScale(New Vector3(Scale)) * Matrix.CreateTranslation(modelPosition), Matrix.CreateLookAt(cameraPosition, modelPosition, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0F), Core.GraphicsDevice.Viewport.AspectRatio, 0.1F, 10000.0F))

        Core.GraphicsDevice.SetRenderTarget(Nothing)

        Return renderTarget
    End Function

End Class