//
// FireballEffect.shader
//
// 火球の周りを飛び交うエフェクト
//

Shader "Custom/FireballEffect"
{
	SubShader
	{
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Assets/Ex/CGInclude/Shape.cginc"
			#include "Assets/Ex/CGInclude/Noise.cginc"

			struct VS_INPUT
			{
				uint id : SV_VertexID;
			};

			struct VS_OUTPUT
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 col : COLOR;
			};

			// 玉の構造体
			struct Fireball
			{
				float3 pos;
				float3 startPos;
				float3 accel;
				float4 col;
			};

			// 玉の構造化バッファ
			StructuredBuffer<Fireball> _Balls;

			sampler2D _MainTex;

			VS_OUTPUT vert(VS_INPUT i)
			{
				VS_OUTPUT o;
				o.pos = float4(_Balls[i.id].pos, 1);
				o.uv = float2(0, 0);
				o.col = _Balls[i.id].col;
				return o;
			}

			[maxvertexcount(4)]
			void geom(point VS_OUTPUT input[1], inout TriangleStream<VS_OUTPUT> outStream)
			{
				VS_OUTPUT output;

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
						float size = 0.05;
						output.pos = pos + mul(float4((uv * 2 - float2(1, 1)) * size, 0, 1), billboardMatrix);
						output.pos = mul(UNITY_MATRIX_VP, output.pos);

						// 色
						output.col = col;

						// ストリームに頂点を追加
						outStream.Append(output);
					}
				}

				// トライアングルストリップを終了
				outStream.RestartStrip();
			}

			float4 frag(VS_OUTPUT i) : SV_Target
			{
				//float4 col = tex2D(_MainTex, i.uv) * i.col;
				float4 col = i.col;

				float2 offset_uv = float2(0.5, 0.5);
				float dist = distance(offset_uv, i.uv);
				//float c = fBm(i.uv * 8); 
				
				//float4 col = i.color;
				//col.rgb += float3(c, c, c);
				//col.rgb += 0.5;
				col.rgb += max(0, (1 - dist));
				col.a *= max(0, (1 - dist) - 0.5);
				if (dist > 0.5) discard;

				
				// アルファが一定値以下で中断
				//if (col.a < 0.3) discard;	
				//col.a = i.col.a;
				return col;
			}
			ENDCG
		}
	}
}
