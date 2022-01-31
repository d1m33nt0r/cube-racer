// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Cubracer/Environvent/Road"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[HideInInspector] __BeginGroup_ShadowHSV ("Shadow HSV", Float) = 0
		_Shadow_HSV_H ("Hue", Range(-180,180)) = 0
		_Shadow_HSV_S ("Saturation", Range(-1,1)) = 0
		_Shadow_HSV_V ("Value", Range(-1,1)) = 0
		[HideInInspector] __EndGroup ("Shadow HSV", Float) = 0
		_MainTex ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		_BandsSmoothing ("Bands Smoothing", Range(0.001,1)) = 0.1
		_LightWrapFactor ("Light Wrap Factor", Range(0,2)) = 0.5
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[TCP2ColorNoAlpha] [HDR] _Emission ("Emission Color", Color) = (0,0,0,1)
		[TCP2Separator]
		[TCP2HeaderHelp(Ambient Lighting)]
		_TCP2_AMBIENT_RIGHT ("+X (Right)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_LEFT ("-X (Left)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_TOP ("+Y (Top)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BOTTOM ("-Y (Bottom)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_FRONT ("+Z (Front)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BACK ("-Z (Back)", Color) = (0,0,0,1)
		[TCP2Separator]
		
		[Toggle(TCP2_TEXTURED_THRESHOLD)] _UseTexturedThreshold ("Enable Textured Threshold", Float) = 0
		_StylizedThreshold ("Stylized Threshold", 2D) = "gray" {}
		[TCP2Separator]
		
		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Shader Properties
		sampler2D _MainTex;
		sampler2D _StylizedThreshold;
		
		// Shader Properties
		float4 _MainTex_ST;
		fixed4 _Color;
		half4 _Emission;
		float4 _StylizedThreshold_ST;
		float _LightWrapFactor;
		float _RampThreshold;
		float _RampSmoothing;
		float _BandsCount;
		float _BandsSmoothing;
		float _Shadow_HSV_H;
		float _Shadow_HSV_S;
		float _Shadow_HSV_V;
		fixed4 _HColor;
		fixed4 _SColor;

		fixed4 _TCP2_AMBIENT_RIGHT;
		fixed4 _TCP2_AMBIENT_LEFT;
		fixed4 _TCP2_AMBIENT_TOP;
		fixed4 _TCP2_AMBIENT_BOTTOM;
		fixed4 _TCP2_AMBIENT_FRONT;
		fixed4 _TCP2_AMBIENT_BACK;

		//--------------------------------
		// HSV HELPERS
		// source: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
		
		float3 rgb2hsv(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
		
			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}
		
		float3 hsv2rgb(float3 c)
		{
			c.g = max(c.g, 0.0); //make sure that saturation value is positive
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}
		
		float3 ApplyHSV_3(float3 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return hsv2rgb(hsv);
		}
		float3 ApplyHSV_3(float color, float h, float s, float v) { return ApplyHSV_3(color.xxx, h, s ,v); }
		
		float4 ApplyHSV_4(float4 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return float4(hsv2rgb(hsv), color.a);
		}
		float4 ApplyHSV_4(float color, float h, float s, float v) { return ApplyHSV_4(color.xxxx, h, s, v); }
		
		half3 DirAmbient (half3 normal)
		{
			fixed3 retColor =
				saturate( normal.x * _TCP2_AMBIENT_RIGHT) +
				saturate(-normal.x * _TCP2_AMBIENT_LEFT) +
				saturate( normal.y * _TCP2_AMBIENT_TOP) +
				saturate(-normal.y * _TCP2_AMBIENT_BOTTOM) +
				saturate( normal.z * _TCP2_AMBIENT_FRONT) +
				saturate(-normal.z * _TCP2_AMBIENT_BACK);
			return retColor * 2.0;
		}

		ENDCG

		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha noforwardadd nolightmap nolppv
		#pragma target 2.5

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature TCP2_TEXTURED_THRESHOLD

		//================================================================
		// STRUCTS

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			float2 texcoord0;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0.xy = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;

		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;

			Input input;
			
			// Shader Properties
			float __stylizedThreshold;
			float __stylizedThresholdScale;
			float __lightWrapFactor;
			float __rampThreshold;
			float __rampSmoothing;
			float __bandsCount;
			float __bandsSmoothing;
			float __shadowHue;
			float __shadowSaturation;
			float __shadowValue;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Shader Properties Sampling
			float4 __albedo = ( tex2D(_MainTex, input.texcoord0.xy).rgba );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			float3 __emission = ( _Emission.rgb );
			output.__stylizedThreshold = ( tex2D(_StylizedThreshold, input.texcoord0.xy * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
			output.__stylizedThresholdScale = ( 1.0 );
			output.__lightWrapFactor = ( _LightWrapFactor );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__bandsCount = ( _BandsCount );
			output.__bandsSmoothing = ( _BandsSmoothing );
			output.__shadowHue = ( _Shadow_HSV_H );
			output.__shadowSaturation = ( _Shadow_HSV_S );
			output.__shadowValue = ( _Shadow_HSV_V );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );

			output.input = input;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
			output.Emission += __emission;
		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, UnityGI gi)
		{
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				//extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			#if defined(TCP2_TEXTURED_THRESHOLD)
			float stylizedThreshold = surface.__stylizedThreshold;
			stylizedThreshold -= 0.5;
			stylizedThreshold *= surface.__stylizedThresholdScale;
			ndl += stylizedThreshold;
			#endif
			half3 ramp;
			#if defined(UNITY_PASS_FORWARDBASE)
			
			// Wrapped Lighting
			half lightWrap = surface.__lightWrapFactor;
			ndl = (ndl + lightWrap) / (1 + lightWrap);
			#endif
			
			#define		RAMP_THRESHOLD		surface.__rampThreshold
			#define		RAMP_SMOOTH			surface.__rampSmoothing
			#define		RAMP_BANDS			surface.__bandsCount
			#define		RAMP_BANDS_SMOOTH	surface.__bandsSmoothing
			ndl = saturate(ndl);
			half bandsNdl = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);
			half bandsSmooth = RAMP_BANDS_SMOOTH * 0.5;
			ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * RAMP_BANDS)) + floor(bandsNdl * RAMP_BANDS)) / RAMP_BANDS).xxx;

			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;
			
			//Shadow HSV
			float3 albedoShadowHSV = ApplyHSV_3(surface.Albedo, surface.__shadowHue, surface.__shadowSaturation, surface.__shadowValue);
			surface.Albedo = lerp(albedoShadowHSV, surface.Albedo, ramp);

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				
				//Directional Ambient
				ambient.rgb += DirAmbient(normal);
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			//GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.3.12f1";ver:"2.7.4";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2020_1","WRAPPED_LIGHTING_CUSTOM","ENABLE_FOG","WRAPPED_LIGHTING_MAIN_LIGHT","RAMP_BANDS","SHADOW_HSV","DIRAMBIENT","TEXTURED_THRESHOLD","TT_SHADER_FEATURE","EMISSION"];flags:list["noforwardadd"];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="2.5"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 9b33b73431861695ca26a0bbb89c16e8 */
