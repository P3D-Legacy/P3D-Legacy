//--------------------------- BASIC PROPERTIES ------------------------------
// The world transformation
float4x4 World;

// The view transformation
float4x4 View;

// The projection transformation
float4x4 Projection;

// The transpose of the inverse of the world transformation,
// used for transforming the vertex's normal
float4x4 WorldInverseTranspose;

//--------------------------- DIFFUSE LIGHT PROPERTIES ------------------------------
// The direction of the diffuse light
float3 DiffuseLightDirection = float3(1, 0, 0);

// The color of the diffuse light
float4 DiffuseColor = float4(1, 1, 1, 1);

// The intensity of the diffuse light
float DiffuseIntensity = 1.0;

//--------------------------- TOON SHADER PROPERTIES ------------------------------
// The color to draw the lines in.  Black is a good default.
float4 LineColor = float4(0, 0, 0, 1);

// The thickness of the lines.  This may need to change, depending on the scale of
// the objects you are drawing.
float LineThickness = .03;

//--------------------------- TEXTURE PROPERTIES ------------------------------
// The texture being used for the object
texture Texture;

// The texture sampler, which will get the texture color
sampler2D textureSampler = sampler_state
{
	Texture = (Texture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

//--------------------------- DATA STRUCTURES ------------------------------
// The structure used to store information between the application and the
// vertex shader
struct AppToVertex
{
	float4 Position : POSITION0;            // The position of the vertex
	float3 Normal : NORMAL0;                // The vertex's normal
	float2 TextureCoordinate : TEXCOORD0;    // The texture coordinate of the vertex
};

// The structure used to store information between the vertex shader and the
// pixel shader
struct VertexToPixel
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : TEXCOORD1;
};

//--------------------------- SHADERS ------------------------------
// The vertex shader that does cel shading.
// It really only does the basic transformation of the vertex location,
// and normal, and copies the texture coordinate over.
VertexToPixel CelVertexShader(AppToVertex input)
{
	VertexToPixel output;

	// Transform the position
	float4 worldPosition = mul(input.Position, World);
		float4 viewPosition = mul(worldPosition, View);
		output.Position = mul(viewPosition, Projection);

	// Transform the normal
	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));

	// Copy over the texture coordinate
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

// The pixel shader that does cel shading.  Basically, it calculates
// the color like is should, and then it discretizes the color into
// one of four colors.
float4 CelPixelShader(VertexToPixel input) : COLOR0
{
	// Calculate diffuse light amount
	float intensity = dot(normalize(DiffuseLightDirection), input.Normal);
	if (intensity < 0)
		intensity = 0;

	// Calculate what would normally be the final color, including texturing and diffuse lighting
	float4 color = tex2D(textureSampler, input.TextureCoordinate) * DiffuseColor * DiffuseIntensity;
		color.a = 1;

	// Discretize the intensity, based on a few cutoff points
	if (intensity > 0.95)
		color = float4(1.0, 1, 1, 1.0) * color;
	else if (intensity > 0.5)
		color = float4(0.7, 0.7, 0.7, 1.0) * color;
	else if (intensity > 0.05)
		color = float4(0.35, 0.35, 0.35, 1.0) * color;
	else
		color = float4(0.1, 0.1, 0.1, 1.0) * color;

	return color;
}

// The vertex shader that does the outlines
VertexToPixel OutlineVertexShader(AppToVertex input)
{
	VertexToPixel output = (VertexToPixel)0;

	// Calculate where the vertex ought to be.  This line is equivalent
	// to the transformations in the CelVertexShader.
	float4 original = mul(mul(mul(input.Position, World), View), Projection);

		// Calculates the normal of the vertex like it ought to be.
		float4 normal = mul(mul(mul(input.Normal, World), View), Projection);

		// Take the correct "original" location and translate the vertex a little
		// bit in the direction of the normal to draw a slightly expanded object.
		// Later, we will draw over most of this with the right color, except the expanded
		// part, which will leave the outline that we want.
		output.Position = original + (mul(LineThickness, normal));

	return output;
}

// The pixel shader for the outline.  It is pretty simple:  draw everything with the
// correct line color.
float4 OutlinePixelShader(VertexToPixel input) : COLOR0
{
	return LineColor;
}

// The entire technique for doing toon shading
technique Toon
{
	// The first pass will go through and draw the back-facing triangles with the outline shader,
	// which will draw a slightly larger version of the model with the outline color.  Later, the
	// model will get drawn normally, and draw over the top most of this, leaving only an outline.
	pass Pass1
	{
		VertexShader = compile vs_2_0 OutlineVertexShader();
		PixelShader = compile ps_2_0 OutlinePixelShader();
		CullMode = CW;
	}

	// The second pass will draw the model like normal, but with the cel pixel shader, which will
	// color the model with certain colors, giving us the cel/toon effect that we are looking for.
	pass Pass2
	{
		VertexShader = compile vs_2_0 CelVertexShader();
		PixelShader = compile ps_2_0 CelPixelShader();
		CullMode = CCW;
	}
}
