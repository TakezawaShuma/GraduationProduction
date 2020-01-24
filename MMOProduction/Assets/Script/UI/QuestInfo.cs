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

    private string questName = "";

    private string detail = "";

    // Start is called before the first frame update
    void Start()
    {
        questNameText.text = questName;
        detailText.text = detail;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            Packes.QuestMasterData data = QuestDatas.FindOne(UserRecord.QuestID);
            if (data.id != 0)
            {
                QuestName(data);
                Detail(data);
                questNameText.text = questName;
                detailText.text = detail;
            }

            if (timeHidden)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= hiddenTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                currentTime = 0;
            }
        }
    }

    private void QuestName(Packes.QuestMasterData data)
    {
        questName = data.name;
    }

    private void Detail(Packes.QuestMasterData data)
    {
        detail = data.comment;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        currentTime = 0;
    }

    public void Pin()
    {
        timeHidden = !timeHidden;
    }
}
