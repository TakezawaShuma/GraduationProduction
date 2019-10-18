/*/////////////////////////////////////////////////////////
||
||	ファイル名：EquipmentData.cs
||
||	概要：装備データのクラス
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

public class EquipmentData : ItemData
{
    // 武器種
    public enum Category
    {
        ShieldAndSword, // 盾剣
        TwoHandedSword, // 両手剣
        Katana,         // 刀
        Fist,           // 拳
        Spear,          // 槍
        DualBlades,     // 双剣
        Bow,            // 弓
        MagicWand,      // 魔杖
        Grimoire,       // 魔導書
        HolyWand,       // 聖杖
        None,
    }

    // 装備部位
    public enum Part
    {
        Hand,           // 手(武器)
        Head,           // 頭
        Body,           // 胴体
        Arm,            // 腕
        Leg,            // 脚
        RightBangle,    // 右腕輪
        LeftBangle,     // 左腕輪
        None,
    }

    // 装備できる職業
    public enum Job
    {
        Tank,       // タンク
        Attacker,   // アタッカー
        Healer,     // ヒーラー
        None,
    }

    public Status      upStatus;            // 上昇ステータス
    public Category    category;            // カテゴリー
    public Part        part;                // 装備部位
    public Job         job;                 // 装備できる職業
    public int         additionalEffects;   // 追加効果

    public EquipmentData(int id = 0, int iconId = 0, int effectId = 0, string name = "NoName", string detail = "NoText", int maxNum = 0, int currentNum = 0, int price = 0, Status upStatus = new Status(), Category category = Category.None, Part part = Part.None, Job job = Job.None, int additionalEffects = 0)
    {
        this.id = id;
        this.iconId = iconId;
        this.effectId = effectId;
        this.name = name;
        this.detail = detail;
        this.maxNum = maxNum;
        this.currentNum = currentNum;
        this.price = price;
        this.upStatus = upStatus;
        this.category = category;
        this.part = part;
        this.job = job;
        this.additionalEffects = additionalEffects;
    }
}
