﻿//
// SpruceBranch_Toon.shader
//
// Author: Tama
// 
// トウヒの木に適用するシェーダー（枝）
//

Shader "Custom/Stage/Prop/SpruceBranch_Toon"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        
        _MaskClipValue("Mask Clip Value", Range(0, 1)) = 0.5

        _Color("Main Color", Color) = (1, 1, 1, 1)

        _Brightness("Brightness", Range(0, 1)) = 0.5
        _ToonStrength("Toon Strength", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }

        /*Pass
        {
            Cull Front

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed _MaskClipValue;

            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.xyz += v.normal * 0.05;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if (col.a <= _MaskClipValue) discard;
                return 0;
            }
            ENDCG
        }*/

        Pass
        {
            Cull Off

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
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _RampTex;

            fixed _MaskClipValue;

            fixed4 _Color;
            fixed _Brightness;
            fixed _ToonStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.lightDir = WorldSpaceLightDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half3 normal = normalize(i.normal);
                half3 lightDir = normalize(i.lightDir);
                float2 uv = i.uv;

                fixed4 col = tex2D(_MainTex, uv);
                if (col.a <= _MaskClipValue) discard;
                col = col * _Color * 0.5 + _ToonStrength;

                // トゥーンライティング
                half dotNL = dot(normal, lightDir);
                fixed4 ramp = fixed4(tex2D(_RampTex, fixed2(dotNL, 0.5)).rgb, 1);
                col *= ramp + _Brightness;

                return col;
            }
            ENDCG
        }

        UsePass "Custom/Effect/SimpleShadow/ShadowCaster"
    }
}
