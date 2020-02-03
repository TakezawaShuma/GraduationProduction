//
// Footprint.shader
//

Shader "Custom/Footprint"
{
    Properties
    {
        [HideInInspector]
        _MainTex("Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
	    { 
		    "RenderType"="Transparent"
            "Queue"="Transparent"
	    }
        
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 lightDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;

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
                fixed4 col = fixed4(0, 0, 0, 1);

                fixed3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
                fixed diff = dot(normal, i.lightDir);
                col.rgb *= diff;

                return diff * 0.5;
            }
            ENDCG
        }
    }
}
