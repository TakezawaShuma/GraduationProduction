//
// MyMath.cs
//
// Author: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 汎用計算関数
/// </summary>
public class MyMath
{
    // -------------------------------------
    // 指定範囲内に数値を収める
    // min: 最小値
    // max: 最大値
    // num: 変更する数値
    // return: min～maxまでの値を返す
    // -------------------------------------
    public static float RangeFloat(float min, float max, float num)
    {
        return Mathf.Min(max, Mathf.Max(min, num));
    }
}
