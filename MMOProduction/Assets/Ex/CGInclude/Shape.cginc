//
// Shape.cginc
//
// Author : Tama
//
// UnityShader用
// 図形を描くための汎用関数をまとめたファイル
// 

// 多重インクルード防止 -----------------------------
#ifndef SHAPE_CGINC
#define SHAPE_CGINC

// --------------------------------------------------
// 線を描く
// --------------------------------------------------
float4 lineShape(float2 uv, float s, float t)
{
    return step(t, uv.x) * step(abs(t - s), 1.0 - uv.x);
}

// --------------------------------------------------
// 円の距離関数
// --------------------------------------------------
float circle(float2 p, float radius)
{
    return length(p) - radius;
}

// --------------------------------------------------
// ストライプ図形
// uv : テクスチャ座標
// --------------------------------------------------
float4 stripeV(float2 uv, float num)
{
    return step(0, sin(num * uv.x));
}

float4 stripeH(float2 uv, float num)
{
    return step(0, sin(num * uv.y));
}
#endif