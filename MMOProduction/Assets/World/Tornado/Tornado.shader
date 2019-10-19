//
// Tornado.shader
// Actor: Tamamura Shuuki
// Date: 2019/04/26(金)
//

// 竜巻を起こすエフェクト
Shader "Performance/Tornado"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainTex1("Texture 1", 2D) = "white"{}
		_Color("Color", Color) = (1, 1, 1, 1)
		_ColorForce("Color force", Float) = 1
		_WindDir("Wind direction", Vector) = (0.2, 0.0, -0.1, 0)
		_WindForce("Wind force", Vector) = (0.2, 0.0, -0.1, 0)
		_WindTime("Wind time", Float) = 1
    }
    SubShader
    {
	
		// 共通で使用
		CGINCLUDE
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "Assets/CGInclude/SimpleMath.cginc"
		#include "Assets/CGInclude/Transform.cginc"

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

		sampler2D _MainTex;
		sampler2D _MainTex1;
		float4 _Color;
		float _ColorForce;
		float3 _WindDir;
		float3 _WindForce;
		float _WindTime;

		fixed2 random2(fixed2 st) 
		{
			st = fixed2(dot(st, fixed2(127.1, 311.7)),
				dot(st, fixed2(269.5, 183.3)));
			return -1.0 + 2.0*frac(sin(st)*43758.5453123);
		}

		float perlinNoise(fixed2 st)
		{
			fixed2 p = floor(st);
			fixed2 f = frac(st);
			fixed2 u = f * f*(3.0 - 2.0*f);

			float v00 = random2(p + fixed2(0, 0));
			float v10 = random2(p + fixed2(1, 0));
			float v01 = random2(p + fixed2(0, 1));
			float v11 = random2(p + fixed2(1, 1));

			return lerp(lerp(dot(v00, f - fixed2(0, 0)), dot(v10, f - fixed2(1, 0)), u.x),
				lerp(dot(v01, f - fixed2(0, 1)), dot(v11, f - fixed2(1, 1)), u.x),
				u.y) + 0.5f;
		}

		// グレースケール変換
		float grayscale(float3 c)
		{
			return (c.r * 0.3 + c.g * 0.7 + c.b * 0.1);
		}
		ENDCG

		Tags
		{
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
		}

		Cull Off
		ZTest Always
		ZWrite Off

		// 0パス目(アウトライン)
		Pass
		{
			Cull Front
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			v2f vert(appdata v)
			{
				v2f o;
				v.vertex.xyz += v.normal * 0.01;
				float wind = _WindForce.xz * sin(dot(v.vertex.xz, _WindDir.xz) + (_Time.y * _WindTime)) * (v.vertex.y - 1);
				v.vertex.x -= wind;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = float4(1, 1, 1, 1);
				col.a = _Color.a;
				return col;
			}
			ENDCG
		}

		// 1パス目(竜巻の内側)
		Pass
		{
			Cull Off
			//ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			v2f vert(appdata v)
			{
				v2f o;
				float wind = _WindForce.xz * sin(dot(v.vertex.xz, _WindDir.xz) + (_Time.y * _WindTime)) * (v.vertex.y - 1);
				v.vertex.x -= wind;
				//float amp = 0.05*sin(_Time*100 + v.vertex.x * 100);
				//v.vertex.xyz = float3(v.vertex.x + amp, v.vertex.y, v.vertex.z);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				i.uv.x -= _Time * 20;
				i.uv.y -= _Time * 20;
				float c = perlinNoise(i.uv * 10);
				float4 c0 = tex2D(_MainTex, i.uv);
				float4 c1 = _Color;

				// リムライティング
				//float rimColor = float4(1, 1, 1, 1);
				//float rim = 1 - saturate(dot(i.normal, i.viewDir));
				//float3 emission = rimColor * pow(rim, 2.5);

				float4 col = lerp(c0, c1, 0.5);
				col.rgb *= _ColorForce;
				//col.rgb += emission;
				col.a = _Color.a;
				return col;
			}
			ENDCG
		}

		// 2パス目(竜巻の外側)
		Pass
		{
			Cull Off
			ZWrite Off
			ZTest Always
			Blend One One

			CGPROGRAM
			v2f vert(appdata v)
			{
				v2f o;
				float wind = _WindForce.xz * sin(dot(v.vertex.xz, _WindDir.xz) + (_Time.y * _WindTime)) * (v.vertex.y - 1);
				v.vertex.x -= wind;
				v.vertex.xyz = scale(v.vertex.xyz, float3(1.3, 1.3, 1.3));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				uv.x += _Time*50;
				uv.y -= _Time*10;
				float c = perlinNoise(uv * 10);
				float4 c0 = tex2D(_MainTex1, uv);
				float4 c1 = tex2D(_MainTex, uv);
				float4 c2 = _Color;
				c0 = (grayscale(c0.rgb) > 0)?1:0;
				float4 col = c0 * c1 * c2;
				col.rgb *= _ColorForce;
				return col;
			}
			ENDCG
		}

		// 3パス目(竜巻の根本)
		Pass
		{
			Cull Off
			ZWrite Off
			Blend One One

			CGPROGRAM
			v2f vert(appdata v)
			{
				v2f o;
				float wind = _WindForce.xz * sin(dot(v.vertex.xz, _WindDir.xz) + (_Time.y * _WindTime)) * (v.vertex.y - 1);
				v.vertex.x -= wind;
				v.vertex.xyz = scale(v.vertex.xyz, float3(1.5, 1.5, 0.1));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				uv.x += _Time*10;
				uv.y -= _Time*10;
				float c = perlinNoise(uv * 10);
				float4 c0 = tex2D(_MainTex1, uv);
				float4 c1 = tex2D(_MainTex, uv);
				float4 c2 = _Color;
				c0 = (grayscale(c0.rgb) > 0)?1:0;
				float4 col = c0 * c1 * c2;
				col.rgb *= _ColorForce;
				return col;
			}
			ENDCG
		}
    }
}
