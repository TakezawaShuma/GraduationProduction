Shader "Custom/BaseTown"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        // 地面用
        _GroundTex("Ground Texture", 2D) = "white" {}
        _GrassMap("Grass map Texture", 2D) = "white" {}
        _GroundMask("Ground mask Texture", 2D) = "white" {}

        // 道用
        _RoadTex("Road Texture", 2D) = "white" {}
        _RoadMap("Road map Texture", 2D) = "white" {}
        _RoadColor("Road Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha

        CGINCLUDE
        // 接空間へ変換する行列を生成する
        // ※ 接空間からローカル空間への変換の逆行列で、ローカルのライトを接空間に変換する
        float4x4 InvTangentMatrix(float3 tan, float3 bin, float3 nor)
        {
            float4x4 mat = float4x4(
                float4(tan, 0),
                float4(bin, 0),
                float4(nor, 0),
                float4(0, 0, 0, 1)
                );

            // 正規直交系行列なので、逆行列は転置行列で求まる
            return transpose(mat);
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 normal;
            float3 lightDir;
        };

        sampler2D _MainTex;

        // 地面
        sampler2D _GroundTex;
        sampler2D _GrassMap;
        sampler2D _GroundMask;

        // 道
        sampler2D _RoadTex;
        sampler2D _RoadMap;
        fixed4 _RoadColor;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // 地面合成関数
        fixed4 ground(float2 uv)
        {
            fixed4 c0 = tex2D(_GroundTex, uv);
            fixed4 c1 = tex2D(_GrassMap, uv);
            fixed4 c2 = tex2D(_GroundMask, uv);

            return lerp(c0, c1, c2);
        }

        // 道合成関数
        fixed4 road(float2 uv, float3 lightDir)
        {
            fixed map = tex2D(_RoadMap, uv).r;

            if (map >= 0.9)
            {
                /*float3 normal = fixed4(UnpackNormal(tex2D(_RoadTex, uv * 10)), 1);
                lightDir = normalize(lightDir);
                float diff = max(0, dot(normal, lightDir));

                return diff * _RoadColor;*/

                return tex2D(_RoadTex, uv * 10) * 0.5;
            }

            
            return fixed4(1, 1, 1, 1);
        }

        

        // 頂点シェーダー
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // ローカル空間上での接空間ベクトルの方向を求める
            float3 n = normalize(v.normal);
            float3 t = normalize(v.tangent);
            float3 b = cross(n, t);

            // ワールド位置にあるライトをローカル空間へ変換する
            float3 localLight = mul(unity_WorldToObject, _WorldSpaceLightPos0);

            // ローカルライトを接空間へ変換する（行列の掛ける順番に注意）
            o.lightDir = mul(localLight, InvTangentMatrix(t, b, n));
        }

        // サーフェースシェーダー
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // 地面色作成
            c *= ground(IN.uv_MainTex);
            // 道作成
            c *= road(IN.uv_MainTex, IN.lightDir);

            // Apply
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
