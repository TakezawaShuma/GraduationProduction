using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Count : MonoBehaviour
{
    int score = 0;
    GameObject score_bord;

    //UPボタンに登録
    public void Up_Button()
    {
        score = score + 1;
        Debug.Log("あがった");
    }

    //DOWNボタンに登録
    public void Down_Button()
    {
        score = score - 1;
        Debug.Log("下がった");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
