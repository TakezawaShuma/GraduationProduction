//
// SwordTrail.shader
//
// 剣の軌跡
//

Shader "Custom/Effect/SwordTrail"
{
	Properties
	{
		// テクスチャ
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Tex", 2D) = "white" {}

		// 色
		_MainColor ("Main Color", Color) = (1, 1, 1, 1)
		_TopColor("Top Color", Color) = (1, 1, 1, 1)
		_BottomColor("Bottom Color", Color) = (0, 0, 1, 1)

		// 閾値（ノイズ透過具合）
		_Threshold ("Noise Threshold", Range(0, 1)) = 1
		// グラデーションの比率
		_Ratio ("Gradation Ratio", Range(0, 1)) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}
		LOD 100

		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			//#include "Assets/CGInclude/Shape.cginc"
			//#include "Assets/CGInclude/Noise.cginc"
			#include "Assets/Ex/CGInclude/SimpleMath.cginc"
			//#include "Assets/CGInclude/Transform.cginc"

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

			uniform sampler2D _MainTex;
			uniform sampler2D _NoiseTex;
			uniform float4 _MainColor;
			uniform float4 _TopColor;
			uniform float4 _BottomColor;
			uniform float _Threshold;
			uniform float _Ratio;

			float2 rotate(float2 uv, float angle)
			{
				const float pi = 3.1415;
				angle *= pi / 180;
				float2x2 rotateMat = {
					cos(angle), -sin(angle),
					sin(angle), cos(angle)
				};
				return mul(uv, rotateMat);
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = _MainColor;
				float4 top = _TopColor;
				float4 bottom = _BottomColor;

				// テクスチャを回転（微調整）
				float2 uv = rotate(i.uv, 180);
				float4 noise = tex2D(_NoiseTex, uv);

				// ノイズテクスチャから透明度を変更
				float g = toGray(noise);
				if (g >= _Threshold) col.a = 0;	// 白い部分を透明に

				// グラデーションをかける
				col.rgb *= lerp(top.rgb, bottom.rgb, i.uv.x * _Ratio);
				col.a *= lerp(top.a, bottom.a, i.uv.x);

				return col;
			}
			ENDCG
		}
	}
}
