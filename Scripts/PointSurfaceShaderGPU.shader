Shader "Graph/Point Surface GPU" 
//Tells unity that this is shader file and what it will change 

{
	Properties{
		_Smoothness("Smoothness", Range(0,1)) = 0.5
		//Adding some properties to the shader i have really know clue what this does
	}
	SubShader
	{
		CGPROGRAM
		#pragma surface ConfigureSurface Standard fullforwardshadows addshadow
		#pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
		#pragma target 4.5
		//First pragma tells shaders to be fully lighting and shadow supported 
		//Second Pragma sets minumun target level and quality for those shaders 
		struct Input
		{
			float3 worldPos;
	//tells shaders that they are effected by position
	};
	float _Smoothness;
	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float3> _Positions;
	#endif
	float _Step;
	void ConfigureProcedural() {
		#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		float3 position = _Positions[unity_InstanceID];
		unity_ObjectToWorld = 0.0;
		//Setting off set via column vector to 4th column 
		unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
		unity_ObjectToWorld._m00_m11_m22 = _Step;
		#endif
	}

	void ConfigureSurface(Input input, inout SurfaceOutputStandard surface)
	{
		surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
		surface.Smoothness = _Smoothness;
		//tells the shaders how world position effects them 
	}
		ENDCG
	}
	FallBack "Diffuse"
//what it does good question
}