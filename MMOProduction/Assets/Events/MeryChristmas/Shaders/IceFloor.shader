//
// Ice.shader
//
// Author : Tama
//
// 氷スキル時にフィールドを凍らすシェーダー
//

Shader "Custom/Event/MeryChristmas/IceFloor"
{
	Properties
	{
		_RampTex("Ramp", 2D) = "white" {}
		_BumpTex("Bump", 2D) = "white" {}
		_BumpRamp("Bump Ramp", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_EdgeThickness("Silouette Dropoff Rate", float) = 1.0
		_DistortStrength("Distort Strength", float) = 1.0
	}

	SubShader
	{
		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		// Background distortion
		Pass
		{
			Tags
			{
				"Queue" = "Transparent"
			}

			ZWrite On
			Blend One One
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#include "UnityCG.cginc"
#include "Assets/Ex/CGInclude/Noise.cginc"

		// Properties
		sampler2D _BackgroundTexture;
		sampler2D _BumpTex;
		//uniform sampler2D _IceTex;
		float     _DistortStrength;

		struct vertexInput
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float3 texCoord : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		struct vertexOutput
		{
			float4 pos : SV_POSITION;
			float4 grabPos : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		// グレースケールに変換
		/*float gray(float4 c)
		{
			return (c.r * 0.3 + c.g * 0.6 + c.b * 0.1);
		}*/

		//vertexOutput vert(vertexInput input)
		//{
		//	vertexOutput output;

		//	// convert input to world space
		//	output.pos = UnityObjectToClipPos(input.vertex);
		//	float3 normal = UnityObjectToWorldNormal(input.normal);

		//	// use ComputeGrabScreenPos function from UnityCG.cginc
		//	// to get the correct texture coordinate
		//	output.grabPos = ComputeGrabScreenPos(output.pos);

		//	// distort based on bump map
		//	float3 bump = tex2Dlod(_BumpTex, float4(input.texCoord.xy, 0, 0)).rgb;
		//	output.grabPos.x += bump.x * _DistortStrength;
		//	output.grabPos.y += bump.y * _DistortStrength;

		//	return output;
		//}

		vertexOutput vert(vertexInput input)
		{
			vertexOutput output;

			// convert input to world space
			output.pos = UnityObjectToClipPos(input.vertex);
			//float3 normal = UnityObjectToWorldNormal(input.normal);

			// use ComputeGrabScreenPos function from UnityCG.cginc
			// to get the correct texture coordinate
			output.uv = input.uv;

			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			float4 col = tex2Dproj(_BackgroundTexture, input.grabPos);
			//col = float4(0, 1, 1, 0.5);
			float c = fBm(input.uv * 5);
			float4 c0 = float4(0, 0.5, 1, 1);
			float4 c1 = float4(1, 1, 1, 1);
			float4 c2 = float4(c, c, c, 1);
			col = lerp(c0, c1, c2);
			//col.rgb += 0.2;
			return col;
		}
		ENDCG
	}

		// Transparent color & lighting pass
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase" // allows shadow rec/cast
				"Queue" = "Transparent"
			}
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Properties
			sampler2D       _RampTex;
			sampler2D       _BumpTex;
			sampler2D       _BumpRamp;
			uniform float4  _Color;
			uniform float   _EdgeThickness;
			sampler2D       _BackgroundTexture;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 texCoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float3 texCoord : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				// convert input to world space
				output.pos = UnityObjectToClipPos(input.vertex);
				float4 normal4 = float4(input.normal, 0.0);
				output.normal = normalize(mul(normal4, unity_WorldToObject).xyz);
				output.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.vertex).xyz);

				// texture coordinates
				output.texCoord = input.texCoord;

				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				// compute sillouette factor
				float edgeFactor = abs(dot(input.viewDir, input.normal));
				float oneMinusEdge = 1.0 - edgeFactor;
				// get sillouette color
				float3 rgb = tex2D(_RampTex, float2(oneMinusEdge, 0.5)).rgb;
				// get sillouette opacity
				float opacity = min(1.0, _Color.a / edgeFactor);
				opacity = pow(opacity, _EdgeThickness);

				// apply bump map
				float3 bump = tex2D(_BumpTex, input.texCoord.xy).rgb + input.normal.xyz;
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float ramp = clamp(dot(bump, lightDir), 0.001, 1.0);
				float4 lighting = float4(tex2D(_BumpRamp, float2(ramp, 0.5)).rgb, 1.0);
				lighting.a = 0.5;
				float4 col = float4(rgb, opacity) * lighting;
				return col;
			}

			ENDCG
		}

		// Shadow pass
		Pass
		{
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f 
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}
