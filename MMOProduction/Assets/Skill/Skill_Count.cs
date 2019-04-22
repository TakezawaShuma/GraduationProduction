using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Count : MonoBehaviour
{
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
        
    }

    //UPボタンに登録
    public void Up_Button()
    {
        //スキル結果の表示
        if(get_point.m_point>0)
        {
            skill_number.Set_Score(+1);
        }
    }

    public void Up_ButtonP()
    {
        //マスターポイント結果の表示
        get_point.Set_Point(-1);
    }

    //DOWNボタンに登録
    public void Down_Button()
    {
        //スキル結果の表示
        skill_number.Set_Score(-1);
    }
    public void Down_ButtonP()
    {
        //マスターポイント結果の表示
        if (skill_number.m_score > 0)
        {
            get_point.Set_Point(+1);
        }
    }
}
