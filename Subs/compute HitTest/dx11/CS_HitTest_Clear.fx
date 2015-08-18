
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;
StructuredBuffer<float4x4> transforms ;
float4x4 tV ;
float4x4 tP ;
float2 mouse ;
//Quad position/uvs
float4 g_positions[2]: IMMUTABLE =
{
float4( -1, -1, 0 ,1 ),
float4( 1, 1, 0, 1 ),
};




[numthreads(1, 1, 1)]
void CS( uint3 ii : SV_DispatchThreadID)
{ 
	//Read color and writed to buffer
float4x4	tVP = mul(tV,tP);
	
 rwbuffer[0] = -1 ;



}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_5_0, CS() ) );
	}
}







