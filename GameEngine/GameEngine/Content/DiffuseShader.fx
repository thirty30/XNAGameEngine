#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
texture Texture;
float3 LightDir;
float3 LightColor;
float3 AmbientColor;
float3 CameraPosition;
float3 SpecularColor;

sampler BasicTextureSampler = sampler_state
{
	texture = <Texture>;
	MinFilter = Anisotropic;
	MagFilter = Linear;
	MinFilter = Linear;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
	float3 ViewDir : TEXCOORD1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.UV = input.UV;
	output.Normal = normalize(input.Normal);
	output.ViewDir = normalize(output.Position - CameraPosition);
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float3 diffuse = tex2D(BasicTextureSampler, input.UV);
	float3 lambertian = saturate(dot(-LightDir, input.Normal)) * LightColor;
	float3 ref = reflect(-LightDir, input.Normal);
	float3 specular = pow(saturate(dot(ref, input.ViewDir)), 4) * SpecularColor;
	float3 output = (saturate(lambertian + AmbientColor) + specular) * diffuse;
	return float4(output, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};