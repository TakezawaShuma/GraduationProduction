using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Number : MonoBehaviour
{
    //Textのオブジェクト
    public GameObject score_object = null;

    
    Text score_text;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトからTextコンポーネントを取得
       score_text = score_object.GetComponent<Text>();
        // テキストの表示を入れ替える
       score_text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スキルカウントスクリプトに呼び出す関数
    public void Set_Score(int score)
    {
        score_text.text = score.ToString();
    }
}
