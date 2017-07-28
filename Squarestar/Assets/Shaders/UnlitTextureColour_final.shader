 Shader "Unlit/Emissive_Pattern"
 {
     Properties
     {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _EmissionMap ("Emission Texture", 2D) = "white" {}
         _Blend ("Blend Amount", Range (0, 1) ) = 0 
         _Colour ("Colour", Color) = (1,1,1,1)
         _EmissionColour("Emission", Color) = (0,0,0)
     }
     SubShader
     {
         Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
         LOD 200
         
         //ZWrite Off
         //Blend SrcAlpha OneMinusSrcAlpha
         //Blend One One
         //Blend SrcAlpha OneMinusSrcAlpha
 
         
         CGPROGRAM
         #pragma surface surf NoLighting  noambient
 
         sampler2D _MainTex;
         sampler2D _EmissionMap;
         fixed4 _Colour;
         fixed4 _EmissionColour;
         float _Blend;
 
         struct Input
         {
             float2 uv_MainTex;
         };
            
         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
         {
             fixed4 c;
             c.rgb = s.Albedo * _Colour.rgb;
             c.a = s.Albedo * _Colour.a;
             return c;
         }
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex) * _Colour;
            fixed4 emissionCol = tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColour;                           
            
            // fixed4 mainOutput = mainCol.rgba * (1.0 - emissionCol.a);
            // fixed4 blendOutput = emissionCol.rgba * emissionCol.a;         
            
			// mainOutput.rgb = lerp(mainOutput.rgb, blendOutput.rgb, blendOutput.a);
			
			mainCol.rgb = lerp(mainCol.rgb, emissionCol.rgb, emissionCol.a);

			o.Albedo = mainCol.rgb;
			o.Alpha = mainCol.a;

            // o.Albedo = mainOutput.rgb; // + blendOutput.rgb;
            // o.Alpha = mainOutput.a; // + blendOutput.a;
        }
         ENDCG
     } 
     FallBack "Diffuse"
 }