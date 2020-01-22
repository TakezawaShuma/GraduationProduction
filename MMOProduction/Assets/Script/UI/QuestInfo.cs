using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [SerializeField, Header("時間経過で消すか")]
    private bool timeHidden = true;

    [SerializeField, Header("何秒で消すか")]
    private float hiddenTime = 5f;

    [SerializeField, Header("クエスト名テキスト")]
    private Text questNameText;

    [SerializeField, Header("詳細テキスト")]
    private Text detailText;

    private float currentTime = 0;

    private string questName = "クエスト名";

    private string monsterName = "モンスター名";

    private int maxKillNum = 10;

    private int currentKillNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        questNameText.name = "";
        detailText.text = DetailTextGenerate();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            questNameText.name = "";
            detailText.text = DetailTextGenerate();
            currentTime += Time.deltaTime;

            if(currentTime >= hiddenTime)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            currentTime = 0;
        }
    }

    private string DetailTextGenerate()
    {
        return monsterName + "を倒す(" + currentKillNum + "/" + maxKillNum + ")";
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }
}
