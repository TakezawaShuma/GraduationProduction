//
// Cliff_Toon.shader
// 

Shader "Custom/Stage/Cliff_Toon"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _CoverageTex("CoverageTex", 2D) = "white" {}

        _Color("Main Color", Color) = (1, 1, 1, 1)
        _ShadeColor("Shade Color", Color) = (1, 1, 1, 1)

        _Brightness("Brightness", Range(0, 1)) = 0.5
        _ToonStrength("Toon Strength", Range(0, 1)) = 0.5

        _BlendRatio("Blend Ratio", Range(0, 1)) = 1
        _ShadowThreshold("Shade Shewshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _RampTex;
            sampler2D _CoverageTex;
            fixed4 _Color;
            fixed4 _ShadeColor;
            fixed _Brightness;
            fixed _ToonStrength;
            fixed _BlendRatio;
            fixed _ShadowThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.lightDir = WorldSpaceLightDir(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half3 normal = normalize(i.normal);
                half3 lightDir = normalize(i.lightDir);
                float2 uv = i.uv;

                fixed4 col = tex2D(_MainTex, uv) * _Color * 0.5 + _ToonStrength;

                // テクスチャブレンド
                half3 up = half3(0, 1, 0);
                half dotNU = max(0, dot(normal, up));
                fixed4 coverage = tex2D(_CoverageTex, uv * 13);
                coverage.rgb += 0.4;

                col = lerp(col, coverage, dotNU);

                // トゥーンライティング
                half dotNL = dot(normal, lightDir);
                fixed4 ramp = fixed4(tex2D(_RampTex, fixed2(dotNL, 0.5)).rgb, 1);

                col *= saturate(ramp + _Brightness);

                // ディフューズライティング
                col *= max(_ShadowThreshold, dotNL);

                return col;
            }
            ENDCG
        }
    }
}
