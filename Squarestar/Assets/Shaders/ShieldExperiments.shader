Shader "Unlit/Shield_Experiments"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1)
		_Center1 ("Hole Center", Vector) = (0.5, 0.5, 0 , 0)
		_Center2 ("Hole Center", Vector) = (0.5, 0.5, 0 , 0)
		_Center3 ("Hole Center", Vector) = (0.5, 0.5, 0 , 0)
		_Radius ("Hole Radius", Float) = 0.25
		_Fill ("Hole Fill", Float) = 0.25
		_Hit ("Received a hit", Range(0, 1)) = 0

	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		// LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off 
		ZWrite Off

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
                fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			uniform half2 _Center1;
			uniform half2 _Center2;
			uniform half2 _Center3;
            half _Radius, _Fill;
            float _Hit;
			

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.uv = _MainTex_ST.xy * v.uv + _MainTex_ST.zw;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float MethodA(float a)
			{
				//if(a == 1.)
				//if(a == .9)
				if(a <= .995  && a >= .9)
					a = 0.;
				else if(a == 0.)
					a = 1.;

				return a;
			}

			float MethodB(float a)
			{
				if(a == 0.)
					a = 1.;
				else if(a == 1.)
					a = 0.;
				else if(a < 0.5)
					a += 0.5;
				else if(a > 0.5)
					a -= 0.5;

				return a;
			}


			fixed4 frag (v2f i) : SV_Target
			{
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;// + emCol;
                
				// if(_Hit == 1)
				if(true)
				{
					// Return on already transparent pixels
					if(col.a  <= 0.1)
					{
						return col;
					}

					half hole = min(distance(i.uv, _Center1) / _Radius, 1.);
					// col.a *= pow(hole, _Fill);
					col.a -= pow(hole, _Fill);

					
						// Method A:
						// col.a = MethodA(col.a);
						// Method B:
						// col.a = MethodB(col.a);
						// Method C:
						// col.a = 1. - col.a;
				}

				// col.a *= 0.75;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}


			

			ENDCG
		}
	}
}


// Method A: (Crude but workable)
				
//if(col.a == 1.)
//if( col.a == .9)
//if(col.a <= .995  && col.a >= .9)
//	col.a = 0.;
//else if(col.a == 0.)
//	col.a = 1.;

// Method B:
/*
if(col.a < 0.5)
	col.a += 0.5;
else if(col.a > 0.5)
	col.a -= 0.5;
else if(col.a == 0.)
	col.a = 1.;
else if(col.a == 1.)
	col.a = 0.;
*/