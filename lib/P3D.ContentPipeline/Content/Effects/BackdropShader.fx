﻿#if OPENGL
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;

texture color;
texture normals;
texture offset;
float4 DiffuseColor;
float2 TexStretch;

sampler color_sampler = sampler_state
{
    texture = <color>;
};
sampler normal_sampler = sampler_state
{
    texture = <normals>;
};
sampler offset_sampler = sampler_state
{
    texture = <offset>;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD1;
    float4 Normal : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
    output.Normal = mul(input.Normal, World);
    output.TexCoord = input.TexCoord;

    return output;
}

float4 TextureShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 resultColor = tex2D(color_sampler, float2(input.TexCoord.x * TexStretch.x, input.TexCoord.y * TexStretch.y)) * DiffuseColor;

    return resultColor;
}

technique Texture
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL TextureShaderFunction();
    }
}