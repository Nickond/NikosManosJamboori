Shader "MyShaders/Decal"
{
	Properties
	{
        _Colour ("Main Colour", Color) = (1,1,1,1)
        _DecalColour ("Decal Colour", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
        _DecalTex ("Decal", 2D) = "black" {}
        _DecalEmission ("Decal Emissive Value", Range (1, 10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 250

		CGPROGRAM
        
        #pragma surface surf Lambert
        
        // Data Declaration
        fixed4 _Colour;
        fixed4 _DecalColour;
        sampler2D _MainTex;
        sampler2D _DecalTex;
        float _DecalEmission;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DecalTex;
        };
        
        
        void surf(Input _in, inout SurfaceOutput _o)
        {
            fixed4 mainCol = tex2D(_MainTex, _in.uv_MainTex);
            half4 decCol = tex2D(_DecalTex, _in.uv_DecalTex);
            
            mainCol *= _Colour;
            decCol *= _DecalColour * _DecalEmission;
            
            mainCol.rgb = lerp(mainCol.rgb, decCol.rgb, decCol.a);
            
            
            _o.Albedo = mainCol.rgb;
            _o.Alpha = mainCol.a;
        }

        ENDCG
	}
    
    Fallback "Legacy Shaders/Diffuse"
}
