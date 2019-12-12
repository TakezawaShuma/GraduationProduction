/*/////////////////////////////////////////////////////////
||
||	ファイル名：ConsumablesList.cs
||
||	概要：消費アイテムリストクラス
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
/// 消費アイテムリストを管理するクラス
/// </summary>
public class ConsumablesList
{
    private Dictionary<int, ConsumablesData> consumablesList;

    // 消費アイテムリストを登録
    public void RegisterItemList(Dictionary<int, ConsumablesData> consumablesData)
    {
        this.consumablesList = consumablesData;
    }

    // 消費アイテムをリストに登録
    public void RegisterItem(ConsumablesData consumables)
    {
        consumablesList[consumables.id] = consumables;
    }

    // 消費アイテムをIDで検索
    public ItemData SearchItem(int id)
    {
        return consumablesList[id];
    }
}
