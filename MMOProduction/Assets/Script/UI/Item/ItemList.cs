/*/////////////////////////////////////////////////////////
||
||	ファイル名：ItemList.cs
||
||	概要：アイテムリストクラス
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
/// アイテムのマスターデータ
/// </summary>
public class ItemList
{
    private Dictionary<int, ItemData> itemList;

    // アイテムリストを登録
    public void RegisterItemList(Dictionary<int, ItemData> itemList)
    {
        this.itemList = itemList;
    }

    // アイテムをリストに登録
    public void RegisterItem(ItemData item)
    {
        itemList[item.id] = item;
    }

    // アイテムをIDで検索
    public ItemData SearchItem(int id)
    {
        return itemList[id];
    }
}
