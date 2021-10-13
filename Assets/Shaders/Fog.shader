Shader "Unlit/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("NoiseTex",2D)="white"{}
        //Fog starting height
        _FogStartHeight("FogStartHeight",float)=0.0
        //Fog end height
        _FogEndHeight("FogEndHeight",float)=1.0
        //Fog x displacement velocity
        _FogXSpeed("FogXSpeed",float)=0.1
        //Fog y displacement velocity
        _FogYSpeed("FogYSpeed",float)=0.1
        _R ("R", float) = 0.5
        _G ("G", float) = 0.5
        _B ("B", float) = 0.5
        _Color ("Color",Color) = (1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            sampler2D _CameraDepthTexture;
            float _FogStartHeight;
            float _FogEndHeight;
            float _FogXSpeed;
            float _FogYSpeed;
            //Four angle vectors of the camera near the cut plane are obtained
            float4x4 _FrustumCornorsRay;

            struct a2v
            {
                float4 vertex:POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f
            {
                float4 pos:SV_POSITION;
                float2 uv:TEXCOORD0;
                float4 interpolatedRay:TEXCOORD1;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                int index = 0;
                if (v.uv.x < 0.5 && v.uv.y < 0.5)
                {
                    index = 0;
                }
                else if (v.uv.x > 0.5 && v.uv.y < 0.5)
                {
                    index = 1;
                }
                else if (v.uv.x > 0.5 && v.uv.y > 0.5)
                {
                    index = 2;
                }
                else
                {
                    index = 3;
                }
                //Arrange four angles of UV to four angle vectors
                o.interpolatedRay = _FrustumCornorsRay[index];
                return o;
            }

            float4 frag(v2f i):SV_Target
            {
                //Linear depth in observation space
                float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
                //Pixel world coordinate = world space camera position + pixel relative camera distance
                float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.interpolatedRay.xyz;
                float speedX = _Time.y * _FogXSpeed;
                float speedY = _Time.y * _FogYSpeed;
                float noise = tex2D(_NoiseTex, i.uv + float2(speedX, speedY));
                //Let the noise map close to black, black and white gap should not be too big
                noise = pow(noise, 0.5);
                //Fog concentration = world height / specified range
                float fogDensity = (_FogEndHeight - worldPos.y) / (_FogEndHeight - _FogStartHeight);
                fogDensity = smoothstep(0.0, 1.0, fogDensity * noise);
                float4 color = tex2D(_MainTex, i.uv);
                //Blends the scene and fog color based on the fog density
                color.rgb = lerp(color.rgb, float3(0.4158508, 0.8073506, 0.990566), fogDensity);
                return color;
            }
            ENDCG
        }
    }
}