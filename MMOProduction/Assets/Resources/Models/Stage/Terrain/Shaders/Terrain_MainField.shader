//
// Terrain_MainField.shader
//
// Author: Tama
//
// 地形
//

Shader "Custom/Terrain_MainField"
{
    Properties
    {
        // Albedo Color
        _MainTex ("Texture", 2D) = "white" {}
        
        // AreaMap
        _CaveAreaMap("Cave Area Map", 2D) = "white" {}
        _CaveAreaTex("Cave Area Tex", 2D) = "white" {}
        _CaveColor("Cave Color", Color) = (1, 1, 1, 1)

        // Shadow
        _ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {

        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 lightDir : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;

            sampler2D _CaveAreaMap;
            sampler2D _CaveAreaTex;
            fixed4 _CaveColor;

            fixed4 _ShadowColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.lightDir = WorldSpaceLightDir(v.vertex);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 caveAreaMap = tex2D(_CaveAreaMap, i.uv);
                fixed4 caveAreaCol = tex2D(_CaveAreaTex, i.uv);
                if (caveAreaMap.r >= 0.99)
                {
                    col.rgb *= caveAreaCol.rgb * _CaveColor + 0.5;
                }

                fixed3 shadow = half3(min((SHADOW_ATTENUATION(i) + _ShadowColor.rgb), 1));
                col.rgb *= shadow;
                return col;
            }
            ENDCG
        }

        UsePass "Custom/Effect/SimpleShadow/ShadowCaster"
    }
}
