//
// ToonBaseTown.shader
//
// Author: Tama
//

Shader "Custom/Stage/Town/ToonBaseTown"
{
    Properties
    {
        // メイン描画用
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)

        // 地面用
        _GroundTex("Ground Texture", 2D) = "white" {}
        _GrassMap("Grass map Texture", 2D) = "white" {}
        _GroundMask("Ground mask Texture", 2D) = "white" {}

        // 道用
        _RoadTex("Road Texture", 2D) = "white" {}
        _RoadNormalMap("Road Normal Map", 2D) = "white" {}
        _RoadHeightMap("Road Height Map", 2D) = "white" {}
        _RoadMap("Road map Texture", 2D) = "white" {}
        _RoadRampMap("Road Ramp Map", 2D) = "white" {}
        _RoadColor("Road Color", Color) = (1, 1, 1, 1)
        _RoadHeightFactor("Road Height Factor", Range(0, 0.1)) = 0.02
        _RoadBrightness("Road Brightness", Range(0, 1)) = 1
        _ToonFactor("Toon Factor", Range(0, 1)) = 1

        // Toggle
        [Toggle(_BUMP_MAPPING)] _BumpMapping("Bump Mapping", Float) = 1
        [Toggle(_PARALLAX_MAPPING)] _ParallaxMapping("Parallax Mapping", Float) = 1
        [Toggle(_TOON_LIGHTING)] _ToonLighting("Toon Lighting", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _BUMP_MAPPING
            #pragma shader_feature _PARALLAX_MAPPING
            #pragma shader_feature _TOON_LIGHTING

            #include "UnityCG.cginc"

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
                float3 lightDir : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            // 地面
            sampler2D _GroundTex;
            sampler2D _GrassMap;
            sampler2D _GroundMask;

            // 道
            sampler2D _RoadTex;
            sampler2D _RoadNormalMap;
            sampler2D _RoadHeightMap;
            sampler2D _RoadMap;
            sampler2D _RoadRampMap;
            fixed4 _RoadColor;
            fixed _RoadHeightFactor;
            fixed _RoadBrightness;
            fixed _ToonFactor;

            // 地面合成関数
            fixed4 ground(float2 uv)
            {
                fixed4 c0 = tex2D(_GroundTex, uv);
                fixed4 c1 = tex2D(_GrassMap, uv);
                fixed4 c2 = tex2D(_GroundMask, uv);

                return lerp(c0, c1, c2);
            }

            // 道合成関数
            fixed4 road(v2f i)
            {
                half3 lightDir = normalize(i.lightDir);
                half3 viewDir = normalize(i.viewDir);
                float2 uv = i.uv;

                fixed map = tex2D(_RoadMap, uv).r;

                if (map >= 0.99)
                {
                    uv *= 40;

                    // 最終結果用の色
                    float4 col = tex2D(_RoadTex, uv);

                    // 視差マッピング用にuvをずらす
#ifdef _PARALLAX_MAPPING
                    // ハイトマップをサンプリングしてUVをずらす
                    fixed4 height = tex2D(_RoadHeightMap, uv);
                    uv += viewDir.xy * height.r * _RoadHeightFactor;
#else
                    // nothing
#endif

                    // バンプマッピング
                    half3 normal;
#ifdef _BUMP_MAPPING
                    normal = fixed4(UnpackNormal(tex2D(_RoadNormalMap, uv)), 1);                    
#else
                    normal = i.worldNormal;
#endif
                    fixed diff = saturate(dot(normal, lightDir));
                    col.rgb *= diff;

                    // トゥーンライティング
#ifdef _TOON_LIGHTING
                    fixed3 ramp = tex2D(_RoadRampMap, fixed2(diff, 0.5));
                    col.rgb += ramp;
                    col.rgb *= _ToonFactor;
#else
                    // nothing
#endif

                    return col * _RoadColor + _RoadBrightness;
                }

                return fixed4(1, 1, 1, 1);
            }

            // 頂点シェーダ
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                // 接線情報取得
                TANGENT_SPACE_ROTATION;
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));

                return o;
            }

            // ピクセルシェーダ
            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= ground(i.uv * 10).rgb;
                col.rgb *= road(i).rgb;
                return col;
            }
            ENDCG
        }
    }
}
