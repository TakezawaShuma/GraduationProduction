﻿//
// MinimapRader.shader
//
// ミニマップに表示するターゲットのレーダー
//

Shader "Custom/UI/Minimap/MinimapRader"
{
    Properties
    {
        [HideInInspector]
        _MainTex("Texture", 2D) = "white" {}
        _TargetTex("Target Texture", 2D) = "white" {}
        _RaderColor("Rader Color", Color) = (1, 1, 1, 1)
        _RaderSize("Rader Size", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

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

            sampler2D _MainTex;
            sampler2D _TargetTex;
            fixed4 _RaderColor;
            fixed _RaderSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_TargetTex, i.uv);

                // 中心にレーダーを表示
                fixed dist = distance(fixed2(0.5, 0.5), i.uv);
                if (dist <= _RaderSize)
                {
                    col.rgb *= _RaderColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
