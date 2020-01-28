// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//
// ForestParticle.shader
//
// w草w
//

Shader "Custom/Grass"
{
    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
            }

            CGPROGRAM
            #pragma target 5.0

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct Particle
            {
                float3 startPos;
                float3 pos;
                float3 scale;
                float3 vel;
                float life;
            };

            struct VSOut
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            StructuredBuffer<Particle> _Particle;
            uniform sampler2D _MainTex;
            uniform sampler2D _RampTex;
            fixed4 _Color;

            VSOut vert(uint id : SV_VertexID)
            {
                VSOut o;
                o.pos = float4(_Particle[id].pos, 1);
                o.uv = float2(0, 0);
                return o;
            }

            [maxvertexcount(4)]
            void geom(point VSOut input[1], uint id : SV_PrimitiveID, inout TriangleStream<VSOut> stream)
            {
                VSOut output;
                float4 pos = input[0].pos;

                // ビルボード用の行列
                float4x4 billboardMatrix = UNITY_MATRIX_V;
                    billboardMatrix._m03 =
                    billboardMatrix._m13 =
                    billboardMatrix._m23 =
                    billboardMatrix._m33 = 0;

                // Quad1
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        // テクスチャ座標を設定
                        output.uv = float2(i, j);

                        // 頂点座標を設定
                        output.pos = pos + mul(float4((output.uv * 2 - float2(1, 1)) * _Particle[id].scale.x, 0, 1), billboardMatrix);
                        output.pos = UnityObjectToClipPos(output.pos);

                        stream.Append(output);
                    }
                }

                stream.RestartStrip();
            }

            fixed4 frag(VSOut i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if (col.a < 0.3) discard;

                half diff = distance(fixed2(1, 1), i.uv);
                fixed4 ramp = tex2D(_RampTex, fixed2(diff, 0.5));
                col *= ramp + _Color;

                col += distance(fixed2(0.5, 0.5), i.uv);

                return col;
            }
            ENDCG
        }
    }
}
