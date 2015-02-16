//@author: vux
//@help: standard constant shader
//@tags: color
//@credits: 

Texture2D texture2d <string uiname="Texture";>;

SamplerState g_samLinear : IMMUTABLE
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};
int ViewIndex: VIEWPORTINDEX;
int InstanceStartIndex = 0;
StructuredBuffer<float4x4> world;
StructuredBuffer<float4x4> TexCords <bool uvspace=true; >;
StructuredBuffer<float4> InstanceColour<bool color=true;>;
StructuredBuffer<int> mute;

cbuffer cbPerDraw : register( b0 )
{
	float4x4 tVP : VIEWPROJECTION;
};



cbuffer cbPerObj : register( b1 )
{
	float4x4 tW : WORLD;
	float Alpha <float uimin=0.0; float uimax=1.0;> = 1; 
	float4 cBG <bool color=true;String uiname="Color";> = { 1.0f,1.0f,1.0f,1.0f };
	float4 cSlider <bool color=true;String uiname="Color2";> = { 0.0f,1.0f,1.0f,1.0f };
	float4x4 tTex <string uiname="Texture Transform"; bool uvspace=true; >;
	float4x4 tColor <string uiname="Color Transform";>;
};

struct VS_IN
{
	float4 PosO : POSITION;
	float4 TexCd : TEXCOORD0;
	uint ii : SV_InstanceID; //Object instance ID1

};

struct vs2ps
{
    float4 PosWVP: SV_POSITION;
    float4 TexCd: TEXCOORD0;
//	float4 Vcol: Color;
//	float4 TexCd2: TEXCOORD1;
	
};

vs2ps VS(VS_IN input)
{
    vs2ps Out = (vs2ps)0;
	//Get the current world transform from buffer.
	//So index is instance ID + start index
	float4x4 w = world[input.ii + InstanceStartIndex];

    Out.PosWVP  = mul(input.PosO *(1-mute[input.ii]) ,mul(w,tVP));
    Out.TexCd = mul(input.TexCd, TexCords[input.ii + InstanceStartIndex]);
//	Out.TexCd2 = values[input.ii+ InstanceStartIndex];//((values[input.ii+ InstanceStartIndex]>input.TexCd.x)*.3+.1);
//    Out.Vcol =  Groups[input.ii];///so we can tint the sliders into groups
	return Out;
}




float4 PS(vs2ps In): SV_Target
{
    float4 col = texture2d.Sample(g_samLinear,In.TexCd.xy)*cBG ;
	
	//col = mul(col, tColor)*(In.TexCd2.x>In.TexCd.x);
//	float temp =(In.TexCd2.x>In.TexCd.x);
//	col = (temp*cBG*In.Vcol)+((1-temp)*cSlider)*In.Vcol;
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




