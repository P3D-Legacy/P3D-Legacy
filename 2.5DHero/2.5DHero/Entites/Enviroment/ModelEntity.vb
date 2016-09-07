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

    Private Sub ApplyEffect()
        If Not _model Is Nothing Then
            For Each mesh As ModelMesh In Me._model.Meshes
                For Each part As ModelMeshPart In mesh.MeshParts
                    If part.Effect.GetType().Name.ToLower() = Screen.Effect.GetType().Name.ToLower() Then
                        With CType(part.Effect, BasicEffect)
                            Lighting.UpdateLighting(CType(part.Effect, BasicEffect), True)

                            .DiffuseColor = Screen.Effect.DiffuseColor

                            If Not Screen.Level.World Is Nothing Then
                                If Screen.Level.World.EnvironmentType = Game.World.EnvironmentTypes.Outside Then
                                    .DiffuseColor *= SkyDome.GetDaytimeColor(True).ToVector3()
                                End If
                            End If

                            .FogEnabled = True
                            .FogColor = Screen.Effect.FogColor
                            .FogEnd = Screen.Effect.FogEnd
                            .FogStart = Screen.Effect.FogStart
                        End With
                    End If
                Next
            Next
        End If
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
                _model.Draw(Me.World, Screen.Camera.View, Screen.Camera.Projection)
            End If

            If drawViewBox = True Then
                BoundingBoxRenderer.Render(ViewBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, Color.Red)
            End If
        End If
    End Sub

End Class