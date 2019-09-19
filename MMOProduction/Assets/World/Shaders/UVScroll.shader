//
// UVScroll.shader
// Actor: Tamamura Shuuki
//


// uvスクロール用
Shader "Custom/UVScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Alpha("Alpha", Float) = 1
		_ScrollX("Scroll x", Float) = 0
		_ScrollY("Scroll y", Float) = 0
		_Speed("Speed", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100

        Pass
        {

			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Alpha;
			float _ScrollX;
			float _ScrollY;
			float _Speed;

			fixed2 random2(fixed2 st) {
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

			float fBm(fixed2 st)
			{
				float f = 0;
				fixed2 q = st;

				f += 0.5000*perlinNoise(q); q = q * 2.01;
				f += 0.2500*perlinNoise(q); q = q * 2.02;
				f += 0.1250*perlinNoise(q); q = q * 2.03;
				f += 0.0625*perlinNoise(q); q = q * 2.01;

				return f;
			}

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float amp = 0.02 * sin(_Time * 100 + o.vertex.x);
				o.vertex.xyz = float3(o.vertex.x, o.vertex.y + amp, o.vertex.z);
				
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				uv.x += _Time*_ScrollX;
				uv.y += _Time*_ScrollY;
				float c = fBm(uv*(20));

                // sample the texture
                fixed4 col = tex2D(_MainTex, uv*c);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				col.a = _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
