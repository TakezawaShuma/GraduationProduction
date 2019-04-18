using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Count : MonoBehaviour
{
    int score = 0;

    public GameObject score_bord;

    //スキルナンバーのスクリプトが入る変数
    Skill_Number skill_number;

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトからスキルナンバーを所得
        skill_number = score_bord.GetComponent<Skill_Number>();
    }

    // Update is called once per frame
    void Update()
    {
        //結果の表示
        skill_number.Set_Score(score);
    }

    //UPボタンに登録
    public void Up_Button()
    {
        score = score + 1;
        Debug.Log("あがった : " + score.ToString());
    }

    //DOWNボタンに登録
    public void Down_Button()
    {
        if (0 < score)
        {
            score = score - 1;
        }
        Debug.Log("下がった : " + score.ToString());
    }
}
