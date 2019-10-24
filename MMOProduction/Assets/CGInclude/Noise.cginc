//
// Noise.cginc
//
// Author : Tama
//
// UnityShader用
// ノイズ表現用の汎用関数をまとめたファイル
//

#include "UnityCG.cginc"
#include "SimpleMath.cginc"

// -----------------------------------------------
// ブロックノイズ
// -----------------------------------------------
float blockNoise(float2 st)
{
    float2 p = floor(st);
    return random(p);
}

// -----------------------------------------------
// バリューノイズ
// -----------------------------------------------
float valueNoise(float2 st)
{
    float2 p = floor(st);
    float2 f = frac(st);

    float v00 = random(p+float2(0,0));
    float v10 = random(p+float2(1,0));
    float v01 = random(p+float2(0,1));
    float v11 = random(p+float2(1,1));
    
    float2 u = f * f * (3.0 - 2.0 * f);            

    float v0010 = lerp(v00, v10, u.x);
    float v0111 = lerp(v01, v11, u.x);
    return lerp(v0010, v0111, u.y);
}

// -----------------------------------------------
// パーリンノイズ
// -----------------------------------------------
float perlinNoise(float2 st) 
{
    float2 p = floor(st);
    float2 f = frac(st);
    float2 u = f*f*(3.0-2.0*f);

    float v00 = random2(p+float2(0,0));
    float v10 = random2(p+float2(1,0));
    float v01 = random2(p+float2(0,1));
    float v11 = random2(p+float2(1,1));

    return lerp(lerp(dot(v00, f - float2(0,0)), dot(v10, f - float2(1,0)), u.x),
                    lerp(dot(v01, f - float2(0,1)), dot(v11, f - float2(1,1)), u.x), 
                    u.y)+0.5f;
}

// -----------------------------------------------
// fbmノイズ
// -----------------------------------------------
float fBm (fixed2 st) 
{
    float f = 0;
    float2 q = st;

    f += 0.5000*perlinNoise( q ); q = q*2.01;
    f += 0.2500*perlinNoise( q ); q = q*2.02;
    f += 0.1250*perlinNoise( q ); q = q*2.03;
    f += 0.0625*perlinNoise( q ); q = q*2.01;

    return f;
}
