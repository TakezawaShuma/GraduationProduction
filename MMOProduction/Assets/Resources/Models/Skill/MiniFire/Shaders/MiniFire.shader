// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//
// Fireball.shader
//
// 火球に使用するエフェクトシェーダー
// 

Shader "Custom/Skill/MiniFire/MiniFire"
{
	Properties
	{
		[HideInInspector]
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveTex ("Dissolve Tex", 2D) = "white" {}

		_MainColor ("Main Color", Color) = (1, 1, 1, 1)
		_GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
		_Opacity ("Glow Opacity", Float) = 2.0
		_Strength ("Glow Strength", Float) = 0.3
		_Threshold ("Dissolve Threshold", Range(0, 1)) = 0.5
	}
	SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "Assets/Ex/CGInclude/Noise.cginc"
		#include "Assets/Ex/CGInclude/SimpleMath.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _DissolveTex;
		uniform float4 _MainColor;
		uniform float4 _GlowColor;
		uniform float _Opacity;
		uniform float _Strength;
		uniform float _Threshold;
		ENDCG

		// ---------------------------------------
		// 1パス目
		// 通常のレンダリング
		// ---------------------------------------
		Pass
		{
			Tags
			{
				"RenderType"="Opaque"
			}
			LOD 100

			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vsIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vsOut
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			vsOut vert(vsIn v)
			{
				vsOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(vsOut i) : SV_Target
			{
				float4 emission = float4(1, 0, 0, 1);
				return _MainColor + emission;
			}
			ENDCG
		}

		// ---------------------------------------
		// 2パス目
		// Glowエフェクト
		// ---------------------------------------
		Pass
		{
			Tags
			{
				"RenderType"="Transparent"
				"IgnoreProjector"="True"
				"Queue"="Transparent"
			}
			LOD 100

			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vsIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vsOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			vsOut vert(vsIn v)
			{
				vsOut o;
				v.vertex += float4(v.normal * (_Strength * 0.8), 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				return o;
			}

			float4 frag(vsOut i) : SV_Target
			{
				float4 col = _GlowColor;

				// ベクトルを正規化
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);

				// Glowエフェクト
				float opacity = saturate(abs(dot(normal, viewDir)));
				//opacity = pow(opacity, 3);
				col.a *= opacity;

				// リムライティング
				float rim = saturate(1 - abs(dot(normal, viewDir)));
				rim = pow(rim, 3);
				col += rim;

				return col;
			}
			ENDCG
		}

		// ---------------------------------------
		// 3パス目
		// 周りのもや1
		// ---------------------------------------
		Pass
		{
			Tags
			{
				"RenderType" = "Transparent"
				"IgnoreProjector"="True"
				"Queue"="Transparent"
			}
			LOD 100

			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vsIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vsOut
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			vsOut vert(vsIn v)
			{
				vsOut o;
				v.vertex += float4(v.normal * _Strength, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.uv;
				return o;
			}

			float4 frag(vsOut i) : SV_Target
			{
				i.uv.x -= _Time * 0.1;
				float4 col = _GlowColor;
				float4 main = tex2D(_MainTex, i.uv);
				float4 dissolve = tex2D(_DissolveTex, i.uv);
				float g = toGray(dissolve);
				if (g <= _Threshold)
				{
					col.a = 0;
				}
				
				float noise = fBm(i.uv * 10);

				col.rgb = lerp(float3(1, 0.7, 0), col.rgb, noise);

				// エッジを強調
				float edgeDist = distance(_Threshold, g);
				if (edgeDist <= 0.01)
				{
					col.gb *= 0.5;
				}

				return col;
			}
			ENDCG
		}

		// ---------------------------------------
		// 4パス目
		// 周りのもや2
		// ---------------------------------------
			Pass
		{
			Tags
			{
				"RenderType" = "Transparent"
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
			}
			LOD 100

			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vsIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vsOut
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			vsOut vert(vsIn v)
			{
				vsOut o;
				v.vertex += float4(v.normal * _Strength * 1.5, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.uv;
				return o;
			}

			float4 frag(vsOut i) : SV_Target
			{
				float4 col = _GlowColor;
				float4 main = tex2D(_MainTex, i.uv);
				float4 dissolve = tex2D(_DissolveTex, i.uv);
				float g = toGray(dissolve);
				i.uv.x += _Time * 0.1;
				float noise = fBm(i.uv * 10);
				if (noise <= _Threshold)
				{
					col.a = 0;
				}
				col.rgb += 0.4;
				col.rgb = lerp(float3(1, 0.7, 0), col.rgb, noise);
				
				// エッジを強調
				float edgeDist = distance(_Threshold, noise);
				if (edgeDist <= 0.01)
				{
					col.gb *= 0.5;
				}

				return col;
			}
			ENDCG
		}
	}
}
