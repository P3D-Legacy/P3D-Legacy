Namespace GameModes.Maps

    ''' <summary>
    ''' Renders imported models attached to entities.
    ''' </summary>
    Class EntityModelRenderer

        ''' <summary>
        ''' Renders an entity with model representation.
        ''' </summary>
        Public Shared Sub Render(ByVal entity As Entity)
            ApplyEffect(entity)

            entity.Model3D.Draw(entity.World, Screen.Camera.View, Screen.Camera.Projection)
        End Sub

        Private Shared Sub ApplyEffect(ByVal entity As Entity)
            For Each mesh As ModelMesh In entity.Model3D.Meshes
                For Each part As ModelMeshPart In mesh.MeshParts
                    If part.Effect.GetType() = Screen.Effect.GetType() Then
                        With CType(part.Effect, BasicEffect)
                            'set the diffuse color from the entity:
                            .DiffuseColor = entity.DiffuseColor

                            'Copy the important variables from the screen's main effect:
                            .FogEnabled = Screen.Effect.FogEnabled
                            .FogColor = Screen.Effect.FogColor
                            .FogEnd = Screen.Effect.FogEnd
                            .FogStart = Screen.Effect.FogStart

                            .SpecularPower = Screen.Effect.SpecularPower
                            .PreferPerPixelLighting = Screen.Effect.PreferPerPixelLighting

                            .AmbientLightColor = Screen.Effect.AmbientLightColor

                            .DirectionalLight0.DiffuseColor = Screen.Effect.DirectionalLight0.DiffuseColor
                            .DirectionalLight0.Direction = Screen.Effect.DirectionalLight0.Direction
                            .DirectionalLight0.SpecularColor = Screen.Effect.DirectionalLight0.SpecularColor
                            .DirectionalLight0.Enabled = Screen.Effect.DirectionalLight0.Enabled

                            .LightingEnabled = Screen.Effect.LightingEnabled
                        End With
                    End If
                Next
            Next
        End Sub

    End Class

End Namespace
