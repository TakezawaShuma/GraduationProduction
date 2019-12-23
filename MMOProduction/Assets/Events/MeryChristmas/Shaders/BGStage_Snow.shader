//
// BGStage_Snow.shader
//
// ログイン時の背景ステージ用シェーダー（クリスマスver）
//

Shader "Custom/Event/MeryChristmas/BGStage_Snow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex("Sub Texture", 2D) = "white"{}
        _Color("Main Color", Color) = (1, 1, 1, 1)
        _Reflectance("Reflection", Float) = 0.5
        _BlendRatio("Blend Ratio", Range(0, 1)) = 0.5

        [Toggle(BLEND_FLAG)] _BlendFlag("Blend Flag", Float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #pragma shader_feature BLEND_FLAG

            #include "UnityCG.cginc"
            #include "Assets/Ex/CGInclude/Noise.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float4 surfaceLightColor : COLOR;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            sampler2D _SubTex;
            sampler2D _Splat;
            float4 _Color;
            float _BlendRatio;

            uniform float _Reflectance;
            uniform float4 _LightColor0;	//_LightColor0はuniformで定義してあげないとエラーになる

            float4 diffuse(float3 lightDir, float3 normal)
            {
                lightDir = normalize(lightDir);
                normal = normalize(normal);

                float diff = saturate(dot(lightDir, normal));

                return float4(diff, diff, diff, 1);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.surfaceLightColor = _LightColor0 * _Reflectance * max(dot(_WorldSpaceLightPos0, o.normal), 0);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // テクスチャブレンド用
                float4 c0 = tex2D(_MainTex, i.uv * 100);
                float4 c1 = tex2D(_SubTex, i.uv);
                float c = fBm(i.uv * 10);
                float4 c2 = float4(c, c, c, 1);

                // ブレンド状態を調整する
                fixed4 col;
#ifdef BLEND_FLAG
                col = lerp(c0, c1, c2);
#else
                col = lerp(c0, c1, _BlendRatio);
#endif

                // 拡散ライティング
                col *= (i.surfaceLightColor + 0.5)* _Color;

                // 積雪
                float snow = dot(i.normal, float3(0, 1, 0));
                col.rgb += clamp(snow, 0, 1);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
