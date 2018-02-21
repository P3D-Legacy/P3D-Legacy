//------------------------------------------- Defines -------------------------------------------

#define Pi 3.14159265

//------------------------------------- Top Level Variables -------------------------------------

// Top level variables can and have to be set at runtime
float4 AmbientColor;
float AmbientIntensity;
float4 DiffuseColor;
texture ScreenTexture;
texture2D SpotlightTexture;

float WorldRotation;

int TextureWidth;

// Matrices for 3D perspective projection
float4x4 View, Projection, World, WorldIT;

//---------------------------------- Input / Output structures ----------------------------------

// Each member of the struct has to be given a "semantic", to indicate what kind of data should go in
// here and how it should be treated. Read more about the POSITION0 and the many other semantics in
// the MSDN library
struct VertexShaderInput
{
	float4 Position3D : POSITION0;
	float4 Normals3D : NORMAL0;
};

// The output of the vertex shader. After being passed through the interpolator/rasterizer it is also
// the input of the pixel shader.
// Note 1: The values that you pass into this struct in the vertex shader are not the same as what
// you get as input for the pixel shader. A vertex shader has a single vertex as input, the pixel
// shader has 3 vertices as input, and lets you determine the color of each pixel in the triangle
// defined by these three vertices. Therefor, all the values in the struct that you get as input for
// the pixel shaders have been linearly interpolated between there three vertices!
// Note 2: You cannot use the data with the POSITION0 semantic in the pixel shader.
struct VertexShaderOutput
{
	float4 Position2D : POSITION0;
	float4 Normals : TEXCOORD0;
	float4 lightDir : TEXCOORD1;
	float4 lambertLightDir : TEXCOORD2;
	float4 Position : TEXCOORD3;
};

//---------------------------------------- Technique: Simple ----------------------------------------

VertexShaderOutput SimpleVertexShader(VertexShaderInput input)
{
	// Allocate an empty output struct
	VertexShaderOutput output = (VertexShaderOutput)0;

	input.Position3D.w = 1.0f;

	// Do the matrix multiplications for perspective projection and the world transform
	float4 worldPosition = mul(input.Position3D, World);
		float4 viewPosition = mul(worldPosition, View);
		output.Position2D = mul(viewPosition, Projection);
	output.Position = mul(viewPosition, Projection);
	output.Normals = mul(input.Normals3D, WorldIT);
	float4 lightPoint = { -3, -2, -2, 0 };
		output.lightDir = normalize(worldPosition - lightPoint);
	output.lambertLightDir = float4(30, 30, 20, 0);

	return output;
}

float4 SimplePixelShader(VertexShaderOutput input) : COLOR0
{

	float4 color = DiffuseColor * 0.2 * max(0, dot(input.Normals, input.lambertLightDir));
	color += AmbientColor * AmbientIntensity;

	return color;
}

float4 SpotlightPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 lightDirection = { 1, 1, 1, 0 };
	float theta = 20.0f;
	float phi = 40.0f;
	float4 color;
	float4 lambert = DiffuseColor * 0.2 * max(0, dot(input.Normals, input.lambertLightDir));
		float angle = acos(dot(input.lightDir, normalize(lightDirection)));

	if (angle > radians(phi))
		color = AmbientColor * AmbientIntensity;
	else if (angle < radians(theta))
		color = lambert;
	else
		color = max(AmbientColor * AmbientIntensity, smoothstep(radians(phi), radians(theta), angle) * lambert);

	return color;
}

sampler SpotlightTextureSampler  :register(s2) = sampler_state
{
	Texture = <SpotlightTexture>;
};

float4 TexturedSpotlightPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color;

	float2 TextureCoordinates;
	TextureCoordinates.x = input.Position.x / input.Position.w / 2.0f + 0.5f;
	TextureCoordinates.y = -input.Position.y / input.Position.w / 2.0f + 0.5f;

	float4 lightDirection = { 1, 1, 1, 0 };
		float theta = 20.0f;
	float phi = 40.0f;
	float angle = acos(dot(input.lightDir, normalize(lightDirection)));
	color = AmbientColor * AmbientIntensity + DiffuseColor * 0.2 * max(0, dot(input.Normals, input.lambertLightDir));
	color *= 0.6;
	if (angle < radians(phi))
		color += tex2D(SpotlightTextureSampler, TextureCoordinates);

	return color;
}

sampler TextureSampler = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 GrayscalePixelShader(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	// Get the color.
	float4 color = tex2D(TextureSampler, TextureCoordinate);

	// Turn pixel to grayscale.
	float grayscale = dot(color.rgb, float3(0.3, 0.59, 0.11));
	color.r = grayscale;
	color.g = grayscale;
	color.b = grayscale;
	color.a = 1.0f;

	// Return the result.
	return color;
}

float Pixels[13] =
{
	-6,
	-5,
	-4,
	-3,
	-2,
	-1,
	0,
	1,
	2,
	3,
	4,
	5,
	6,
};

float BlurWeights[13] =
{
	0.002216,
	0.008764,
	0.026995,
	0.064759,
	0.120985,
	0.176033,
	0.199471,
	0.176033,
	0.120985,
	0.064759,
	0.026995,
	0.008764,
	0.002216,
};


float4 GaussianPixelShader(float2 TextureCoordinate : TEXCOORD0) : COLOR
{
	// Pixel width
	float pixelWidth = 1.0 / (float)TextureWidth;

	float4 color = { 0, 0, 0, 1 };

	float2 blur;
	blur.y = TextureCoordinate.y;

	for (int i = 0; i < 13; i++)
	{
		blur.x = TextureCoordinate.x + Pixels[i] * (pixelWidth * 2);
		blur.y = TextureCoordinate.y + Pixels[i] * (pixelWidth * 2);
		color += tex2D(TextureSampler, blur.xy) * BlurWeights[i];
	}

	return color;
}


technique Simple
{

	pass Pass0
	{
		VertexShader = compile vs_2_0 SimpleVertexShader();
		PixelShader = compile ps_2_0 SimplePixelShader();
	}
}

technique Spotlight
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimpleVertexShader();
		PixelShader = compile ps_2_0 SpotlightPixelShader();
	}
}

technique Greyscale
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 GrayscalePixelShader();
	}
}

technique GaussianBlur
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 GaussianPixelShader();
	}
}

technique TexturedLight
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimpleVertexShader();
		PixelShader = compile ps_2_0 TexturedSpotlightPixelShader();
	}
}
