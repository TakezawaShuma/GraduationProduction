//
// SnowFallEffect.shader
//
// Author : Tama
//

Shader "Custom/Event/MeryChristmas/SnowFallEffect"
{
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VSIn
            {
                uint id : SV_VertexID;
            };

            struct VSOut
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 col : COLOR;
            };

            // パーティクルバッファデータ
            struct Particle
            {
                float3 startPos;
                float3 pos;
                float3 vel;
                float4 col;
                float life;
            };

            sampler2D _MainTex;
            StructuredBuffer<Particle> _Particle;   // 構造化バッファ

            VSOut vert(VSIn i)
            {
                VSOut o;
                o.pos = float4(_Particle[i.id].pos, 1);
                o.uv = float2(0, 0);
                o.col = _Particle[i.id].col;
                return o;
            }

            [maxvertexcount(4)]
            void geom(point VSOut input[1], inout TriangleStream<VSOut> stream)
            {
                VSOut output;

                float4 pos = input[0].pos;
                float4 col = input[0].col;

                // 四角形になるように頂点を計算
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        // ビルボード行列
                        float4x4 billboardMatrix = UNITY_MATRIX_V;
                        billboardMatrix._m03 = 0;
                        billboardMatrix._m13 = 0;
                        billboardMatrix._m23 = 0;
                        billboardMatrix._m33 = 0;

                        // テクスチャ座標
                        float2 uv = float2(i, j);
                        output.uv = uv;

                        // 頂点位置を計算
                        float size = 0.5;
                        output.pos = pos + mul(float4((uv * 2 - float2(1, 1)) * size, 0, 1), billboardMatrix);
                        output.pos = mul(UNITY_MATRIX_VP, output.pos);

                        // 色
                        output.col = col;

                        // ストリームに頂点を追加
                        stream.Append(output);
                    }
                }

                // トライアングルストリップを終了
                stream.RestartStrip();
            }

            fixed4 frag(VSOut i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.col;
                col.rgb = 1;
                return col;
            }
            ENDCG
        }
    }
}
