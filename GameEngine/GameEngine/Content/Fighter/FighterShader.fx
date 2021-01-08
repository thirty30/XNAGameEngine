#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix matM;
matrix matMVP;
matrix matMTI;

float3 DiffuseColor;
texture TexDiffuse;
texture TexNormal;
float2 Tiling;
float3 LightDir;
float3 LightColor;
float3 LightPosition;
float LightAttenuation;
float LightCutOffDistance;
float3 AmbientColor;
float3 CameraPosition;
float3 SpecularColor;
float SpecularPower;
bool UseNormalTexture;


sampler DiffuseTextureSampler = sampler_state
{
	texture = <TexDiffuse>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Linear;
};

sampler NormalTextureSampler = sampler_state
{
	texture = <TexNormal>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
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
	float3 WorldPosition : TEXCOORD2;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, matMVP);
	output.UV = input.UV * Tiling;
	output.Normal = normalize(mul(input.Normal, matM));
	output.WorldPosition = mul(input.Position, matM);
	output.ViewDir = normalize(output.WorldPosition - CameraPosition);
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float3 diffuse = DiffuseColor * tex2D(DiffuseTextureSampler, input.UV);
	LightDir = normalize(LightDir);
	float3 normal = input.Normal;
	if (UseNormalTexture == true)
	{
		normal = tex2D(NormalTextureSampler, input.UV).rgb;
		normal = normal * 2 - 1;
	}

	float3 lambertian = saturate(dot(-LightDir, normal)) * LightColor;
	float3 ref = reflect(-LightDir, normal);
	float3 specular = pow(saturate(dot(ref, input.ViewDir)), SpecularPower) * SpecularColor;
	float3 output = (lambertian + AmbientColor + specular) * diffuse;
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
