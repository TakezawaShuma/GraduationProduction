//
// Toon_Fountain.shader
//
// Author: Tama
//
// 噴水
//

Shader "Custom/Prop/Toon_Fountain"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)

        _NormalMap("Normal Map", 2D) = "white" {}

        _RampMap("Ramp Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }

        // Rendering Pass
        Pass
        {
            Cull Back

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
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 lightDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            sampler2D _NormalMap;

            sampler2D _RampMap;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                TANGENT_SPACE_ROTATION;
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed atten = LIGHT_ATTENUATION(i);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                half3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
                half3 lightDir = normalize(i.lightDir);
                fixed diff = saturate(dot(normal, lightDir));
                fixed4 ramp = tex2D(_RampMap, float2(diff, 0.5)) * 0.5 + 0.5;
                col.rgb *= ramp * atten;
                return col;
            }
            ENDCG
        }
    }
}
