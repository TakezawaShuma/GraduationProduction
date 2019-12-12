/*/////////////////////////////////////////////////////////
||
||	ファイル名：Status.cs
||
||	概要：ステータスの構造体
||
||	作成者：杉浦諒
||
||	作成日：2019/10/17
||
||	更新日：2019/10/17
||
*//////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステータスの基礎データ
/// </summary>
[System.Serializable]
public struct Status
{
    public int hitPoint;    // 体力ポイント
    public int magicPoint;  // 魔法ポイント
    public int strength;    // 筋力
    public int vitality;    // 持続力
    public int intelligence;// 知性
    public int mind;        // 精神力
    public int dexterity;   // 器用さ
    public int agility;     // 俊敏さ

    public Status(int hp = 0, int mp = 0, int str = 0, int vit = 0, int inte = 0, int min = 0, int dex = 0, int agi = 0)
    {
        hitPoint = hp;
        magicPoint = mp;
        strength = str;
        vitality = vit;
        intelligence = inte;
        mind = min;
        dexterity = dex;
        agility = agi;
    }
}
