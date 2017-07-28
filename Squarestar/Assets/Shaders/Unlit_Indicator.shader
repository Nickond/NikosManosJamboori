Shader "Unlit/Unlit_Indicator_NoZwrite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Colour ("Main Colour", Color) = (1,1,1,1)
        _Intensity ("Indicator Intensity", Range(1, 2)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Lighting Off
        Cull Off
        ZWrite Off
       
        
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            fixed4 _Colour;
			float4 _MainTex_ST;
			float _Intensity;
            
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Colour * _Intensity;
				
				return col;
			}
			ENDCG
		}
	}
}
