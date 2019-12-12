//
// Fireball.shader
//
// 火球に使用するエフェクトシェーダー
// 

Shader "Custom/Fireball"
{
	Properties
	{
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}

		_DissolveTex ("Dissolve Tex", 2D) = "white" {}
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
		_Opacity ("Glow Opacity", Float) = 2.0
		_Strength ("Glow Strength", Float) = 0.3
	}
	SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "Assets/Ex/CGInclude/Noise.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _DissolveTex;
		uniform float4 _Color;
		uniform float4 _GlowColor;
		uniform float _Opacity;
		uniform float _Strength;
		ENDCG

		// ---------------------------------------
		// 1パス目
		// 通常のレンダリング
		// ---------------------------------------
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}
			LOD 100

			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float t = _Time;
				i.uv.y -= t;
				float4 dissolve = tex2D(_DissolveTex, i.uv);
				float4 col = _Color;
				float c = fBm(i.uv * 100);
				col.rgb += float3(c, c, c);
				return col;
			}
			ENDCG
		}

		// ---------------------------------------
		// 2パス目
		// 外側に徐々に膨らませる
		// ---------------------------------------
		Pass
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue"="Transparent"
			}
			LOD 100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// 膨張させる
				v.vertex += float4(v.normal * _Strength, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = float4(1, 1, 1, 1);

				// 各ベクトルを正規化
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);

				// Glowエフェクトの設定
				float opacity = saturate(abs(dot(viewDir, normal)));
				opacity = pow(opacity, 3);

				// ディゾルブテクスチャをスクロール
				float t = _Time;
				//i.uv.y -= t * 10;
				float4 dissolve = tex2D(_DissolveTex, i.uv);
				dissolve.rgb += 0.5;

				col = float4(_GlowColor.xyz, opacity);
				col.rgb += dissolve.rgb;

				return col;
			}
			ENDCG
		}
	}
}
