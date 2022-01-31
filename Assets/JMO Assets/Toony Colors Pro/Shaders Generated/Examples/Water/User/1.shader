// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Toony Colors Pro 2/User/1"
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
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularSmoothness ("Smoothness", Float) = 0.2
		_AnisotropicSpread ("Anisotropic Spread", Range(0,2)) = 1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		//Rim Direction
		_RimDirVert ("Rim Direction", Vector) = (0,0,1,1)
		[TCP2Separator]

		[TCP2HeaderHelp(Reflections)]
		[Toggle(TCP2_REFLECTIONS)] _UseReflections ("Enable Reflections", Float) = 0
		[TCP2ColorNoAlpha] _ReflectionColor ("Color", Color) = (1,1,1,1)
		[HideInInspector] _ReflectionTex ("Planar Reflection RenderTexture", 2D) = "white" {}
		_FresnelMin ("Fresnel Min", Range(0,2)) = 0
		_FresnelMax ("Fresnel Max", Range(0,2)) = 1.5
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
		float4 _RimDirVert;
		float _RimMinVert;
		float _RimMaxVert;
		float4 _MainTex_ST;
		fixed4 _Color;
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
		float _AnisotropicSpread;
		float _SpecularSmoothness;
		fixed4 _SpecularColor;
		fixed4 _RimColor;
		float _FresnelMin;
		float _FresnelMax;
		fixed4 _ReflectionColor;

		sampler2D _ReflectionTex;
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

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha noforwardadd novertexlights halfasview nolightmap nolppv
		#pragma target 2.5

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature TCP2_SPECULAR
		#pragma shader_feature TCP2_RIM_LIGHTING
		#pragma shader_feature TCP2_REFLECTIONS
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
			half4 tangent : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			half3 tangent;
			half3 worldNormal; INTERNAL_DATA
			float4 screenPosition;
			half rim;
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
			float3 __rimDirVert = ( _RimDirVert.xyz );
			float __rimMinVert = ( _RimMinVert );
			float __rimMaxVert = ( _RimMaxVert );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;
			half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

			output.tangent = mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0)).xyz;
			#if defined(TCP2_RIM_LIGHTING)
			half3 rViewDir = viewDir;
			half3 rimDir = __rimDirVert;
			rViewDir = normalize(UNITY_MATRIX_V[0].xyz * rimDir.x + UNITY_MATRIX_V[1].xyz * rimDir.y + UNITY_MATRIX_V[2].xyz * rimDir.z);
			half rim = 1.0f - saturate(dot(rViewDir, v.normal.xyz));
			rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
			output.rim = rim;
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
			half ndv;
			half ndvRaw;

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
			float __anisotropicSpread;
			float __specularSmoothness;
			float3 __specularColor;
			float3 __rimColor;
			float __rimStrength;
			float __fresnelMin;
			float __fresnelMax;
			float3 __reflectionColor;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Shader Properties Sampling
			float4 __albedo = ( tex2D(_MainTex, input.texcoord0.xy).rgba );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
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
			output.__anisotropicSpread = ( _AnisotropicSpread );
			output.__specularSmoothness = ( _SpecularSmoothness );
			output.__specularColor = ( _SpecularColor.rgb );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );
			output.__fresnelMin = ( _FresnelMin );
			output.__fresnelMax = ( _FresnelMax );
			output.__reflectionColor = ( _ReflectionColor.rgb );

			output.input = input;

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			half ndv = abs(dot(input.viewDir, normalize(output.Normal.xyz)));
			half ndvRaw = ndv;
			output.ndv = ndv;
			output.ndvRaw = ndvRaw;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{
			half ndv = surface.ndv;
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
			half lightWrap = surface.__lightWrapFactor;
			ndl = (ndl + lightWrap) / (1 + lightWrap);
			
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

			#if defined(TCP2_SPECULAR)
			//Anisotropic Specular
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (normal, h));
			half3 binorm = cross(normal, surface.input.tangent);
			float aX = dot(h, surface.input.tangent) / surface.__anisotropicSpread;
			float aY = dot(h, binorm) / surface.__specularSmoothness;
			float specAniso = sqrt(max(0.0, ndl / surface.ndvRaw)) * exp(-2.0 * (aX * aX + aY * aY) / (1.0 + ndh));
			float spec = specAniso;
			spec = saturate(spec);
			spec *= atten;
			
			//Apply specular
			color.rgb += spec * lightColor.rgb * surface.__specularColor;
			#endif
			// Rim Lighting
			#if defined(TCP2_RIM_LIGHTING)
			#if !defined(UNITY_PASS_FORWARDADD)
			half rim = surface.input.rim;
			rim = ( rim );
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			color.rgb += rim * rimColor * rimStrength;
			#endif
			#endif

			half3 reflections = half3(0, 0, 0);
			#if defined(TCP2_REFLECTIONS)
			reflections.rgb += tex2D(_ReflectionTex, surface.input.screenPosition.xy / surface.input.screenPosition.w).rgb;
			#endif
			half fresnelMin = surface.__fresnelMin;
			half fresnelMax = surface.__fresnelMax;
			half fresnelTerm = smoothstep(fresnelMin, fresnelMax, 1 - surface.ndvRaw);
			reflections *= fresnelTerm;
			reflections *= surface.__reflectionColor;
			color.rgb += reflections;

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

/* TCP_DATA u config(unity:"2020.3.12f1";ver:"2.7.4";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","SS_SHADER_FEATURE","SUBSURFACE_AMB_COLOR","TT_SHADER_FEATURE","RIM_SHADER_FEATURE","RAMP_BANDS","SKETCH_AMBIENT","SKETCH_SHADER_FEATURE","VERTICAL_FOG_ALPHA","VERTICAL_FOG_COLOR","ENABLE_FOG","SPECULAR_SHADER_FEATURE","MATCAP_SHADER_FEATURE","MATCAP_PERSPECTIVE_CORRECTION","RIM_DIR","RIM_VERTEX","RIM_DIR_PERSP_CORRECTION","DIRAMBIENT","UNITY_2020_1","SKETCH_PROGRESSIVE_SMOOTH","SHADOW_HSV","TEXTURED_THRESHOLD","PLANAR_REFLECTION","REFLECTION_FRESNEL","REFLECTION_SHADER_FEATURE","RIM","SPECULAR_ANISOTROPIC","SPECULAR","WRAPPED_LIGHTING_CUSTOM"];flags:list["noforwardadd","novertexlights","halfasview"];flags_extra:dict[pragma_gpu_instancing=list[]];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="2.5",RIM_LABEL="Rim Lighting"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 445fa7c2663d9491ac9e3fc70c11fd5b */
