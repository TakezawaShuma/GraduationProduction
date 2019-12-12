/*/////////////////////////////////////////////////////////
||
||	ファイル名：ItemData.cs
||
||	概要：アイテムデータのクラス
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
/// アイテムの基礎データ
/// </summary>
[System.Serializable]
public class ItemData
{
    public int     id;         // アイテムのID
    public int     iconId;     // アイコンのID
    public int     effectId;   // 使用するエフェクトのID
    public string  name;       // アイテムの名前
    public string  detail;       // 説明文
    public int     maxNum;     // 最大所持数
    public int     currentNum; // 現在所持数
    public int     price;      // 値段

    public ItemData(int id = 0, int iconId = 0, int effectId = 0, string name = "NoName", string detail = "NoText", int maxNum = 0, int currentNum = 0, int price = 0)
    {
        this.id         = id;
        this.iconId     = iconId;
        this.effectId   = effectId;
        this.name       = name;
        this.detail     = detail;
        this.maxNum     = maxNum;
        this.currentNum = currentNum;
        this.price      = price;
    }
}
