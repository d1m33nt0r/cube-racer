// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 Shader3"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_MainTex ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(MatCap)]
		[Toggle(TCP2_MATCAP)] _UseMatCap ("Enable MatCap", Float) = 0
		[NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "black" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		[TCP2Separator]
		[TCP2HeaderHelp(Ambient Lighting)]
		[Toggle(TCP2_AMBIENT)] _UseAmbient ("Enable Ambient/Indirect Diffuse", Float) = 0
		//AMBIENT CUBEMAP
		_AmbientCube ("Ambient Cubemap", Cube) = "_Skybox" {}
		_TCP2_AMBIENT_RIGHT ("+X (Right)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_LEFT ("-X (Left)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_TOP ("+Y (Top)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BOTTOM ("-Y (Bottom)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_FRONT ("+Z (Front)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BACK ("-Z (Back)", Color) = (0,0,0,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Vertex Displacement)]
		[Toggle(TCP2_VERTEX_DISPLACEMENT)] _UseVertexDisplacement ("Enable Vertex Displacement", Float) = 0
		_DisplacementTex ("Displacement Texture", 2D) = "black" {}
		 _DisplacementStrength ("Displacement Strength", Range(-1,1)) = 0.01
		[TCP2Separator]
		
		[Toggle(TCP2_TEXTURED_THRESHOLD)] _UseTexturedThreshold ("Enable Textured Threshold", Float) = 0
		_StylizedThreshold ("Stylized Threshold", 2D) = "gray" {}
		[TCP2Separator]
		
		[TCP2ColorNoAlpha] _DiffuseTint ("Diffuse Tint", Color) = (1,0.5,0,1)
		[NoScaleOffset] _DiffuseTintMask ("Diffuse Tint Mask", 2D) = "white" {}
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
		sampler2D _DisplacementTex;
		sampler2D _MainTex;
		sampler2D _StylizedThreshold;
		sampler2D _DiffuseTintMask;
		
		// Shader Properties
		float4 _DisplacementTex_ST;
		float _DisplacementStrength;
		float4 _MainTex_ST;
		fixed4 _Color;
		fixed4 _MatCapColor;
		float4 _StylizedThreshold_ST;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		fixed4 _DiffuseTint;

		sampler2D _MatCapTex;
		samplerCUBE _AmbientCube;
		fixed4 _TCP2_AMBIENT_RIGHT;
		fixed4 _TCP2_AMBIENT_LEFT;
		fixed4 _TCP2_AMBIENT_TOP;
		fixed4 _TCP2_AMBIENT_BOTTOM;
		fixed4 _TCP2_AMBIENT_FRONT;
		fixed4 _TCP2_AMBIENT_BACK;

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

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha noambient noforwardadd nolightmap nofog nolppv
		#pragma target 3.0

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature TCP2_VERTEX_DISPLACEMENT
		#pragma shader_feature TCP2_AMBIENT
		#pragma shader_feature TCP2_MATCAP
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
			half3 worldNormal; INTERNAL_DATA
			half2 matcap;
			float2 texcoord0;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0.xy = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			// Shader Properties Sampling
			float3 __vertexDisplacement = ( v.normal.xyz * tex2Dlod(_DisplacementTex, float4(output.texcoord0.xy * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw, 0, 0)).rgb * _DisplacementStrength );

			#if defined(TCP2_VERTEX_DISPLACEMENT)
			v.vertex.xyz += __vertexDisplacement;
			#endif
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);

			#if defined(TCP2_MATCAP)
			//MatCap
			float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
			worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
			float3 perspectiveOffset = (screenPos.xyz / screenPos.w) - 0.5;
			worldNorm.xy -= (perspectiveOffset.xy * perspectiveOffset.z) * 0.5;
			output.matcap = worldNorm.xy * 0.5 + 0.5;
			#endif

		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 worldNormal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;

			Input input;
			
			// Shader Properties
			float __stylizedThreshold;
			float __stylizedThresholdScale;
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float3 __diffuseTint;
			float3 __diffuseTintMask;
			float __occlusion;
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
			float3 __matcapColor = ( _MatCapColor.rgb );
			output.__stylizedThreshold = ( tex2D(_StylizedThreshold, input.texcoord0.xy * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
			output.__stylizedThresholdScale = ( 1.0 );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__diffuseTint = ( _DiffuseTint.rgb );
			output.__diffuseTintMask = ( tex2D(_DiffuseTintMask, input.texcoord0.xy).rgb );
			output.__occlusion = ( __albedo.a );
			output.__ambientIntensity = ( 1.0 );

			output.input = input;

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
			
			//MatCap
			#if defined(TCP2_MATCAP)
			fixed3 matcap = tex2D(_MatCapTex, input.matcap).rgb * __matcapColor;
			output.Emission += matcap;
			#endif
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
			
			// Wrapped Lighting
			ndl = ndl * 0.5 + 0.5;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);

			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			// Diffuse Tint
			half3 diffuseTint = saturate(surface.__diffuseTint + ndl);
			ramp = lerp(ramp, ramp * diffuseTint, surface.__diffuseTintMask);
			
			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = surface.__occlusion;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			#if defined(TCP2_AMBIENT)
				half3 ambient = gi.indirect.diffuse;
				
				//Ambient Cubemap
				ambient.rgb += texCUBE(_AmbientCube, normal);
				
				//Directional Ambient
				ambient.rgb += DirAmbient(normal);
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif
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

/* TCP_DATA u config(unity:"2019.3.10f1";ver:"2.7.4";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","WRAPPED_LIGHTING_HALF","MATCAP","MATCAP_SHADER_FEATURE","MATCAP_PERSPECTIVE_CORRECTION","MATCAP_ADD","DIRAMBIENT","CUBE_AMBIENT","OCCLUSION","AMBIENT_SHADER_FEATURE","VERTEX_DISPLACEMENT","VERTEX_DISP_SHADER_FEATURE","TEXTURED_THRESHOLD","TT_SHADER_FEATURE","DIFFUSE_TINT","DIFFUSE_TINT_MASK"];flags:list["noambient","noforwardadd"];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 4bf981bc5d6a5fd17b31abc282282e9b */
