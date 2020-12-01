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
float3 CameraPosition;
texture Texture;

samplerCUBE BasicTextureSampler = sampler_state
{
	texture = <Texture>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, matMVP);
	float3 vertexPos = mul(input.Position, matM);
	output.UV = normalize(vertexPos - CameraPosition);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return texCUBE(BasicTextureSampler, input.UV);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};