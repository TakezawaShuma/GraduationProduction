//
// IceBlock_Normal.shader
//
// 通常の氷オブジェクトに適用するシェーダー
//

Shader "Custom/Skill/IceBlock_Normal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Brightness ("Shadow Brightness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
        LOD 100

		//Cull Off
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

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
				float3 normal : NORMAL;
            };

            struct VSOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 lightDir : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
			float _Brightness;

			VSOut vert(VSIn v)
			{
				VSOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.lightDir = WorldSpaceLightDir(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				return o;
			}

			float4 frag(VSOut i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				// 拡散反射ライティング
				float3 normal = normalize(i.normal);
				float3 lightDir = normalize(i.lightDir);
				float diff = dot(normal, lightDir);
				col.rgb *= diff + _Brightness;

				// 透明度を変化
				//float3 viewDir = normalize(i.viewDir);
				//float alpha = saturate(abs(1 - dot(normal, viewDir)));
				//col.a = alpha;

				return col;
			}
            ENDCG
        }
    }
}
