//@author: vux
//@help: standard constant shader
//@tags: color
//@credits: 

Texture2D texture2d <string uiname="Texture";>;

SamplerState g_samLinear : IMMUTABLE
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};
int ViewIndex: VIEWPORTINDEX;
int InstanceStartIndex = 0;
//StructuredBuffer<float4x4> world;
//StructuredBuffer<float> values;
float value=0.5;
cbuffer cbPerDraw : register( b0 )
{
	float4x4 tVP : VIEWPROJECTION;
};



cbuffer cbPerObj : register( b1 )
{
	float4x4 tW : WORLD;
	float Alpha <float uimin=0.0; float uimax=1.0;> = 1; 
	float4 cAmb <bool color=true;String uiname="Color";> = { 1.0f,1.0f,1.0f,1.0f };
	float4x4 tTex <string uiname="Texture Transform"; bool uvspace=true; >;
	float4x4 tColor <string uiname="Color Transform";>;
};

struct VS_IN
{
	float4 PosO : POSITION;
	float4 TexCd : TEXCOORD0;
//	uint ii : SV_InstanceID; //Object instance ID1

};

struct vs2ps
{
    float4 PosWVP: SV_POSITION;
    float4 TexCd: TEXCOORD0;
//	float4 Vcol: Color;
	
};

vs2ps VS(VS_IN input)
{
    vs2ps Out = (vs2ps)0;
	//Get the current world transform from buffer.
	//So index is instance ID + start index
//	float4x4 w = world[input.ii + InstanceStartIndex];
	
    Out.PosWVP  = mul(input.PosO,mul(tW,tVP));
    Out.TexCd = mul(input.TexCd, tTex);
//	Out.Vcol = ((values[input.ii+ InstanceStartIndex]>input.TexCd.x)*.3+.1);
    return Out;
}




float4 PS(vs2ps In): SV_Target
{
    float4 col = texture2d.Sample(g_samLinear,In.TexCd.xy) * cAmb;
	
	col = mul(col, tColor)*((value>In.TexCd.x)*.1)+.1;
	col.a *= Alpha;
    return col;
}





technique10 Constant
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}




