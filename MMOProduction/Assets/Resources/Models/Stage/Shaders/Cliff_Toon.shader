//
// Cliff_Toon.shader
// 

Shader "Custom/Stage/Cliff_Toon"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalMap("Normal map Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _CoverageTex("CoverageTex", 2D) = "white" {}

        _Color("Main Color", Color) = (1, 1, 1, 1)
        _ShadeColor("Shade Color", Color) = (1, 1, 1, 1)

        _Brightness("Brightness", Range(0, 1)) = 0.5
        _ToonStrength("Toon Strength", Range(0, 1)) = 0.5

        _BlendRatio("Blend Ratio", Range(0, 1)) = 1
        _ShadowThreshold("Shade Shewshold", Range(0, 1)) = 0.5

        [Toggle(_BAMP_MAPPING)] _BampMapping("Bamp Mapping", Float) = 1
    }
    SubShader
    {
        // バンプマッピング用
        CGINCLUDE
        #pragma shader_feature _BAMP_MAPPING

        sampler2D _NormalMap;

        // 接空間変換行列を取得
        float4x4 invTangentMatrix(float3 tan, float3 bin, float3 nor)
        {
            float4x4 mat = float4x4(
                float4(tan, 0),
                float4(bin, 0),
                float4(nor, 0),
                float4(0, 0, 0, 1)
                );
            return transpose(mat);
        }
        ENDCG

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
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 lightDir : TEXCOORD3;
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
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

#ifdef _BAMP_MAPPING
                o.normal = v.normal;

                // ローカル空間上での接ベクトルを求める
                half3 n = normalize(v.normal);
                half3 t = v.tangent;
                half3 b = cross(n, t);

                half3 localLight = mul(unity_WorldToObject, _WorldSpaceLightPos0);
                o.lightDir = mul(localLight, invTangentMatrix(t, b, n));
#else
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.lightDir = WorldSpaceLightDir(v.vertex);
#endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half3 lightDir = normalize(i.lightDir);
                half3 worldNormal = normalize(i.worldNormal);

                //fixed4 col = tex2D(_MainTex, i.uv) * _Color * 0.5 + _ToonStrength;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // テクスチャブレンド
                half3 up = half3(0, 1, 0);
                half dotNU = max(0, dot(i.normal, up));
                fixed4 coverage = tex2D(_CoverageTex, i.uv * 13);
                //coverage.rgb += 0.4;
                col = lerp(col, coverage, dotNU);

                // バンプマッピング
                fixed4 normal = fixed4(UnpackNormal(tex2D(_NormalMap, i.uv)), 1);
                half diff = max(0, dot(normal, lightDir)) * 0.5 + 1;
                col *= diff;

                // トゥーン？ライティング
                //diff = max(0, dot(worldNormal, lightDir));
                fixed4 ramp = tex2D(_RampTex, fixed2(diff, 0.5));
                col *= ramp + _Brightness;

                //diff = max(0, dot(worldNormal, lightDir));
                //ramp = tex2D(_RampTex, fixed2(diff, 0.5));
                //col *= ramp + _Brightness;

                return col;

/*
                fixed4 ramp = 1;
                half dotNL = dot(normal, lightDir);
#ifdef _BAMP_MAPPING
                // バンプマッピングによるトゥーンライティング
                fixed4 nMap = fixed4(UnpackNormal(tex2D(_NormalMap, uv)), 1);
                half diff = max(0, dot(nMap, lightDir));
                ramp = fixed4(tex2D(_RampTex, fixed2(diff, 0.5)).rgb, 1);
                col *= diff * _Color + _Brightness;
#else
                // 通常のトゥーンライティング
                ramp = fixed4(tex2D(_RampTex, fixed2(dotNL, 0.5)).rgb, 1);
#endif
                col *= saturate(ramp + _Brightness);

                // ディフューズライティング
                //col *= max(_ShadowThreshold, dotNL);
*/

                return col;
            }
            ENDCG
        }
    }
}
