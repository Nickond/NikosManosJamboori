﻿Shader "Particles/Alpha Blended DT_DC_VertexManip" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _DecalColour("Decal Colour", Color) = (1,1,1,1)
    _DecalEmission("Decal Emission Value", Range(0,1)) = 1
	_MainTex ("Particle Texture", 2D) = "white" {}
    _DecalTex ("Decal Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    
    _VM_Distance("VM Distance", Range (0,1)) = 0.0
    _VM_Speed("VM Speed", Range(0,500)) = 0.0
    _VM_Amount("VM Amount", Range(0,500)) = 0.0
    
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="CutOut" }
	
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	Cull Off 
	Lighting Off 
	ZWrite On


	SubShader {

		Pass 
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
            sampler2D _DecalTex;
           
            fixed4 _DecalColour;
			fixed4 _TintColor;
			float _DecalEmission;
            
            float _VM_Distance;
            float _VM_Speed;
            float _VM_Amount;
            
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(0)
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
               
                o.vertex.y +=  sin( _Time.y * _VM_Speed + v.vertex.z * _VM_Amount) * _VM_Distance;
               
                
                
                //o.vertex.xyz += sin( _Time.y * _VM_Speed + v.vertex.xyz * _VM_Amount) * _VM_Distance;
                
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				
				//fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
                
                fixed4 col = tex2D(_MainTex, i.texcoord) * _TintColor;
                half4 decCol = i.color* tex2D(_DecalTex, i.texcoord) * _DecalColour * _DecalEmission;
                
                col.rgb = lerp(col.rgb, decCol.rgb, decCol.a);
                col.a = i.color.a;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG 
		}
	}	
}
}