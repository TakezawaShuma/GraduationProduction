Shader "Custom/Event/MeryChristmas/ToonSnow"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_RampTex("Ramp Texture", 2D) = "white" {}
		_Brightness("Ramp Brightness", Range(0, 1)) = 0.5
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
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

				uniform sampler2D _MainTex;
				uniform sampler2D _RampTex;
				uniform float _Brightness;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.normal);
					o.uv = v.uv;
					o.lightDir = WorldSpaceLightDir(v.vertex);

					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					float4 col = tex2D(_MainTex, i.uv);

					float d = dot(i.normal, i.lightDir) * 0.5 + 0.5;
					float3 ramp = tex2D(_RampTex, float2(d, 0.5)).rgb;

					ramp += _Brightness;
					col.rgb *= ramp;
					col.a = 0;

					return col;
				}
				ENDCG
			}
			//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}
