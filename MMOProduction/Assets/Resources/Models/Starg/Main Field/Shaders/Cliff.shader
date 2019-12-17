//
// Cliff_A.shader
//
// Author : Tama
//
// 崖オブジェクトに使用するシェーダファイル
//

Shader "Custom/Stage/Cliff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Brightness ("Brightness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Opaque" 
		}
        LOD 100

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
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 lightDir : TEXCOORD1;
            };

            sampler2D _MainTex;
			uniform float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.lightDir = WorldSpaceLightDir(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

				// 各ベクトルの正規化
				float3 normal = normalize(i.normal);
				float3 lightDir = normalize(i.lightDir);

				// 拡散反射ライティング
				float diff = dot(normal, lightDir);
				col.rgb *= diff + _Brightness;

                return col;
            }
            ENDCG
        }
		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
