Public Class ModelEntity

    Inherits Entity

    'A private copy of a model, because who needs Inherhitance or OO in general.
    'Just... Microsoft... Why make the Model class NotInheritable
    Private _model As Model

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        If ModelManager.ModelExist(Me.AdditionalValue) = True Then
            Me._model = ModelManager.GetModel(Me.AdditionalValue)
        End If
        Me.NeedsUpdate = True

        ApplyEffect()
    End Sub

    Public Sub LoadModel(ByVal m As String)
        Me._model = ModelManager.GetModel(m)
        Me.AdditionalValue = m

        ApplyEffect()
    End Sub


    Public Overrides Sub Update()
        ViewBox = New BoundingBox(
                  Vector3.Transform(New Vector3(-1, -1, -1), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)),
                  Vector3.Transform(New Vector3(1, 1, 1), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)))

        ApplyEffect()
    End Sub

    Public Overrides Sub Render()
        If Visible = True Then
            If Not _model Is Nothing Then
                For Each modelMesh As ModelMesh In Model.Meshes
                    For Each modelMeshPart As ModelMeshPart In modelMesh.MeshParts
                        If modelMeshPart.Effect.GetType() = GetType(BasicEffect)
                            Dim effect = New BasicEffectWithAlphaTest(CType(modelMeshPart.Effect, BasicEffect))
                            modelMeshPart.Effect = effect
                        End If
                    Next
                Next
                Core.GraphicsDevice.SamplerStates(0) = SamplerState.PointWrap
                _model.Draw(Me.World, Screen.Camera.View, Screen.Camera.Projection)
                Core.GraphicsDevice.SamplerStates(0) = Core.Sampler
            End If

            If drawViewBox = True Then
                BoundingBoxRenderer.Render(ViewBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, Microsoft.Xna.Framework.Color.Red)
            End If
        End If
    End Sub

End Class