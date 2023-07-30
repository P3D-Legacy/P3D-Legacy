Imports System
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

<Flags>
Public Enum EffectDirtyFlags
    WorldViewProj = 1
    World = 2
    EyePosition = 4
    MaterialColor = 8
    Fog = 16
    FogEnable = 32
    AlphaTest = 64
    ShaderIndex = 128
    All = -1
End Enum

Public Module EffectHelpers
    Public Function EnableDefaultLighting(ByVal light0 As DirectionalLight, ByVal light1 As DirectionalLight, ByVal light2 As DirectionalLight) As Vector3
        light0.Direction = New Vector3(-0.5265408F, -0.5735765F, -0.6275069F)
        light0.DiffuseColor = New Vector3(1, 0.9607844F, 0.8078432F)
        light0.SpecularColor = New Vector3(1, 0.9607844F, 0.8078432F)
        light0.Enabled = True
        
        light1.Direction = New Vector3(0.7198464F, 0.3420201F, 0.6040227F)
        light1.DiffuseColor = New Vector3(0.9647059F, 0.7607844F, 0.4078432F)
        light1.SpecularColor = Vector3.Zero
        light1.Enabled = True
        
        light2.Direction = New Vector3(0.4545195F, -0.7660444F, 0.4545195F)
        light2.DiffuseColor = New Vector3(0.3231373F, 0.3607844F, 0.3937255F)
        light2.SpecularColor = New Vector3(0.3231373F, 0.3607844F, 0.3937255F)
        light2.Enabled = True
        
        Return New Vector3(0.05333332F, 0.09882354F, 0.1819608F)
    End Function

    Public Function SetWorldViewProjAndFog(ByVal dirtyFlags As EffectDirtyFlags, ByRef world As Matrix, ByRef view As Matrix, ByRef projection As Matrix, ByRef worldView As Matrix, ByVal fogEnabled As Boolean, ByVal fogStart As Single, ByVal fogEnd As Single, ByVal worldViewProjParam As EffectParameter, ByVal fogVectorParam As EffectParameter) As EffectDirtyFlags
        If (dirtyFlags And EffectDirtyFlags.WorldViewProj) <> 0 Then
            Dim worldViewProj As Matrix = Nothing
            Matrix.Multiply(world, view, worldView)
            Matrix.Multiply(worldView, projection, worldViewProj)
            worldViewProjParam.SetValue(worldViewProj)
            dirtyFlags = dirtyFlags And Not EffectDirtyFlags.WorldViewProj
        End If

        If fogEnabled Then
            If (dirtyFlags And (EffectDirtyFlags.Fog Or EffectDirtyFlags.FogEnable)) <> 0 Then
                SetFogVector(worldView, fogStart, fogEnd, fogVectorParam)
                dirtyFlags = dirtyFlags And Not (EffectDirtyFlags.Fog Or EffectDirtyFlags.FogEnable)
            End If
        Else
            If (dirtyFlags And EffectDirtyFlags.FogEnable) <> 0 Then
                fogVectorParam.SetValue(Vector4.Zero)
                dirtyFlags = dirtyFlags And Not EffectDirtyFlags.FogEnable
            End If
        End If

        Return dirtyFlags
    End Function

    Public Function SetLightingMatrices(ByVal dirtyFlags As EffectDirtyFlags, ByRef world As Matrix, ByRef view As Matrix, ByVal worldParam As EffectParameter, ByVal worldInverseTransposeParam As EffectParameter, ByVal eyePositionParam As EffectParameter) As EffectDirtyFlags
        If (dirtyFlags And EffectDirtyFlags.World) <> 0 Then
            Dim worldTranspose As Matrix = Nothing, worldInverseTranspose As Matrix = Nothing
            Matrix.Invert(world, worldTranspose)
            Matrix.Transpose(worldTranspose, worldInverseTranspose)
            worldParam.SetValue(world)
            worldInverseTransposeParam.SetValue(worldInverseTranspose)
            dirtyFlags = dirtyFlags And Not EffectDirtyFlags.World
        End If
        
        If (dirtyFlags And EffectDirtyFlags.EyePosition) <> 0 Then
            Dim viewInverse As Matrix = Nothing
            Matrix.Invert(view, viewInverse)
            eyePositionParam.SetValue(viewInverse.Translation)
            dirtyFlags = dirtyFlags And Not EffectDirtyFlags.EyePosition
        End If

        Return dirtyFlags
    End Function

    Public Sub SetMaterialColor(ByVal lightingEnabled As Boolean, ByVal alpha As Single, ByRef diffuseColor As Vector3, ByRef emissiveColor As Vector3, ByRef ambientLightColor As Vector3, ByVal diffuseColorParam As EffectParameter, ByVal emissiveColorParam As EffectParameter)
        If lightingEnabled Then
            Dim diffuse = New Vector4()
            Dim emissive = New Vector3()
            diffuse.X = diffuseColor.X * alpha
            diffuse.Y = diffuseColor.Y * alpha
            diffuse.Z = diffuseColor.Z * alpha
            diffuse.W = alpha
            emissive.X = (emissiveColor.X + ambientLightColor.X * diffuseColor.X) * alpha
            emissive.Y = (emissiveColor.Y + ambientLightColor.Y * diffuseColor.Y) * alpha
            emissive.Z = (emissiveColor.Z + ambientLightColor.Z * diffuseColor.Z) * alpha
            diffuseColorParam.SetValue(diffuse)
            emissiveColorParam.SetValue(emissive)
        Else
            Dim diffuse = New Vector4 With {
                .X = (diffuseColor.X + emissiveColor.X) * alpha,
                .Y = (diffuseColor.Y + emissiveColor.Y) * alpha,
                .Z = (diffuseColor.Z + emissiveColor.Z) * alpha,
                .W = alpha
            }
            diffuseColorParam.SetValue(diffuse)
        End If
    End Sub

    Private Sub SetFogVector(ByRef worldView As Matrix, ByVal fogStart As Single, ByVal fogEnd As Single, ByVal fogVectorParam As EffectParameter)
        If fogStart = fogEnd Then
            fogVectorParam.SetValue(New Vector4(0, 0, 0, 1))
        Else
            Dim scale = 1F / (fogStart - fogEnd)
            Dim fogVector = New Vector4 With {
                .X = worldView.M13 * scale,
                .Y = worldView.M23 * scale,
                .Z = worldView.M33 * scale,
                .W = (worldView.M43 + fogStart) * scale
            }
            fogVectorParam.SetValue(fogVector)
        End If
    End Sub
End Module
