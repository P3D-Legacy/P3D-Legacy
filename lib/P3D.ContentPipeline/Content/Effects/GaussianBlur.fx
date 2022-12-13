#if OPENGL
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define RADIUS  7
#define KERNEL_SIZE (RADIUS * 2 + 1)

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

sampler colorMapTexture : register(s0);

//-----------------------------------------------------------------------------
// Pixel Shaders.
//-----------------------------------------------------------------------------

float4 PS_MAIN(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    for (int i = 0; i < KERNEL_SIZE; ++i)
        color += tex2D(colorMapTexture, uv + offsets[i]) * weights[i];
    
    return color;
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique GaussianBlur
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_MAIN();
    }
}
