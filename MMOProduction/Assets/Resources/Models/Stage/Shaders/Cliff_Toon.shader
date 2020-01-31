//
// Cliff_Toon.shader
// 

Shader "Custom/Stage/Cliff_Toon"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalMap("Normal map Texture", 2D) = "white" {}
        _HeightMap("Height map Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _CoverageTex("CoverageTex", 2D) = "white" {}

        _Color("Main Color", Color) = (1, 1, 1, 1)
        _ShadeColor("Shade Color", Color) = (1, 1, 1, 1)
        _ShadowColor("Shadow Color", Color) = (1, 1, 1, 1)

        _Brightness("Brightness", Range(0, 1)) = 0.5
        _ToonStrength("Toon Strength", Range(0, 1)) = 0.5

        _BlendRatio("Blend Ratio", Range(0, 1)) = 1
        _ShadowThreshold("Shade Shewshold", Range(0, 1)) = 0.5
        _HeightFactor("Height Factor", Range(0.0, 0.1)) = 0.02

        [Toggle(_BAMP_MAPPING)] _BampMapping("Bamp Mapping", Float) = 1
    }
    SubShader
    {

        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #pragma shader_feature _BAMP_MAPPING

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "Assets/Ex/CGInclude/SimpleMath.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 lightDir : TEXCOORD3;
                float3 viewDir : TEXCOORD4;
                SHADOW_COORDS(5)
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _HeightMap;
            sampler2D _RampTex;
            sampler2D _CoverageTex;
            fixed4 _Color;
            fixed4 _ShadeColor;
            fixed4 _ShadowColor;
            fixed _Brightness;
            fixed _ToonStrength;
            fixed _BlendRatio;
            fixed _ShadowThreshold;
            fixed _HeightFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

#ifdef _BAMP_MAPPING
                o.normal = v.normal;

                // ローカル空間上での接ベクトルを求める
                TANGENT_SPACE_ROTATION;
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
#else
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.lightDir = WorldSpaceLightDir(v.vertex);
                o.viewDir = WorldSpaceViewDir(v.vertex);
#endif

                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half3 lightDir = normalize(i.lightDir);
                half3 viewDir = normalize(i.viewDir);
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
                fixed height = tex2D(_HeightMap, i.uv);
                height = toGray(height);
                i.uv += viewDir.xy * height * _HeightFactor;
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

                fixed3 shadow = half3(min((SHADOW_ATTENUATION(i) + _ShadowColor.rgb), 1));
                col *= fixed4(shadow, 1);

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

        UsePass "Custom/Effect/SimpleShadow/ShadowCaster"
    }
}
