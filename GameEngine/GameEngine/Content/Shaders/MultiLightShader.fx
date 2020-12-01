#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
#endif

matrix matM;
matrix matMVP;
matrix matMTI;
float3 DiffuseColor;
texture Texture;
float2 Tiling;
float3 AmbientColor;
float3 CameraPosition;
float3 SpecularColor;
float SpecularPower;

#define LightsLimit 8
int LightNum;
int LightType[LightsLimit];
float3 LightPosition[LightsLimit];
float3 LightDir[LightsLimit];
float3 LightColor[LightsLimit];
float LightAttenuation[LightsLimit];
float LightCutOffDistance[LightsLimit];
float ConeAngle[LightsLimit];

float fogStart = 50;
float fogEnd = 90;
float3 fogColor = float3(0.2, 0.2, 0.2);


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
	float3 diffuse = DiffuseColor * tex2D(BasicTextureSampler, input.UV);
	float3 output = float3(0, 0, 0);
	for (int i = 0; i < LightNum; i++)
	{
		if (LightType[i] == 0) // direction light
		{
			float3 lightDir = normalize(LightDir[i]);
			float3 lambertian = saturate(dot(-lightDir, input.Normal)) * LightColor[i];
			float3 ref = reflect(-lightDir, input.Normal);
			float3 specular = pow(saturate(dot(ref, input.ViewDir)), SpecularPower) * SpecularColor;
			output += (lambertian + AmbientColor + specular) * diffuse;
		}
		else if (LightType[i] == 1)	// point light
		{
			float3 light2vertex = normalize(input.WorldPosition - LightPosition[i]);
			float3 lambertian = saturate(dot(-light2vertex, input.Normal)) * LightColor[i];
			float dis = distance(input.WorldPosition, LightPosition[i]);
			float att = pow(1.0 - saturate(dis / LightAttenuation[i]), LightCutOffDistance[i]);
			float3 ref = reflect(-light2vertex, input.Normal);
			float3 specular = pow(saturate(dot(ref, input.ViewDir)), SpecularPower) * SpecularColor;
			output += AmbientColor * diffuse + att * diffuse * (lambertian + specular);
		}
		else if (LightType[i] == 2) // spot light
		{
			float3 light2vertex = normalize(input.WorldPosition - LightPosition[i]);
			float3 spotDir = normalize(LightDir[i]);
			float3 lambertian = saturate(dot(-light2vertex, input.Normal)) * LightColor[i];
			float dis = distance(input.WorldPosition, LightPosition[i]);
			float d = saturate(dot(light2vertex, spotDir));
			float a = cos(radians(ConeAngle[i]));
			float att = 0;
			if (a <= d) { att = 1.0 - pow(saturate(a / d), LightCutOffDistance[i]); }
			float3 ref = reflect(-light2vertex, input.Normal);
			float3 specular = pow(saturate(dot(ref, input.ViewDir)), SpecularPower) * SpecularColor;
			output += AmbientColor * diffuse + att * diffuse * (lambertian + specular);
		}
	}
	float fogDis = length(input.WorldPosition - CameraPosition);
	float fog = saturate((fogDis - fogStart) / (fogEnd - fogStart));
	output = lerp(output, fogColor, fog);
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