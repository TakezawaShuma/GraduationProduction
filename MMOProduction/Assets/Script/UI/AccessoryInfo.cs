using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryInfo : MonoBehaviour
{
    [SerializeField]
    private Text[] status;

    private string[] statusName;

    private int hitID = -1;
    // Start is called before the first frame update
    void Start()
    {
        //配列の長さ取得
        int length = status.Length;

        //初期の名前を入れておく
        //HPなら"HP",STRなら"STR"
        statusName = new string[length];

        for (int i = 0; i < length; i++)
        {
            statusName[i] = status[i].text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition + new Vector3(160, -200, 0);

        if (hitID != -1 && hitID != 0)
        {
            int count = this.transform.childCount;

            string[] st = new string[count];

            //ステータス更新
            status[0].text = statusName[0] + AccessoryDatas.FindOne(hitID).name; //装備名
            status[1].text = statusName[1] + AccessoryDatas.FindOne(hitID).hp.ToString();//hp
            status[2].text = statusName[2] + AccessoryDatas.FindOne(hitID).mp.ToString();//mp
            status[3].text = statusName[3] + AccessoryDatas.FindOne(hitID).str.ToString();//str
            status[4].text = statusName[4] + AccessoryDatas.FindOne(hitID).vit.ToString();//vit
            status[5].text = statusName[5] + AccessoryDatas.FindOne(hitID).vit.ToString();//vit
            status[6].text = statusName[6] + AccessoryDatas.FindOne(hitID).mmd.ToString();//mnd?
            status[7].text = statusName[7] + AccessoryDatas.FindOne(hitID).dex.ToString();//dex
            status[8].text = statusName[8] + AccessoryDatas.FindOne(hitID).agi.ToString();//agi
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitID = collision.GetComponent<SlotData>().ID;
        if (hitID != -1 && hitID != 0)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.SetAsLastSibling();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitID = -1;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
