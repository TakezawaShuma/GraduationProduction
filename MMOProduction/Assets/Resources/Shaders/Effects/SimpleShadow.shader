//
// SimpleShadow.shader
//

Shader "Custom/Effect/SimpleShadow"
{
    Properties
    {
        [HideInInspector]
        _MainTex("Texture", 2D) = "white" {}

        _Color("Color", Color) = (1, 1, 1, 1)
        _ShadowColor("Shadow Color", Color) = (1, 1, 1, 1)
        _DiffuseColor("Diffese Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {

        Pass
        {
            Name "ReceiveShadow"

            Tags
            {
                "LightMode" = "ForwardBase"
            }
            LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 diff : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            fixed4 _Color;
            fixed4 _ShadowColor;
            fixed4 _DiffuseColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half dotNL = saturate(dot(worldNormal, _WorldSpaceLightPos0));
                o.diff = fixed4(saturate((dotNL + _DiffuseColor.rgb)), 1);

                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 shadow = half3(min((SHADOW_ATTENUATION(i) + _ShadowColor.rgb), 1));
                return _Color * i.diff * fixed4(shadow, 1);
            }
            ENDCG
        }

        Pass
        {
            Name "ShadowCaster"

            Tags
            {
                "LightMode" = "ShadowCaster"
            }
            LOD 100

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i);
            }
            ENDCG
        }
    }
}
