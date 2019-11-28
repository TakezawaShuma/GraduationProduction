/*/////////////////////////////////////////////////////////
||
||	ファイル名：ConsumablesData.cs
||
||	概要：消費アイテムデータのクラス
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
/// 消費アイテムの基礎データ
/// </summary>
[System.Serializable]
public class ConsumablesData : ItemData
{
    // 効果対象
    public enum Target
    {
        Myself,     // 自分自身
        Extent,     // 範囲
        Selection,  // 選択
        None
    }


    // 消費アイテムのタイプ
    public enum Type
    {
        Recovery,   // 回復
        Buff,       // バフ
        Transition, // 転移
        None,
    }

    public Status   effect; // 効果量
    public Target   target; // 効果対象
    public Type     type;   // 効果のタイプ
    public int      time;   // 効果時間

    public ConsumablesData(int id = 0, int iconId = 0, int effectId = 0, string name = "NoName", string detail = "NoText", int maxNum = 0, int currentNum = 0, int price = 0, Status effect = new Status(), Target target = Target.None, Type type = Type.None, int time = 0)
    {
        this.id = id;
        this.iconId = iconId;
        this.effectId = effectId;
        this.name = name;
        this.detail = detail;
        this.maxNum = maxNum;
        this.currentNum = currentNum;
        this.price = price;
        this.effect = effect;
        this.target = target;
        this.type = type;
        this.time = time;
    }
}
