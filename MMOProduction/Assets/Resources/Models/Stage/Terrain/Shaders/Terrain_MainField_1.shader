Shader "Custom/Terrain_MainField_1"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

    // AreaMap
        _CaveAreaMap("Cave Area Map", 2D) = "white" {}
        _CaveAreaTex("Cave Area Tex", 2D) = "white" {}
        _CaveColor("Cave Color", Color) = (1, 1, 1, 1)

            // Shadow
            _ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;

        sampler2D _CaveAreaMap;
        sampler2D _CaveAreaTex;
        fixed4 _CaveColor;

        fixed4 _ShadowColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;
            fixed4 col = tex2D(_MainTex, uv) * _Color;
            fixed4 caveAreaMap = tex2D(_CaveAreaMap, uv);
            fixed4 caveAreaCol = tex2D(_CaveAreaTex, uv);
            if (caveAreaMap.r >= 0.99)
            {
                col.rgb *= caveAreaCol.rgb * _CaveColor + 0.5;
            }

            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
