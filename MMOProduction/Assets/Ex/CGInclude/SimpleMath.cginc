//
// SimpleMath.cginc
//
// Author : Tama
//
// UnityShader用
// 汎用計算関数をまとめたファイル
//

#include "UnityCG.cginc"

// -------------------------------------------
// ランダム関数
// -------------------------------------------
float random(fixed2 p) 
{ 
    return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
}

float2 random2(float2 st)
{
    float x = dot(st,fixed2(127.1,311.7));
    float y = dot(st,fixed2(269.5,183.3));
    st = float2(x, y);
    return -1.0 + 2.0*frac(sin(st)*43758.5453123);
}

// -------------------------------------------
// グレースケールに変換
// -------------------------------------------
// グレースケールに変換
float toGray(float4 c)
{
	return (c.r * 0.3 + c.g * 0.6 + c.b * 0.1);
}
