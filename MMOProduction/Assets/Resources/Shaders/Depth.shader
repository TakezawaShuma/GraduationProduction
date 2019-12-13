//
// Depth.shader
//
// デプスバッファの内容を表示するシェーダー
//

Shader "Custom/Depth"
{
    Properties
    {
		[HideInInspector]
		_MainTex("Texture", 2D) = "white" {}

		_Scale("Scale", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
        LOD 100

		GrabPass
		{
			"_GrabTex"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VSIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VSOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;
			uniform sampler2D _GrabTex;
			uniform float4 _Scale;

			VSOut vert(VSIn v)
			{
				VSOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(VSOut i) : SV_Target
			{
				float2 uv = 0.5 + (i.uv - 0.5) / float2(_Scale.x, _Scale.y);
				float4 col = tex2D(_GrabTex, uv);
				float depth = UNITY_SAMPLE_DEPTH(col);

				//return col;
				return float4(depth, depth, depth, 1);
			}
            ENDCG
        }
    }
}
