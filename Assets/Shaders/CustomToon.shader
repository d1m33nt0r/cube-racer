// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/Outlined Diffuse" {
     Properties {
         _MainTex ("Base (RGB)", 2D) = "white" { }
         _Ramp ("Shading Ramp", 2D) = "gray" {}
     }
 
     SubShader {
     
         Pass {
             Name "OUTLINE"
             Tags { "LightMode" = "Always" }
             Cull Front
             ZWrite Off
             ZTest Always
             //Offset 15,15
             
             CGINCLUDE
             #include "UnityCG.cginc"
 
             struct appdata {
                 half4 vertex : POSITION;
                 half3 normal : NORMAL;
             };
 
             struct v2f {
                 half4 pos : POSITION;
             };
 
             v2f vert(appdata v) {
                 // just make a copy of incoming vertex data but scaled according to normal direction
                 v2f o;
                 o.pos = UnityObjectToClipPos(v.vertex);
 
                 half3 norm  = mul ((half3x3)UNITY_MATRIX_IT_MV, v.normal);
                 half2 offset = TransformViewToProjection(norm.xy);
                 o.pos.xy += offset * o.pos.z * 0.002;   
                 
                 return o;
             }
 
             float4 frag(v2f i) : COLOR {
                 return float4(0.0, 0.0, 0.0, 1.0);
             }
 
             ENDCG
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag            
             ENDCG
         } // Name "OUTLINE"
 // Cel shading based on one directional light with the use of a surface shader
 //        Tags {"LightMode" = "ForwardBase" }
 //CGPROGRAM
 //        #pragma surface surf Ramp noambient noforwardadd approxview
 //
 //        sampler2D_half _MainTex;
 //          sampler2D_half _Ramp;
 //          
 //        half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
 //          half NdotL = dot (s.Normal, lightDir);
 //          half diff = NdotL * 0.5 + 0.5;
 //          half3 ramp = tex2D (_Ramp, half2(diff)).rgb;
 //          half4 c;          
 //          c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
 //          c.a = s.Alpha;
 //          return c;
 //          }
 //
 //        struct Input {
 //            half2 uv_MainTex;
 //        };
 //
 //        void surf (Input IN, inout SurfaceOutput o) {
 //            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
 //            o.Albedo = c.rgb;
 //            o.Alpha = c.a;
 //        }
 //
 //ENDCG
 
         Pass {
             Name "VERTEX_LIGHTING"
             Tags { "LightMode" = "ForwardBase" }
             Cull Back
 
             CGPROGRAM
             
             #pragma vertex vert_pass_2
             #pragma fragment frag_pass_2                        
             
             #include "UnityCG.cginc"
             #include "Lighting.cginc"
         
             sampler2D_half _MainTex;
             sampler2D_half _Ramp;
 
              struct vertexInput {
                 half4 vertex : POSITION;
                 half2 texcoord : TEXCOORD0;
                 half3 normal : NORMAL;
              };
              
              struct vertexOutput {
                 half4 pos : SV_POSITION;
                 half2 uv : TEXCOORD0;
                 half4 light : TEXCOORD1;
              };
  
          vertexOutput vert_pass_2(vertexInput v) 
          {
             vertexOutput output;
              output.pos = UnityObjectToClipPos(v.vertex);
              output.uv = v.texcoord;             
 
 // 1st approach
 
 //            half3 worldPos = mul(_Object2World, v.vertex).xyz;
 //            half3 worldN = mul((half3x3)_Object2World, SCALED_NORMAL);
 
 //            half3 vertexL = Shade4PointLights(
 //                unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
 //                unity_LightColor0, unity_LightColor1, unity_LightColor2, unity_LightColor3,
 //                unity_4LightAtten0, worldPos, worldN);
 
 // 2nd approach 
             half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
             half3 worldN = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
               
             half3 vertexL = half3(0.0, 0.0, 0.0);
             for (int i = 0; i < 4; ++i)
             {    
                 half3 lightPos = half3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
  
                 half3 vertexToLightSource = lightPos - worldPos;
                half3 lightDirection = normalize(vertexToLightSource);
                half squaredDistance = dot(vertexToLightSource, vertexToLightSource);
                half attenuation = 1.0 / (1.0 + unity_4LightAtten0[i] * squaredDistance);
                half3 diffuseReflection =  attenuation * half3(unity_LightColor[i]) * 
                     max(0.0, dot(worldN, lightDirection));         
  
                vertexL += diffuseReflection;
             }
 //                
             half3 dir = normalize(half3(_WorldSpaceLightPos0));
             half3 directionL = _LightColor0.rgb * dot(worldN, dir);
                       
             half3 l = vertexL + directionL;
             half intensity = (l.r + l.g + l.b) / 3.0;
             output.light = half4(l, intensity);           
             
             return output;
         }
                   
          half4 frag_pass_2(vertexOutput input) : COLOR
          {
              half4 ambientL = half4(half3(UNITY_LIGHTMODEL_AMBIENT), 1.0) * 2;            
              half4 c = tex2D(_MainTex, input.uv) * (ambientL + input.light * tex2D(_Ramp, half2(input.light.w, input.light.w)));
              return c;
          }
 
             ENDCG
         }            
     }                    
 }