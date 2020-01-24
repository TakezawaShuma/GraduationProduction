//
// MinimapRader.shader
//
// ミニマップに表示するターゲットのレーダー
//

Shader "Custom/UI/Minimap/MinimapRader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Rader Color", Color) = (1, 1, 1, 1)
        _Radius ("Rader Radius", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Radius;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float d = distance(i.uv, float2(0.5, 0.5));
                if (d <= _Radius)
                {
                    col = _Color;
                }
                return col;
            }
            ENDCG
        }
    }
}
