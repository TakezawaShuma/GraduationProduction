//
// LoginBackStage.shader
//


// Login時の背景ステージに使用するシェーダ
Shader "Custom/LoginBackStage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SubTex ("Sub Texture", 2D) = "white"{}
		//_Splat ("Splat", 2D) = "white"{}
		_Reflectance("Reflection", Float) = 0.5
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_BlendRatio ("Blend Ratio", Range(0, 1)) = 0.5

		[Toggle(BLEND_FLAG)] _BlendFlag("Blend Flag", Float) = 1
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Opaque" 
			"LightMode" = "ForwardBase"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#pragma shader_feature BLEND_FLAG
			#pragma shader_feature DEBUG_EROSION_FLAG


            #include "UnityCG.cginc"

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
				float4 surfaceLightColor : COLOR;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
			sampler2D _SubTex;
			sampler2D _Splat;
			float4 _Color;
			float _BlendRatio;

			uniform float _Reflectance;	
			uniform float4 _LightColor0;	//_LightColor0はuniformで定義してあげないとエラーになる

			float4 hash4fast(float2 gridcell)
			{
				const float2 OFFSET = float2(26.0, 161.0);
				const float DOMAIN = 71.0;
				const float SOMELARGEFIXED = 951.135664;
				float4 P = float4(gridcell.xy, gridcell.xy + 1);
				P = frac(P*(1 / DOMAIN)) * DOMAIN;
				P += OFFSET.xyxy;
				P *= P;
				return frac(P.xzxz * P.yyww * (1 / SOMELARGEFIXED));
			}

			float2 random2(float2 st) {
				st = float2(dot(st, fixed2(127.1, 311.7)),
					dot(st, float2(269.5, 183.3)));
				return -1.0 + 2.0*frac(sin(st)*43758.5453123);
			}

			float perlinNoise(fixed2 st)
			{
				fixed2 p = floor(st);
				fixed2 f = frac(st);
				fixed2 u = f*f*(3.0 - 2.0*f);

				float v00 = random2(p + fixed2(0, 0));
				float v10 = random2(p + fixed2(1, 0));
				float v01 = random2(p + fixed2(0, 1));
				float v11 = random2(p + fixed2(1, 1));

				return lerp(lerp(dot(v00, f - fixed2(0, 0)), dot(v10, f - fixed2(1, 0)), u.x),
					lerp(dot(v01, f - fixed2(0, 1)), dot(v11, f - fixed2(1, 1)), u.x),
					u.y) + 0.5f;
			}

			float fBm(float2 st)
			{
				float f = 0;
				fixed2 q = st;

				f += 0.5000*perlinNoise(q); q = q*2.01;
				f += 0.2500*perlinNoise(q); q = q*2.02;
				f += 0.1250*perlinNoise(q); q = q*2.03;
				f += 0.0625*perlinNoise(q); q = q*2.01;

				return f;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.uv;
				o.surfaceLightColor = _LightColor0 * _Reflectance * max(dot(_WorldSpaceLightPos0, o.normal), 0);
				return o;
			}

            float4 frag (v2f i) : SV_Target
            {
				// テクスチャブレンド用
				float4 c0 = tex2D(_MainTex, i.uv*100);
				float4 c1 = tex2D(_SubTex, i.uv);
				float c = fBm(i.uv * 10);
				float4 c2 = float4(c, c, c, 1);

				// ブレンド状態を調整する
				float4 col;
#ifdef BLEND_FLAG
				col = lerp(c0, c1, c2);
#else
				col = lerp(c0, c1, _BlendRatio);
#endif
				//float4 c3 = tex2D(_Splat, i.uv) * _Color;

				return col * (i.surfaceLightColor + 0.5) * _Color;
            }
            ENDCG
        }
		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
