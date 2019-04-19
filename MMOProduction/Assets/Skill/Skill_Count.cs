using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Count : MonoBehaviour
{
    int score;

    int score_p = 30; 

    public GameObject score_bord;

    public GameObject point_bord;

    //スキルナンバーのスクリプトが入る変数
    Skill_Number skill_number;

    //スキルカウントのスクリプトが入る変数
    Get_Point get_point;

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトからスキルナンバーを所得
        skill_number = score_bord.GetComponent<Skill_Number>();
        //オブジェクトからスキルカウントを所得
        get_point = point_bord.GetComponent<Get_Point>();
    }

    // Update is called once per frame
    void Update()
    {
        //スキル結果の表示
        skill_number.Set_Score(score);
        //マスターポイント結果の表示
        get_point.Set_Point(score_p);
    }

    //UPボタンに登録
    public void Up_Button()
    {
        score = score + 1;
        Debug.Log("あがった : " + score.ToString());
    }

    public void Up_ButtonP()
    {
        if (0 < score_p)
        {
            score_p = score_p - 1;
        }
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
    public void Down_ButtonP()
    {
        if (score_p < 30)
        {
            score_p = score_p + 1;
        }
    }
}
