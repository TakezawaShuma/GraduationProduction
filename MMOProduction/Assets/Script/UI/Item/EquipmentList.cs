/*/////////////////////////////////////////////////////////
||
||	ファイル名：EquipmentList.cs
||
||	概要：装備リストクラス
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

public class EquipmentList
{
    private Dictionary<int, EquipmentData> equipmentList;

    // 装備リストを登録
    public void RegisterItemList(Dictionary<int, EquipmentData> equipmentList)
    {
        this.equipmentList = equipmentList;
    }

    // 装備をリストに登録
    public void RegisterItem(EquipmentData equipment)
    {
        equipmentList[equipment.id] = equipment;
    }

    // 装備をIDで検索
    public ItemData SearchItem(int id)
    {
        return equipmentList[id];
    }
}
