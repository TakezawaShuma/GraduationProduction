//
// ToonHouse.shader
//
// Author: Tama
//
// 拠点の家
// 

Shader "Custom/BaseTown/Toon_House"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

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
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _RampTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.lightDir = WorldSpaceLightDir(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed atten = LIGHT_ATTENUATION(i);

                fixed4 col = tex2D(_MainTex, i.uv);
                half3 normal = normalize(i.normal);
                half3 lightDir = normalize(i.lightDir);
                fixed diff = saturate(dot(normal, lightDir)) * 0.5 + 0.5;
                fixed3 ramp = tex2D(_RampTex, fixed2(diff, 0.5));
                col.rgb *= ramp * atten;
                return col;
            }
            ENDCG
        }
    }
}
