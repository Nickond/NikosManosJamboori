Shader "Unlit/Unlit_Decal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _DecalTex ("Texture", 2D) = "black" {} 
        _Colour ("Main Colour", Color) = (1,1,1,1)
        _DecalColour ("Decal Colour", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off 
        ZWrite On
        
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                float2 d_uv : TEXCOORD1;

				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            sampler2D _DecalTex;
            fixed4 _Colour;
            fixed4 _DecalColour;
			float4 _MainTex_ST;
			
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
				fixed4 col = tex2D(_MainTex, i.uv) * _Colour;
                fixed4 dCol = tex2D(_DecalTex, i.d_uv) * _DecalColour;
                
                fixed4 c =  tex2D( _MainTex, i.uv ) * _Colour + 1 * tex2D( _DecalTex, i.uv ) * _DecalColour;
                
                //dCol.rgb = lerp(dCol.rgb, col.rgb, col.a);
                col.rgb = lerp(col.rgb, dCol.rgb, dCol.a);
                
                c.rgb *= 5;
                
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return c;
			}
			ENDCG
		}
	}
}
