// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//
// Grass.shader
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
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma target 5.0

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct Grass
            {
                float3 startPos;
                float3 pos;
                float3 scale;
                float3 wind;
                int type;
            };

            struct VSOut
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            StructuredBuffer<Grass> _Grass;
            uniform sampler2D _MainTex;
            uniform sampler2D _HeightMap;
            uniform sampler2D _RampTex;
            float4 _Color;
            fixed4 _ShadowColor;
            float3 _Scale;
            float3 _Rotate;
            float3 _Translation;
            float3 _HeightMapPoint;
            float _HeightMapScale;

            float4x4 scale()
            {
                float x = _Scale.x;
                float y = _Scale.y;
                float z = _Scale.z;

                float4x4 mat = float4x4(
                    x, 0, 0, 0,
                    0, y, 0, 0,
                    0, 0, z, 0,
                    0, 0, 0, 1
                    );

                return mat;
            }

            float4x4 rotateY(float angle)
            {
                float pi = 3.1415;

                angle *= pi / 180;

                float4x4 mat = float4x4(
                    cos(angle), 0, -sin(angle), 0,
                             0, 1,           0, 0,
                    sin(angle), 0,  cos(angle), 0,
                             0, 0,           0, 1
                    );

                return mat;
            }

            float4x4 trans()
            {
                float x = _Translation.x;
                float y = _Translation.y;
                float z = _Translation.z;

                float4x4 mat = float4x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    x, y, z, 1
                    );

                return mat;
            }

            VSOut vert(uint id : SV_VertexID)
            {
                VSOut o;
                o.pos = float4(_Grass[id].pos, 1);
                o.uv = float2(0, 0);
                return o;
            }

            [maxvertexcount(4)]
            void geom(point VSOut input[1], uint id : SV_PrimitiveID, inout TriangleStream<VSOut> stream)
            {
                VSOut output;
                float4 pos = input[0].pos;

                // ビルボード用の行列
                float4x4 billboardYMat = float4x4(
                    UNITY_MATRIX_V._m00, UNITY_MATRIX_V._m01, UNITY_MATRIX_V._m02, 0,
                    UNITY_MATRIX_V._m10, UNITY_MATRIX_V._m11, UNITY_MATRIX_V._m12, 0,
                    UNITY_MATRIX_V._m20, UNITY_MATRIX_V._m21, UNITY_MATRIX_V._m22, 0,
                    UNITY_MATRIX_V._m30, UNITY_MATRIX_V._m31, UNITY_MATRIX_V._m32, 1
                    );

                // Quad1
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        // 頂点座標を設定
                        output.pos = float4(float2(i, j + 0.5) - 0.5, 0, 0);
                        output.pos *= float4(_Scale * _Grass[id].scale, 1);
                        output.pos = mul(billboardYMat, output.pos);
                        output.pos += pos;
                        if (j == 1)
                        {
                            output.pos += float4(_Grass[id].wind * sin(_Time * _Grass[id].wind + 0.1) * 0.1, 1);
                        }
                        output.worldPos = mul(unity_ObjectToWorld, output.pos);
                        output.pos = UnityObjectToClipPos(output.pos);

                        // テクスチャ座標を設定
                        float left = i * 0.25 + _Grass[id].type * 0.25;
                        float right = j;
                        output.uv = float2(left, right);

                        TRANSFER_SHADOW(o);

                        stream.Append(output);
                    }
                }

                stream.RestartStrip();
            }

            fixed4 frag(VSOut i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (col.a < 0.3) discard;

                col.rb *= 0;
                col.g += 0.3;

                float3 worldPos = i.worldPos;
                float3 p = _HeightMapPoint;
                float size = _HeightMapScale;
                float com;
                com = step(p.x - size, worldPos.x);
                com = step(worldPos.x, p.x + size) * com;
                com = step(p.y - size, worldPos.z) * com;
                com = step(worldPos.z, p.y + size) * com;

                fixed4 map = tex2D(_HeightMap, (worldPos.xz - p) * 0.5 / size + 0.5);
                map.a = map.a * com;
                float displace = float4(map.rgb * map.a, 1).r;
                if (displace >= 0.9) discard;


                fixed4 ramp = tex2D(_RampTex, i.uv);
                col *= lerp(col, ramp, 0.5);

                //fixed3 shadow = half3(min((SHADOW_ATTENUATION(i) + _ShadowColor.rgb), 1));
                //col *= fixed4(shadow, 1);

                return col;
            }
            ENDCG
        }
        
        UsePass "Custom/Effect/SimpleShadow/ShadowCaster"
    }
}
