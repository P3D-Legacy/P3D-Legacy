// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P3D;

/// <summary>
///     Built-in effect that supports optional texturing, vertex coloring, fog, and lighting.
/// </summary>
public class BasicEffectWithAlphaTest : Effect, IEffectMatrices, IEffectLights, IEffectFog
{
    /// <inheritdoc />
    public DirectionalLight DirectionalLight0 { get; private set; }

    /// <inheritdoc />
    public DirectionalLight DirectionalLight1 { get; private set; }

    /// <inheritdoc />
    public DirectionalLight DirectionalLight2 { get; private set; }

    /// <summary>
    ///     Gets or sets the world matrix.
    /// </summary>
    public Matrix World
    {
        get => world;

        set
        {
            world = value;
            dirtyFlags |= EffectDirtyFlags.World | EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
        }
    }

    /// <summary>
    ///     Gets or sets the view matrix.
    /// </summary>
    public Matrix View
    {
        get => view;

        set
        {
            view = value;
            dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.EyePosition | EffectDirtyFlags.Fog;
        }
    }

    /// <summary>
    ///     Gets or sets the projection matrix.
    /// </summary>
    public Matrix Projection
    {
        get => projection;

        set
        {
            projection = value;
            dirtyFlags |= EffectDirtyFlags.WorldViewProj;
        }
    }

    /// <summary>
    ///     Gets or sets the material diffuse color (range 0 to 1).
    /// </summary>
    public Vector3 DiffuseColor
    {
        get => diffuseColor;

        set
        {
            diffuseColor = value;
            dirtyFlags |= EffectDirtyFlags.MaterialColor;
        }
    }

    /// <summary>
    ///     Gets or sets the material emissive color (range 0 to 1).
    /// </summary>
    public Vector3 EmissiveColor
    {
        get => emissiveColor;

        set
        {
            emissiveColor = value;
            dirtyFlags |= EffectDirtyFlags.MaterialColor;
        }
    }

    /// <summary>
    ///     Gets or sets the material specular color (range 0 to 1).
    /// </summary>
    public Vector3 SpecularColor
    {
        get => specularColorParam.GetValueVector3();
        set => specularColorParam.SetValue(value);
    }

    /// <summary>
    ///     Gets or sets the material specular power.
    /// </summary>
    public float SpecularPower
    {
        get => specularPowerParam.GetValueSingle();
        set => specularPowerParam.SetValue(value);
    }

    /// <summary>
    ///     Gets or sets the material alpha.
    /// </summary>
    public float Alpha
    {
        get => alpha;

        set
        {
            alpha = value;
            dirtyFlags |= EffectDirtyFlags.MaterialColor;
        }
    }

