using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Get_Point : MonoBehaviour
{
    public int m_point = 30;

    //Textのオブジェクト
    public GameObject point_object;

    Text point_text;
    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトからTextコンポーネントを取得
        point_text = point_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        point_text.text = "30";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set_Point(int set_point)
    {
        if ((set_point > 0 && (m_point < 30 && m_point >= 0)) || 
            (set_point < 0 && (m_point <= 30 && m_point > 0)))
        {
            m_point += set_point;
        }

        point_text.text = m_point.ToString();
    }
}
