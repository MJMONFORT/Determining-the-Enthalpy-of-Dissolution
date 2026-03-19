Shader "Custom/HeatConvectionLiquid"
{
    Properties
    {
        _MainColor ("Base Water Color", Color) = (0.2, 0.6, 0.7, 0.4)
        _HotColor ("Hot Color", Color) = (1, 0.3, 0, 0)
        _CoolColor ("Cool Color", Color) = (0, 0.4, 1, 0)

        _NoiseTex ("Flow Noise", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Float) = 0.2
        _FlowScale ("Flow Scale", Float) = 2
        _Intensity ("Heat Intensity", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float4 _MainColor, _HotColor, _CoolColor;
            float _FlowSpeed, _FlowScale, _Intensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 flowUV1 = i.uv * _FlowScale + float2(_Time.y * _FlowSpeed, 0);
                float2 flowUV2 = i.uv * _FlowScale + float2(0, _Time.y * _FlowSpeed * 0.7);

                float noise1 = tex2D(_NoiseTex, flowUV1).r;
                float noise2 = tex2D(_NoiseTex, flowUV2).g;

                float heatPattern = (noise1 + noise2) * 0.5;

                float3 heatColor = lerp(_CoolColor.rgb, _HotColor.rgb, heatPattern * _Intensity);

                float3 finalColor = _MainColor.rgb + heatColor * 0.5;

                return float4(finalColor, _MainColor.a);
            }
            ENDCG
        }
    }
}
