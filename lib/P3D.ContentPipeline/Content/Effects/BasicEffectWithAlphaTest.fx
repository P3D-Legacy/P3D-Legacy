//-----------------------------------------------------------------------------
// BasicEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);


BEGIN_CONSTANTS

    float4 DiffuseColor             _vs(c0)  _ps(c1)  _cb(c0);
    float3 EmissiveColor            _vs(c1)  _ps(c2)  _cb(c1);
    float3 SpecularColor            _vs(c2)  _ps(c3)  _cb(c2);
    float  SpecularPower            _vs(c3)  _ps(c4)  _cb(c2.w);

    float3 DirLight0Direction       _vs(c4)  _ps(c5)  _cb(c3);
    float3 DirLight0DiffuseColor    _vs(c5)  _ps(c6)  _cb(c4);
    float3 DirLight0SpecularColor   _vs(c6)  _ps(c7)  _cb(c5);

    float3 DirLight1Direction       _vs(c7)  _ps(c8)  _cb(c6);
    float3 DirLight1DiffuseColor    _vs(c8)  _ps(c9)  _cb(c7);
    float3 DirLight1SpecularColor   _vs(c9)  _ps(c10) _cb(c8);

    float3 DirLight2Direction       _vs(c10) _ps(c11) _cb(c9);
    float3 DirLight2DiffuseColor    _vs(c11) _ps(c12) _cb(c10);
    float3 DirLight2SpecularColor   _vs(c12) _ps(c13) _cb(c11);

    float3 EyePosition              _vs(c13) _ps(c14) _cb(c12);

    float3 FogColor                          _ps(c0)  _cb(c13);
    float4 FogVector                _vs(c14)          _cb(c14);

    float4x4 World                  _vs(c19)          _cb(c15);
    float3x3 WorldInverseTranspose  _vs(c23)          _cb(c19);
    
    float AlphaTest                          _ps(c15) _cb(c20);

MATRIX_CONSTANTS

    float4x4 WorldViewProj          _vs(c15)          _cb(c0);

END_CONSTANTS


#include "Structures.fxh"
#include "Common.fxh"
#include "Lighting.fxh"


