//
// TitleLogo.shader
// 
// Author : Tama
//
// タイトル画面のロゴに使用
//

Shader "Custom/TitleLogo"
{
    Properties
    {
		//[HideInInspector] 
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (0, 0, 0, 1)

		// Hide
		[HideInInspector] _Luster ("Luster", Float) = 0
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Assets/Ex/CGInclude/Noise.cginc"
			#include "Assets/Ex/CGInclude/Shape.cginc"

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

            sampler2D _MainTex;
			float4 _Color;
			float _Luster;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
				// メインカラー
                float4 col = tex2D(_MainTex, i.uv) * _Color;

				// ブレンドカラー
				float4 c0 = float4(1, 0, 0, 1);
				float4 c1 = float4(1, 1, 0, 1);
				float c = fBm(i.uv * 10);
				float4 c2 = float4(c, c, c, 1);
				c0.rgb += c2.rgb - 0;

				float4 topCol = float4(1, 1, 1, 1);
				float4 bottomCol = float4(0, 0, 1, 1);
				float amount = abs(0.5 - i.uv.y) + 0;
				
				// 光沢を表現する
				float t = _Luster;
				float4 l0 = lineShape(i.uv, 0.9, t) * float4(0.5, 1, 0, 1);
				t -= 0.1;
				float4 l1 = lineShape(i.uv, 0.95, t) * float4(0.5, 1, 0, 1);

				return (lerp(c1, c0, amount) + l0 + l1) * col.a;
                //return lerp(c0, c1, c2) * col.a;
            }
            ENDCG
        }
    }
}
