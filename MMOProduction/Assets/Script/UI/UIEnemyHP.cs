﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーのHPを表示させる
/// </summary>
public class UIEnemyHP : MonoBehaviour
{
    [SerializeField, Header("パネル")]
    private GameObject panel = null;

    [SerializeField, Header("クエスト対象マーク")]
    private GameObject targetMark = null;

    [SerializeField, Header("スライダー")]
    private Slider slider = null;

    [SerializeField, Header("テスト用テキスト")]
    private Text text = null;

    private int enemyID = 0;

    private float maxHP = 100;

    private void Update()
    {
        Packes.QuestMasterData data = QuestDatas.FindOne(UserRecord.QuestID);

        if(enemyID == data.targetId)
        {
            TargetMarkON();
        }
        else
        {
            TargetMarkOFF();
        }
    }

    public void SetEnemyID(int id)
    {
        enemyID = id;
    }

    public float MAX_HP
    {
        set
        {
            maxHP = value;
            slider.maxValue = maxHP;
        }
    }

    public void UpdateHP(float hp)
    {
        slider.value = hp;
        text.text = hp.ToString();
    }

    public void On()
    {
        panel.SetActive(true);
    }

    public void Off()
    {
        panel.SetActive(false);
    }

    public void TargetMarkON()
    {
        targetMark.SetActive(true);
    }

    public void TargetMarkOFF()
    {
        targetMark.SetActive(false);
    }
}