// Vertex shader: basic.
VSOutput VSBasic(VSInput vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: basic.
VSOutput VSHWBasic(VSInput vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: no fog.
VSOutputNoFog VSBasicNoFog(VSInput vin)
{
    VSOutputNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    return vout;
}


// Vertex shader: no fog.
VSOutputNoFog VSHWBasicNoFog(VSInput vin, VSHWInputInstance vhwin)
{
    VSOutputNoFog vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    return vout;
}


// Vertex shader: vertex color.
VSOutput VSBasicVc(VSInputVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex color.
VSOutput VSHWBasicVc(VSInputVc vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex color, no fog.
VSOutputNoFog VSBasicVcNoFog(VSInputVc vin)
{
    VSOutputNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex color, no fog.
VSOutputNoFog VSHWBasicVcNoFog(VSInputVc vin, VSHWInputInstance vhwin)
{
    VSOutputNoFog vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture.
VSOutputTx VSBasicTx(VSInputTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture.
VSOutputTx VSHWBasicTx(VSInputTx vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture, no fog.
VSOutputTxNoFog VSBasicTxNoFog(VSInputTx vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture, no fog.
VSOutputTxNoFog VSHWBasicTxNoFog(VSInputTx vin, VSHWInputInstance vhwin)
{
    VSOutputTxNoFog vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture + vertex color.
VSOutputTx VSBasicTxVc(VSInputTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture + vertex color.
VSOutputTx VSHWBasicTxVc(VSInputTxVc vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture + vertex color, no fog.
VSOutputTxNoFog VSBasicTxVcNoFog(VSInputTxVc vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture + vertex color, no fog.
VSOutputTxNoFog VSHWBasicTxVcNoFog(VSInputTxVc vin, VSHWInputInstance vhwin)
{
    VSOutputTxNoFog vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting.
VSOutput VSBasicVertexLighting(VSInputNm vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: vertex lighting.
VSOutput VSHWBasicVertexLighting(VSInputNm vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: vertex lighting + vertex color.
VSOutput VSBasicVertexLightingVc(VSInputNmVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting + vertex color.
VSOutput VSHWBasicVertexLightingVc(VSInputNmVc vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting + texture.
VSOutputTx VSBasicVertexLightingTx(VSInputNmTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex lighting + texture.
VSOutputTx VSHWBasicVertexLightingTx(VSInputNmTx vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex lighting + texture + vertex color.
VSOutputTx VSBasicVertexLightingTxVc(VSInputNmTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting + texture + vertex color.
VSOutputTx VSHWBasicVertexLightingTxVc(VSInputNmTxVc vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light.
VSOutput VSBasicOneLight(VSInputNm vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: one light.
VSOutput VSHWBasicOneLight(VSInputNm vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: one light + vertex color.
VSOutput VSBasicOneLightVc(VSInputNmVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light + vertex color.
VSOutput VSHWBasicOneLightVc(VSInputNmVc vin, VSHWInputInstance vhwin)
{
    VSOutput vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light + texture.
VSOutputTx VSBasicOneLightTx(VSInputNmTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light + texture.
VSOutputTx VSHWBasicOneLightTx(VSInputNmTx vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light + texture + vertex color.
VSOutputTx VSBasicOneLightTxVc(VSInputNmTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light + texture + vertex color.
VSOutputTx VSHWBasicOneLightTxVc(VSInputNmTxVc vin, VSHWInputInstance vhwin)
{
    VSOutputTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: pixel lighting.
VSOutputPixelLighting VSBasicPixelLighting(VSInputNm vin)
{
    VSOutputPixelLighting vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;

    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    
    return vout;
}


// Vertex shader: pixel lighting.
VSOutputPixelLighting VSHWBasicPixelLighting(VSInputNm vin, VSHWInputInstance vhwin)
{
    VSOutputPixelLighting vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;

    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    
    return vout;
}


// Vertex shader: pixel lighting + vertex color.
VSOutputPixelLighting VSBasicPixelLightingVc(VSInputNmVc vin)
{
    VSOutputPixelLighting vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    
    return vout;
}


// Vertex shader: pixel lighting + vertex color.
VSOutputPixelLighting VSHWBasicPixelLightingVc(VSInputNmVc vin, VSHWInputInstance vhwin)
{
    VSOutputPixelLighting vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    
    return vout;
}


// Vertex shader: pixel lighting + texture.
VSOutputPixelLightingTx VSBasicPixelLightingTx(VSInputNmTx vin)
{
    VSOutputPixelLightingTx vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting + texture.
VSOutputPixelLightingTx VSHWBasicPixelLightingTx(VSInputNmTx vin, VSHWInputInstance vhwin)
{
    VSOutputPixelLightingTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting + texture + vertex color.
VSOutputPixelLightingTx VSBasicPixelLightingTxVc(VSInputNmTxVc vin)
{
    VSOutputPixelLightingTx vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    vout.TexCoord = vin.TexCoord;
    
    return vout;
}


// Vertex shader: pixel lighting + texture + vertex color.
VSOutputPixelLightingTx VSHWBasicPixelLightingTxVc(VSInputNmTxVc vin, VSHWInputInstance vhwin)
{
    VSOutputPixelLightingTx vout;
    
    ApplyPositionOffset(vin.Position, vhwin.Position);
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    vout.TexCoord = vin.TexCoord;
    
    return vout;
}


// Pixel shader: basic.
float4 PSBasic(VSOutput pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: no fog.
float4 PSBasicNoFog(VSOutputNoFog pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);

    return color;
}


// Pixel shader: texture.
float4 PSBasicTx(VSOutputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: texture, no fog.
float4 PSBasicTxNoFog(VSOutputTxNoFog pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);

    return color;
}


// Pixel shader: vertex lighting.
float4 PSBasicVertexLighting(VSOutput pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);

    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: vertex lighting, no fog.
float4 PSBasicVertexLightingNoFog(VSOutput pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: vertex lighting + texture.
float4 PSBasicVertexLightingTx(VSOutputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: vertex lighting + texture, no fog.
float4 PSBasicVertexLightingTxNoFog(VSOutputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: pixel lighting.
float4 PSBasicPixelLighting(VSOutputPixelLighting pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = normalize(pin.NormalWS);
    
    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, 3);

    color.rgb *= lightResult.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);
    
    AddSpecular(color, lightResult.Specular);
    ApplyFog(color, pin.PositionWS.w);
    
    return color;
}


// Pixel shader: pixel lighting + texture.
float4 PSBasicPixelLightingTx(VSOutputPixelLightingTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = normalize(pin.NormalWS);
    
    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, 3);
    
    color.rgb *= lightResult.Diffuse;

    clip(color.a <= AlphaTest ? -1 : 1);

    AddSpecular(color, lightResult.Specular);
    ApplyFog(color, pin.PositionWS.w);
    
    return color;
}


// NOTE: The order of the techniques here are
// defined to match the indexing in BasicEffect.cs.

TECHNIQUE( BasicEffect,								VSBasic,			PSBasic );
TECHNIQUE( BasicEffect_NoFog,						VSBasicNoFog,		PSBasicNoFog );
TECHNIQUE( BasicEffect_VertexColor,					VSBasicVc,			PSBasic );
TECHNIQUE( BasicEffect_VertexColor_NoFog,			VSBasicVcNoFog,		PSBasicNoFog );
TECHNIQUE( BasicEffect_Texture,						VSBasicTx,			PSBasicTx );
TECHNIQUE( BasicEffect_Texture_NoFog,				VSBasicTxNoFog,		PSBasicTxNoFog );
TECHNIQUE( BasicEffect_Texture_VertexColor,			VSBasicTxVc,		PSBasicTx );
TECHNIQUE( BasicEffect_Texture_VertexColor_NoFog,	VSBasicTxVcNoFog,	PSBasicTxNoFog );

TECHNIQUE( BasicEffect_VertexLighting,								VSBasicVertexLighting,		PSBasicVertexLighting );
TECHNIQUE( BasicEffect_VertexLighting_NoFog,						VSBasicVertexLighting,		PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_VertexLighting_VertexColor,					VSBasicVertexLightingVc,	PSBasicVertexLighting );
TECHNIQUE( BasicEffect_VertexLighting_VertexColor_NoFog,			VSBasicVertexLightingVc,	PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_VertexLighting_Texture,						VSBasicVertexLightingTx,	PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_VertexLighting_Texture_NoFog,				VSBasicVertexLightingTx,	PSBasicVertexLightingTxNoFog );
TECHNIQUE( BasicEffect_VertexLighting_Texture_VertexColor,			VSBasicVertexLightingTxVc,	PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_VertexLighting_Texture_VertexColor_NoFog,	VSBasicVertexLightingTxVc,	PSBasicVertexLightingTxNoFog );

TECHNIQUE( BasicEffect_OneLight,							VSBasicOneLight,		PSBasicVertexLighting );
TECHNIQUE( BasicEffect_OneLight_NoFog,						VSBasicOneLight,		PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_OneLight_VertexColor,				VSBasicOneLightVc,		PSBasicVertexLighting );
TECHNIQUE( BasicEffect_OneLight_VertexColor_NoFog,			VSBasicOneLightVc,		PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_OneLight_Texture,					VSBasicOneLightTx,		PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_OneLight_Texture_NoFog,				VSBasicOneLightTx,		PSBasicVertexLightingTxNoFog );
TECHNIQUE( BasicEffect_OneLight_Texture_VertexColor,		VSBasicOneLightTxVc,	PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_OneLight_Texture_VertexColor_NoFog,	VSBasicOneLightTxVc,	PSBasicVertexLightingTxNoFog );

TECHNIQUE( BasicEffect_PixelLighting,							VSBasicPixelLighting,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_PixelLighting_NoFog,						VSBasicPixelLighting,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_PixelLighting_VertexColor,				VSBasicPixelLightingVc,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_PixelLighting_VertexColor_NoFog,			VSBasicPixelLightingVc,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_PixelLighting_Texture,					VSBasicPixelLightingTx,		PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_PixelLighting_Texture_NoFog,				VSBasicPixelLightingTx,		PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_PixelLighting_Texture_VertexColor,		VSBasicPixelLightingTxVc,	PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_PixelLighting_Texture_VertexColor_NoFog,	VSBasicPixelLightingTxVc,	PSBasicPixelLightingTx );

TECHNIQUE( BasicEffect_HardwareInstancing,								VSHWBasic,			PSBasic );
TECHNIQUE( BasicEffect_HardwareInstancing_NoFog,						VSHWBasicNoFog,		PSBasicNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexColor,					VSHWBasicVc,		PSBasic );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexColor_NoFog,			VSHWBasicVcNoFog,	PSBasicNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_Texture,						VSHWBasicTx,		PSBasicTx );
TECHNIQUE( BasicEffect_HardwareInstancing_Texture_NoFog,				VSHWBasicTxNoFog,	PSBasicTxNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_Texture_VertexColor,			VSHWBasicTxVc,		PSBasicTx );
TECHNIQUE( BasicEffect_HardwareInstancing_Texture_VertexColor_NoFog,	VSHWBasicTxVcNoFog, PSBasicTxNoFog );

TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting,							VSHWBasicVertexLighting,	    PSBasicVertexLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_NoFog,						VSHWBasicVertexLighting,	    PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_VertexColor,				VSHWBasicVertexLightingVc,	    PSBasicVertexLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_VertexColor_NoFog,			VSHWBasicVertexLightingVc,	    PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_Texture,					VSHWBasicVertexLightingTx,	    PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_Texture_NoFog,				VSHWBasicVertexLightingTx,	    PSBasicVertexLightingTxNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_Texture_VertexColor,		VSHWBasicVertexLightingTxVc,    PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_VertexLighting_Texture_VertexColor_NoFog, VSHWBasicVertexLightingTxVc,    PSBasicVertexLightingTxNoFog );

TECHNIQUE( BasicEffect_HardwareInstancing_OneLight,							    VSHWBasicOneLight,		PSBasicVertexLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_NoFog,						VSHWBasicOneLight,		PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_VertexColor,				    VSHWBasicOneLightVc,	PSBasicVertexLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_VertexColor_NoFog,			VSHWBasicOneLightVc,	PSBasicVertexLightingNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_Texture,					    VSHWBasicOneLightTx,	PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_Texture_NoFog,				VSHWBasicOneLightTx,	PSBasicVertexLightingTxNoFog );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_Texture_VertexColor,		    VSHWBasicOneLightTxVc,  PSBasicVertexLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_OneLight_Texture_VertexColor_NoFog,	VSHWBasicOneLightTxVc,  PSBasicVertexLightingTxNoFog );

TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting,							VSHWBasicPixelLighting,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_NoFog,						VSHWBasicPixelLighting,		PSBasicPixelLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_VertexColor,				VSHWBasicPixelLightingVc,	PSBasicPixelLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_VertexColor_NoFog,			VSHWBasicPixelLightingVc,	PSBasicPixelLighting );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_Texture,					VSHWBasicPixelLightingTx,	PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_Texture_NoFog,				VSHWBasicPixelLightingTx,   PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_Texture_VertexColor,		VSHWBasicPixelLightingTxVc,	PSBasicPixelLightingTx );
TECHNIQUE( BasicEffect_HardwareInstancing_PixelLighting_Texture_VertexColor_NoFog,	VSHWBasicPixelLightingTxVc, PSBasicPixelLightingTx );