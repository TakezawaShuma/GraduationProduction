//
// NowLoadingImage.shader
//
// Author: Tamamura Shuuki
//


// 「NowLoading」テキストイメージに使用するシェーダー
Shader "Custom/NowLoadingImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Transparent" 
			"IgnoreProjector"="True"
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

			// uv切り取り情報
			struct rect
			{
				float left;
				float top;
				float right;
				float bottom;
			};

            sampler2D _MainTex;
            float4 _Color;
			rect _Rect;

            v2f vert (appdata v)
            {
                v2f o;
				//float time = _Time;
				//float amp = 0.5*sin(_Time*100 + v.vertex.x * 100);
				//v.vertex.xyz = float3(v.vertex.x, v.vertex.y+(amp*10), v.vertex.z);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb += _Color;

				// グラデーションをかける
				/*float4 c0 = float4(1, 0, 0, 1);
				float4 c1 = float4(0, 1, 0, 1);
				col.rgb = _Color.rgb;
				//float amount = clamp(abs(0 - i.uv.x)) + (0.5 - 0);
				float time = _Time;
				col.rgb = float4(1 - i.uv.x*abs(cos(time)), 1 - i.uv.y*abs(sin(time)), 1, 1);*/

				/*rect r;
				r.left = 0;
				r.top = 1;
				r.right = 0.5;
				r.bottom = 0;
				if (i.uv.x >= r.left && i.uv.x <= r.right && i.uv.y <= r.top && i.uv.y >= r.bottom)
				{
					
				}
				i.uv.y += 10;*/

                return col;
            }
            ENDCG
        }
    }
}
