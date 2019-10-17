/*/////////////////////////////////////////////////////////
||
||	ファイル名：ItemData.cs
||
||	概要：アイテムデータの構造体
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

[System.Serializable]
public struct ItemData
{
    private int     id;         // アイテムのID
    private int     iconId;     // アイコンのID
    private string  name;       // アイテムの名前
    private string  detail;       // 説明文
    private int     maxNum;     // 最大所持数
    private int     currentNum; // 現在所持数
    private int     price;      // 値段

    public ItemData(int id = 0, int iconId = 0, string name = "NoName", string detail = "NoText", int maxNum = 0, int currentNum = 0, int price = 0)
    {
        this.id         = id;
        this.iconId     = iconId;
        this.name       = name;
        this.detail     = detail;
        this.maxNum     = maxNum;
        this.currentNum = currentNum;
        this.price      = price;
    }
}
