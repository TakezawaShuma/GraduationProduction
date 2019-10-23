//
// Shape.cginc
//
// Author : Tama
//
// UnityShader用
// 図形を描くための汎用関数をまとめたファイル
// 

#include "UnityCG.cginc"

// --------------------------------------------------
// 線を描く
// --------------------------------------------------
float4 lineShape(float2 uv, float s, float t)
{
    return step(t, uv.x) * step(abs(t - s), 1.0 - uv.x);
}