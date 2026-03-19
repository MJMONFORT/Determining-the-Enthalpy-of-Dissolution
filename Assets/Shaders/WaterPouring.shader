Shader "Custom/URP_Lit_Liquid_Reflection"
{
    Properties
    {
        [Header(Surface)]
        _BaseColor("Base Color", Color) = (0,0.5,1,0.5)
        _Metallic("Metallic", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0,1)) = 0.95

        [Header(Liquid Behavior)]
        _FillLevel("Fill Level", Float) = 0
        _WaveStrength("Wave Strength", Float) = 0.03
        _WaveFrequency("Wave Frequency", Float) = 3
        _WaveSpeed("Wave Speed", Float) = 2
        _TiltAmount("Tilt Amount", Float) = 0

        [Header(Render Face Mode)]
        [Enum(Off,0, Front,1, Back,2)]
        _Cull("Render Faces", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "UniversalMaterialType"="Lit"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull [_Cull]

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Metallic;
                float _Smoothness;
                float _FillLevel;
                float _WaveStrength;
                float _WaveFrequency;
                float _WaveSpeed;
                float _TiltAmount;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);

                float wave =
                    sin(_Time.y * _WaveSpeed +
                        worldPos.x * _WaveFrequency +
                        worldPos.z * _WaveFrequency)
                    * _WaveStrength;

                float tilt = worldPos.x * _TiltAmount;

                worldPos.y += wave + tilt;

                OUT.positionWS = worldPos;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(worldPos);
                OUT.positionCS = TransformWorldToHClip(worldPos);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                if (_FillLevel < IN.positionWS.y)
                    discard;

                SurfaceData surfaceData;
                surfaceData.albedo = _BaseColor.rgb;
                surfaceData.alpha = _BaseColor.a;
                surfaceData.metallic = _Metallic;
                surfaceData.specular = 0;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = 0;
                surfaceData.occlusion = 1;
                surfaceData.emission = 0;
                surfaceData.clearCoatMask = 0;
                surfaceData.clearCoatSmoothness = 0;

                InputData inputData;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = normalize(IN.normalWS);
                inputData.viewDirectionWS = normalize(IN.viewDirWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                inputData.fogCoord = 0;
                inputData.vertexLighting = 0;
                inputData.bakedGI = SampleSH(inputData.normalWS);
                inputData.normalizedScreenSpaceUV = 0;
                inputData.shadowMask = 1;

                half4 color = UniversalFragmentPBR(inputData, surfaceData);

                return color;
            }

            ENDHLSL
        }
    }
}
