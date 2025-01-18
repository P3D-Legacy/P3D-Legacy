Imports System
Imports System.IO
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public NotInheritable Class BasicEffectWithAlphaTest
    Inherits Effect
    Implements IEffectMatrices, IEffectLights, IEffectFog

    Public Property DirectionalLight0 As DirectionalLight Implements IEffectLights.DirectionalLight0
    Public Property DirectionalLight1 As DirectionalLight Implements IEffectLights.DirectionalLight1
    Public Property DirectionalLight2 As DirectionalLight Implements IEffectLights.DirectionalLight2

    Public Property World As Matrix Implements IEffectMatrices.World
        Get
            Return _world
        End Get
        Set(ByVal value As Matrix)
            _world = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.World Or EffectDirtyFlags.WorldViewProj Or EffectDirtyFlags.Fog
        End Set
    End Property

    Public Property View As Matrix Implements IEffectMatrices.View
        Get
            Return _view
        End Get
        Set(ByVal value As Matrix)
            _view = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.WorldViewProj Or EffectDirtyFlags.EyePosition Or EffectDirtyFlags.Fog
        End Set
    End Property

    Public Property Projection As Matrix Implements IEffectMatrices.Projection
        Get
            Return _projection
        End Get
        Set(ByVal value As Matrix)
            _projection = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.WorldViewProj
        End Set
    End Property

    Public Property DiffuseColor As Vector3
        Get
            Return _diffuseColor
        End Get
        Set(ByVal value As Vector3)
            _diffuseColor = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.MaterialColor
        End Set
    End Property

    Public Property EmissiveColor As Vector3
        Get
            Return _emissiveColor
        End Get
        Set(ByVal value As Vector3)
            _emissiveColor = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.MaterialColor
        End Set
    End Property

    Public Property SpecularColor As Vector3
        Get
            Return _specularColorParam.GetValueVector3()
        End Get
        Set(ByVal value As Vector3)
            _specularColorParam.SetValue(value)
        End Set
    End Property

    Public Property SpecularPower As Single
        Get
            Return _specularPowerParam.GetValueSingle()
        End Get
        Set(ByVal value As Single)
            _specularPowerParam.SetValue(value)
        End Set
    End Property

    Public Property Alpha As Single
        Get
            Return _alpha
        End Get
        Set(ByVal value As Single)
            _alpha = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.MaterialColor
        End Set
    End Property

    Public Property LightingEnabled As Boolean Implements IEffectLights.LightingEnabled
        Get
            Return _lightingEnabled
        End Get
        Set(ByVal value As Boolean)
            If _lightingEnabled <> value Then
                _lightingEnabled = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex Or EffectDirtyFlags.MaterialColor
            End If
        End Set
    End Property

    Public Property PreferPerPixelLighting As Boolean
        Get
            Return _preferPerPixelLighting
        End Get
        Set(ByVal value As Boolean)
            If _preferPerPixelLighting <> value Then
                _preferPerPixelLighting = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex
            End If
        End Set
    End Property

    Public Property AmbientLightColor As Vector3 Implements IEffectLights.AmbientLightColor
        Get
            Return _ambientLightColor
        End Get
        Set(ByVal value As Vector3)
            _ambientLightColor = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.MaterialColor
        End Set
    End Property

    Public Property FogEnabled As Boolean Implements IEffectFog.FogEnabled
        Get
            Return _fogEnabled
        End Get
        Set(ByVal value As Boolean)
            If _fogEnabled <> value Then
                _fogEnabled = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex Or EffectDirtyFlags.FogEnable
            End If
        End Set
    End Property

    Public Property FogStart As Single Implements IEffectFog.FogStart
        Get
            Return _fogStart
        End Get
        Set(ByVal value As Single)
            _fogStart = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.Fog
        End Set
    End Property

    Public Property FogEnd As Single Implements IEffectFog.FogEnd
        Get
            Return _fogEnd
        End Get
        Set(ByVal value As Single)
            _fogEnd = value
            _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.Fog
        End Set
    End Property

    Public Property FogColor As Vector3 Implements IEffectFog.FogColor
        Get
            Return _fogColorParam.GetValueVector3()
        End Get
        Set(ByVal value As Vector3)
            _fogColorParam.SetValue(value)
        End Set
    End Property

    Public Property TextureEnabled As Boolean
        Get
            Return _textureEnabled
        End Get
        Set(ByVal value As Boolean)
            If _textureEnabled <> value Then
                _textureEnabled = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex
            End If
        End Set
    End Property

    Public Property Texture As Texture2D
        Get
            Return _textureParam.GetValueTexture2D()
        End Get
        Set(ByVal value As Texture2D)
            _textureParam.SetValue(value)
        End Set
    End Property

    Public Property VertexColorEnabled As Boolean
        Get
            Return _vertexColorEnabled
        End Get
        Set(ByVal value As Boolean)
            If _vertexColorEnabled <> value Then
                _vertexColorEnabled = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex
            End If
        End Set
    End Property

    Public Property AlphaCutoff As Single
        Get
            Return _alphaTestParam.GetValueSingle()
        End Get
        Set(ByVal value As Single)
            _alphaTestParam.SetValue(value)
        End Set
    End Property

    Public Property EnableHardwareInstancing As Boolean
        Get
            Return _hwEnabled
        End Get
        Set(ByVal value As Boolean)
            If _hwEnabled <> value Then
                _hwEnabled = value
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex
            End If
        End Set
    End Property

    Private _textureParam As EffectParameter
    Private _diffuseColorParam As EffectParameter
    Private _emissiveColorParam As EffectParameter
    Private _specularColorParam As EffectParameter
    Private _specularPowerParam As EffectParameter
    Private _eyePositionParam As EffectParameter
    Private _fogColorParam As EffectParameter
    Private _fogVectorParam As EffectParameter
    Private _worldParam As EffectParameter
    Private _worldInverseTransposeParam As EffectParameter
    Private _worldViewProjParam As EffectParameter
    Private _alphaTestParam As EffectParameter
    Private _lightingEnabled As Boolean
    Private _preferPerPixelLighting As Boolean
    Private _oneLight As Boolean
    Private _fogEnabled As Boolean
    Private _textureEnabled As Boolean
    Private _vertexColorEnabled As Boolean
    Private _world As Matrix = Matrix.Identity
    Private _view As Matrix = Matrix.Identity
    Private _projection As Matrix = Matrix.Identity
    Private _worldView As Matrix
    Private _diffuseColor As Vector3 = Vector3.One
    Private _emissiveColor As Vector3 = Vector3.Zero
    Private _ambientLightColor As Vector3 = Vector3.Zero
    Private _alpha As Single = 1
    Private _fogStart As Single
    Private _fogEnd As Single = 1
    Private _hwEnabled As Boolean
    Private _dirtyFlags As EffectDirtyFlags = EffectDirtyFlags.All

    Public Sub New(ByVal device As GraphicsDevice)
        MyBase.New(device, File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "Content", "Effects", "BasicEffectWithAlphaTest.mgfxdx")))
        CacheEffectParameters()
        DirectionalLight0.Enabled = True
        SpecularColor = Vector3.One
        SpecularPower = 16
        AlphaCutoff = 0
    End Sub

    Public Sub New(ByVal device As GraphicsDevice, ByVal data As Byte())
        MyBase.New(device, data)
        CacheEffectParameters()
        DirectionalLight0.Enabled = True
        SpecularColor = Vector3.One
        SpecularPower = 16
        AlphaCutoff = 0
    End Sub

    Public Sub New(ByVal cloneSource As BasicEffectWithAlphaTest)
        MyBase.New(cloneSource)
        CacheEffectParameters(cloneSource)
        _lightingEnabled = cloneSource.lightingEnabled
        _preferPerPixelLighting = cloneSource.preferPerPixelLighting
        _fogEnabled = cloneSource.fogEnabled
        _textureEnabled = cloneSource.textureEnabled
        _vertexColorEnabled = cloneSource.vertexColorEnabled
        _world = cloneSource.world
        _view = cloneSource.view
        _projection = cloneSource.projection
        _diffuseColor = cloneSource.diffuseColor
        _emissiveColor = cloneSource.emissiveColor
        _ambientLightColor = cloneSource.ambientLightColor
        _alpha = cloneSource.alpha
        _fogStart = cloneSource.fogStart
        _fogEnd = cloneSource.fogEnd
    End Sub

    Public Sub New(ByVal cloneSource As BasicEffect)
        MyBase.New(cloneSource.GraphicsDevice, File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "Content", "Effects", "BasicEffectWithAlphaTest.mgfxdx")))
        CacheEffectParameters(cloneSource)
        With DirectionalLight0
            .Direction = cloneSource.DirectionalLight0.Direction
            .Enabled = cloneSource.DirectionalLight0.Enabled
            .DiffuseColor = cloneSource.DirectionalLight0.DiffuseColor
            .SpecularColor = cloneSource.DirectionalLight0.SpecularColor
        End With
        With DirectionalLight1
            .Direction = cloneSource.DirectionalLight1.Direction
            .Enabled = cloneSource.DirectionalLight1.Enabled
            .DiffuseColor = cloneSource.DirectionalLight1.DiffuseColor
            .SpecularColor = cloneSource.DirectionalLight1.SpecularColor
        End With
        With DirectionalLight2
            .Direction = cloneSource.DirectionalLight2.Direction
            .Enabled = cloneSource.DirectionalLight2.Enabled
            .DiffuseColor = cloneSource.DirectionalLight2.DiffuseColor
            .SpecularColor = cloneSource.DirectionalLight2.SpecularColor
        End With
        World = cloneSource.World
        View = cloneSource.View
        Projection = cloneSource.Projection
        DiffuseColor = cloneSource.DiffuseColor
        EmissiveColor = cloneSource.EmissiveColor
        SpecularColor = cloneSource.SpecularColor
        SpecularPower = cloneSource.SpecularPower
        Alpha = cloneSource.Alpha
        LightingEnabled = cloneSource.LightingEnabled
        PreferPerPixelLighting = cloneSource.PreferPerPixelLighting
        AmbientLightColor = cloneSource.AmbientLightColor
        FogEnabled = cloneSource.FogEnabled
        FogStart = cloneSource.FogStart
        FogEnd = cloneSource.FogEnd
        FogColor = cloneSource.FogColor
        TextureEnabled = cloneSource.TextureEnabled
        Texture = cloneSource.Texture
        VertexColorEnabled = cloneSource.VertexColorEnabled
    End Sub

    Public Widening Shared Operator CType(ByVal effect As BasicEffectWithAlphaTest) As BasicEffect
        Dim newEffect As New BasicEffect(effect.GraphicsDevice)
        With newEffect
            With .DirectionalLight0
                .Enabled = effect.DirectionalLight0.Enabled
                .Direction = effect.DirectionalLight0.Direction
                .DiffuseColor = effect.DirectionalLight0.DiffuseColor
                .SpecularColor = effect.DirectionalLight0.SpecularColor
            End With
            With .DirectionalLight1
                .Enabled = effect.DirectionalLight1.Enabled
                .Direction = effect.DirectionalLight1.Direction
                .DiffuseColor = effect.DirectionalLight1.DiffuseColor
                .SpecularColor = effect.DirectionalLight1.SpecularColor
            End With
            With .DirectionalLight2
                .Enabled = effect.DirectionalLight2.Enabled
                .Direction = effect.DirectionalLight2.Direction
                .DiffuseColor = effect.DirectionalLight2.DiffuseColor
                .SpecularColor = effect.DirectionalLight2.SpecularColor
            End With
            .World = effect.World
            .View = effect.View
            .Projection = effect.Projection
            .DiffuseColor = effect.DiffuseColor
            .EmissiveColor = effect.EmissiveColor
            .SpecularColor = effect.SpecularColor
            .SpecularPower = effect.SpecularPower
            .Alpha = effect.Alpha
            .LightingEnabled = effect.LightingEnabled
            .PreferPerPixelLighting = effect.PreferPerPixelLighting
            .AmbientLightColor = effect.AmbientLightColor
            .FogStart = effect.FogStart
            .FogEnd = effect.FogEnd
            .FogColor = effect.FogColor
            .TextureEnabled = effect.TextureEnabled
            .Texture = effect.Texture
            .VertexColorEnabled = effect.VertexColorEnabled
        End With
        Return newEffect
    End Operator

    Public Overrides Function Clone() As Effect
        Return New BasicEffectWithAlphaTest(Me)
    End Function

    Public Sub EnableDefaultLighting() Implements IEffectLights.EnableDefaultLighting
        LightingEnabled = True
        AmbientLightColor = EffectHelpers.EnableDefaultLighting(DirectionalLight0, DirectionalLight1, DirectionalLight2)
    End Sub

    Protected Overrides Sub OnApply()
        _dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(_dirtyFlags, _world, _view, _projection, _worldView, _fogEnabled, _fogStart, _fogEnd, _worldViewProjParam, _fogVectorParam)

        If (_dirtyFlags And EffectDirtyFlags.MaterialColor) <> 0 Then
            EffectHelpers.SetMaterialColor(_lightingEnabled, _alpha, _diffuseColor, _emissiveColor, _ambientLightColor, _diffuseColorParam, _emissiveColorParam)
            _dirtyFlags = _dirtyFlags And Not EffectDirtyFlags.MaterialColor
        End If

        If _lightingEnabled Then
            _dirtyFlags = EffectHelpers.SetLightingMatrices(_dirtyFlags, _world, _view, _worldParam, _worldInverseTransposeParam, _eyePositionParam)
            Dim newOneLight = Not DirectionalLight1.Enabled AndAlso Not DirectionalLight2.Enabled

            If _oneLight <> newOneLight Then
                _oneLight = newOneLight
                _dirtyFlags = _dirtyFlags Or EffectDirtyFlags.ShaderIndex
            End If
        End If

        If (_dirtyFlags And EffectDirtyFlags.ShaderIndex) <> 0 Then
            Dim shaderIndex = 0

            If Not _fogEnabled Then
                shaderIndex += 1
            End If

            If _vertexColorEnabled Then
                shaderIndex += 2
            End If

            If _textureEnabled Then
                shaderIndex += 4
            End If

            If _lightingEnabled Then
                If _preferPerPixelLighting Then
                    shaderIndex += 24
                ElseIf _oneLight Then
                    shaderIndex += 16
                Else
                    shaderIndex += 8
                End If
            End If

            If _hwEnabled Then
                shaderIndex += 32
            End If

            _dirtyFlags = _dirtyFlags And Not EffectDirtyFlags.ShaderIndex
            CurrentTechnique = Techniques(shaderIndex)
        End If
    End Sub

    Private Sub CacheEffectParameters()
        _textureParam = Parameters("Texture")
        _diffuseColorParam = Parameters("DiffuseColor")
        _emissiveColorParam = Parameters("EmissiveColor")
        _specularColorParam = Parameters("SpecularColor")
        _specularPowerParam = Parameters("SpecularPower")
        _eyePositionParam = Parameters("EyePosition")
        _fogColorParam = Parameters("FogColor")
        _fogVectorParam = Parameters("FogVector")
        _worldParam = Parameters("World")
        _worldInverseTransposeParam = Parameters("WorldInverseTranspose")
        _worldViewProjParam = Parameters("WorldViewProj")
        _alphaTestParam = Parameters("AlphaTest")
        DirectionalLight0 = New DirectionalLight(Parameters("DirLight0Direction"), Parameters("DirLight0DiffuseColor"), Parameters("DirLight0SpecularColor"), Nothing)
        DirectionalLight1 = New DirectionalLight(Parameters("DirLight1Direction"), Parameters("DirLight1DiffuseColor"), Parameters("DirLight1SpecularColor"), Nothing)
        DirectionalLight2 = New DirectionalLight(Parameters("DirLight2Direction"), Parameters("DirLight2DiffuseColor"), Parameters("DirLight2SpecularColor"), Nothing)
    End Sub

    Private Sub CacheEffectParameters(ByVal cloneSource As BasicEffectWithAlphaTest)
        _textureParam = Parameters("Texture")
        _diffuseColorParam = Parameters("DiffuseColor")
        _emissiveColorParam = Parameters("EmissiveColor")
        _specularColorParam = Parameters("SpecularColor")
        _specularPowerParam = Parameters("SpecularPower")
        _eyePositionParam = Parameters("EyePosition")
        _fogColorParam = Parameters("FogColor")
        _fogVectorParam = Parameters("FogVector")
        _worldParam = Parameters("World")
        _worldInverseTransposeParam = Parameters("WorldInverseTranspose")
        _worldViewProjParam = Parameters("WorldViewProj")
        _alphaTestParam = Parameters("AlphaTest")
        DirectionalLight0 = New DirectionalLight(Parameters("DirLight0Direction"), Parameters("DirLight0DiffuseColor"), Parameters("DirLight0SpecularColor"), cloneSource?.DirectionalLight0)
        DirectionalLight1 = New DirectionalLight(Parameters("DirLight1Direction"), Parameters("DirLight1DiffuseColor"), Parameters("DirLight1SpecularColor"), cloneSource?.DirectionalLight1)
        DirectionalLight2 = New DirectionalLight(Parameters("DirLight2Direction"), Parameters("DirLight2DiffuseColor"), Parameters("DirLight2SpecularColor"), cloneSource?.DirectionalLight2)
    End Sub

    Private Sub CacheEffectParameters(ByVal cloneSource As BasicEffect)
        _textureParam = Parameters("Texture")
        _diffuseColorParam = Parameters("DiffuseColor")
        _emissiveColorParam = Parameters("EmissiveColor")
        _specularColorParam = Parameters("SpecularColor")
        _specularPowerParam = Parameters("SpecularPower")
        _eyePositionParam = Parameters("EyePosition")
        _fogColorParam = Parameters("FogColor")
        _fogVectorParam = Parameters("FogVector")
        _worldParam = Parameters("World")
        _worldInverseTransposeParam = Parameters("WorldInverseTranspose")
        _worldViewProjParam = Parameters("WorldViewProj")
        _alphaTestParam = Parameters("AlphaTest")
        DirectionalLight0 = New DirectionalLight(Parameters("DirLight0Direction"), Parameters("DirLight0DiffuseColor"), Parameters("DirLight0SpecularColor"), cloneSource?.DirectionalLight0)
        DirectionalLight1 = New DirectionalLight(Parameters("DirLight1Direction"), Parameters("DirLight1DiffuseColor"), Parameters("DirLight1SpecularColor"), cloneSource?.DirectionalLight1)
        DirectionalLight2 = New DirectionalLight(Parameters("DirLight2Direction"), Parameters("DirLight2DiffuseColor"), Parameters("DirLight2SpecularColor"), cloneSource?.DirectionalLight2)
    End Sub
End Class
