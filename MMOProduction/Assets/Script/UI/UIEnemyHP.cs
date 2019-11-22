using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHP : MonoBehaviour
{
    [SerializeField, Header("キャンバス")]
    private GameObject canvas = null;

    [SerializeField, Header("スライダー")]
    private Slider slider = null;

    [SerializeField, Header("テスト用テキスト")]
    private Text text = null;

    private float maxHP = 100;

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
        canvas.SetActive(true);
    }

    public void Off()
    {
        canvas.SetActive(false);
    }
}