    /// <inheritdoc />
    public bool LightingEnabled
    {
        get => lightingEnabled;

        set
        {
            if (lightingEnabled != value)
            {
                lightingEnabled = value;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex | EffectDirtyFlags.MaterialColor;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the per-pixel lighting prefer flag.
    /// </summary>
    public bool PreferPerPixelLighting
    {
        get => preferPerPixelLighting;

        set
        {
            if (preferPerPixelLighting != value)
            {
                preferPerPixelLighting = value;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }
        }
    }

    /// <inheritdoc />
    public Vector3 AmbientLightColor
    {
        get => ambientLightColor;

        set
        {
            ambientLightColor = value;
            dirtyFlags |= EffectDirtyFlags.MaterialColor;
        }
    }

    /// <inheritdoc />
    public bool FogEnabled
    {
        get => fogEnabled;

        set
        {
            if (fogEnabled != value)
            {
                fogEnabled = value;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex | EffectDirtyFlags.FogEnable;
            }
        }
    }

    /// <inheritdoc />
    public float FogStart
    {
        get => fogStart;

        set
        {
            fogStart = value;
            dirtyFlags |= EffectDirtyFlags.Fog;
        }
    }

    /// <inheritdoc />
    public float FogEnd
    {
        get => fogEnd;

        set
        {
            fogEnd = value;
            dirtyFlags |= EffectDirtyFlags.Fog;
        }
    }

    /// <inheritdoc />
    public Vector3 FogColor
    {
        get => fogColorParam.GetValueVector3();
        set => fogColorParam.SetValue(value);
    }

    /// <summary>
    ///     Gets or sets whether texturing is enabled.
    /// </summary>
    public bool TextureEnabled
    {
        get => textureEnabled;

        set
        {
            if (textureEnabled != value)
            {
                textureEnabled = value;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the current texture.
    /// </summary>
    public Texture2D Texture
    {
        get => textureParam.GetValueTexture2D();
        set => textureParam.SetValue(value);
    }

    /// <summary>
    ///     Gets or sets whether vertex color is enabled.
    /// </summary>
    public bool VertexColorEnabled
    {
        get => vertexColorEnabled;

        set
        {
            if (vertexColorEnabled != value)
            {
                vertexColorEnabled = value;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the alpha cutoff.
    /// </summary>
    public float AlphaCutoff
    {
        get => alphaTestParam.GetValueSingle();
        set => alphaTestParam.SetValue(value);
    }

    private EffectParameter textureParam;
    private EffectParameter diffuseColorParam;
    private EffectParameter emissiveColorParam;
    private EffectParameter specularColorParam;
    private EffectParameter specularPowerParam;
    private EffectParameter eyePositionParam;
    private EffectParameter fogColorParam;
    private EffectParameter fogVectorParam;
    private EffectParameter worldParam;
    private EffectParameter worldInverseTransposeParam;
    private EffectParameter worldViewProjParam;
    private EffectParameter alphaTestParam;

    private bool lightingEnabled;
    private bool preferPerPixelLighting;
    private bool oneLight;
    private bool fogEnabled;
    private bool textureEnabled;
    private bool vertexColorEnabled;

    private Matrix world = Matrix.Identity;
    private Matrix view = Matrix.Identity;
    private Matrix projection = Matrix.Identity;

    private Matrix worldView;

    private Vector3 diffuseColor = Vector3.One;
    private Vector3 emissiveColor = Vector3.Zero;
    private Vector3 ambientLightColor = Vector3.Zero;

    private float alpha = 1;

    private float fogStart;
    private float fogEnd = 1;

    private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;

    /// <summary>
    ///     Creates a new BasicEffect with default parameter settings.
    /// </summary>
    public BasicEffectWithAlphaTest(GraphicsDevice device) : base(device, File.ReadAllBytes(Path.Join(Path.GetDirectoryName(AppContext.BaseDirectory), "Content", "Effects", "BasicEffectWithAlphaTest.mgfx")))
    {
        CacheEffectParameters(null);

        DirectionalLight0.Enabled = true;
        SpecularColor = Vector3.One;
        SpecularPower = 16;
        AlphaCutoff = 0.01f;
    }

    /// <summary>
    ///     Creates a new BasicEffect by cloning parameter settings from an existing instance.
    /// </summary>
    protected BasicEffectWithAlphaTest(BasicEffectWithAlphaTest cloneSource) : base(cloneSource)
    {
        CacheEffectParameters(cloneSource);

        lightingEnabled = cloneSource.lightingEnabled;
        preferPerPixelLighting = cloneSource.preferPerPixelLighting;
        fogEnabled = cloneSource.fogEnabled;
        textureEnabled = cloneSource.textureEnabled;
        vertexColorEnabled = cloneSource.vertexColorEnabled;

        world = cloneSource.world;
        view = cloneSource.view;
        projection = cloneSource.projection;

        diffuseColor = cloneSource.diffuseColor;
        emissiveColor = cloneSource.emissiveColor;
        ambientLightColor = cloneSource.ambientLightColor;

        alpha = cloneSource.alpha;

        fogStart = cloneSource.fogStart;
        fogEnd = cloneSource.fogEnd;
    }

    /// <summary>
    ///     Creates a clone of the current BasicEffect instance.
    /// </summary>
    public override Effect Clone()
    {
        return new BasicEffectWithAlphaTest(this);
    }

    /// <inheritdoc />
    public void EnableDefaultLighting()
    {
        LightingEnabled = true;

        AmbientLightColor = EffectHelpers.EnableDefaultLighting(DirectionalLight0, DirectionalLight1, DirectionalLight2);
    }

    /// <summary>
    ///     Lazily computes derived parameter values immediately before applying the effect.
    /// </summary>
    protected override void OnApply()
    {
        // Recompute the world+view+projection matrix or fog vector?
        dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(dirtyFlags, ref world, ref view, ref projection, ref worldView, fogEnabled, fogStart, fogEnd, worldViewProjParam, fogVectorParam);

        // Recompute the diffuse/emissive/alpha material color parameters?
        if ((dirtyFlags & EffectDirtyFlags.MaterialColor) != 0)
        {
            EffectHelpers.SetMaterialColor(lightingEnabled, alpha, ref diffuseColor, ref emissiveColor, ref ambientLightColor, diffuseColorParam, emissiveColorParam);

            dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
        }

        if (lightingEnabled)
        {
            // Recompute the world inverse transpose and eye position?
            dirtyFlags = EffectHelpers.SetLightingMatrices(dirtyFlags, ref world, ref view, worldParam, worldInverseTransposeParam, eyePositionParam);

            // Check if we can use the only-bother-with-the-first-light shader optimization.
            var newOneLight = !DirectionalLight1.Enabled && !DirectionalLight2.Enabled;

            if (oneLight != newOneLight)
            {
                oneLight = newOneLight;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }
        }

        // Recompute the shader index?
        if ((dirtyFlags & EffectDirtyFlags.ShaderIndex) != 0)
        {
            var shaderIndex = 0;

            if (!fogEnabled)
            {
                shaderIndex += 1;
            }

            if (vertexColorEnabled)
            {
                shaderIndex += 2;
            }

            if (textureEnabled)
            {
                shaderIndex += 4;
            }

            if (lightingEnabled)
            {
                if (preferPerPixelLighting)
                {
                    shaderIndex += 24;
                }
                else if (oneLight)
                {
                    shaderIndex += 16;
                }
                else
                {
                    shaderIndex += 8;
                }
            }

            dirtyFlags &= ~EffectDirtyFlags.ShaderIndex;

            CurrentTechnique = Techniques[shaderIndex];
        }
    }

    /// <summary>
    ///     Looks up shortcut references to our effect parameters.
    /// </summary>
    private void CacheEffectParameters(BasicEffectWithAlphaTest? cloneSource)
    {
        textureParam = Parameters["Texture"];
        diffuseColorParam = Parameters["DiffuseColor"];
        emissiveColorParam = Parameters["EmissiveColor"];
        specularColorParam = Parameters["SpecularColor"];
        specularPowerParam = Parameters["SpecularPower"];
        eyePositionParam = Parameters["EyePosition"];
        fogColorParam = Parameters["FogColor"];
        fogVectorParam = Parameters["FogVector"];
        worldParam = Parameters["World"];
        worldInverseTransposeParam = Parameters["WorldInverseTranspose"];
        worldViewProjParam = Parameters["WorldViewProj"];
        alphaTestParam = Parameters["AlphaTest"];

        DirectionalLight0 = new DirectionalLight(Parameters["DirLight0Direction"],
            Parameters["DirLight0DiffuseColor"],
            Parameters["DirLight0SpecularColor"],
            cloneSource?.DirectionalLight0);

        DirectionalLight1 = new DirectionalLight(Parameters["DirLight1Direction"],
            Parameters["DirLight1DiffuseColor"],
            Parameters["DirLight1SpecularColor"],
            cloneSource?.DirectionalLight1);

        DirectionalLight2 = new DirectionalLight(Parameters["DirLight2Direction"],
            Parameters["DirLight2DiffuseColor"],
            Parameters["DirLight2SpecularColor"],
            cloneSource?.DirectionalLight2);
    }
}