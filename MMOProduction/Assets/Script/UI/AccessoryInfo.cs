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
        int length = status.Length;
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

        if (hitID != -1)
        {
            int count = this.transform.childCount;

            string[] st = new string[count];

            //ステータス更新
            status[0].text = statusName + AccessoryDatas.FindOne(hitID).name.ToString(); ; //装備名
            status[1].text = statusName + AccessoryDatas.FindOne(hitID).hp.ToString(); ; //hp
            status[2].text = statusName + AccessoryDatas.FindOne(hitID).mp.ToString(); ; //mp
            status[3].text = statusName + AccessoryDatas.FindOne(hitID).str.ToString(); ; //str
            status[4].text = statusName + AccessoryDatas.FindOne(hitID).vit.ToString(); ; //vit
            status[5].text = statusName + AccessoryDatas.FindOne(hitID).vit.ToString(); ; //vit
            status[6].text = statusName + AccessoryDatas.FindOne(hitID).mmd.ToString(); ; //mnd?
            status[7].text = statusName + AccessoryDatas.FindOne(hitID).dex.ToString(); ; //dex
            status[8].text = statusName + AccessoryDatas.FindOne(hitID).agi.ToString(); ; //agi
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitID = collision.GetComponent<SlotData>().ID;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.SetAsLastSibling();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitID = -1;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
