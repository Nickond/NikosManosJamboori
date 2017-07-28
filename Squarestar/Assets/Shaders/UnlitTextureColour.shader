Shader "Unlit/Texture_Colour"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Colour ("Colour", Color) = (1,1,1,1)
        _EmissionColour("Emission", Color) = (0,0,0)
        _EmissionMap("Emission Texture", 2D) = "white" {}
        _Blend ("Blend Amount", Range(0,1)) = 0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		// LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off Lighting Off ZWrite On
        
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma shader_feature _EMISSION
            
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                fixed4 colour : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                fixed4 colour : COLOR;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            sampler2D _EmissionMap;
			float4 _MainTex_ST;
			fixed4 _Colour;
            fixed4 _EmissionColour;
            float _Blend;
            
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.colour = v.colour;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Colour;// + emCol;
                half4 dCol = tex2D(_EmissionMap, i.uv) * _EmissionColour;
                
                col.rgb = lerp(col.rgb, dCol.rgb, dCol.a);
                
                //col = col + emCol;
                
                //col = lerp(col, emCol, _Blend);
                
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
