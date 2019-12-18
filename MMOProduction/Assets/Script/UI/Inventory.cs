using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    //管理系
    [SerializeField]
    private List<GameObject> list = null;

    //生成に必要な物群
    [SerializeField]
    private GameObject Slot = null;

    [SerializeField]
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        GenerateInventory();
    }

    void Update()
    {
        foreach(var a in list)
        {
            if(a.GetComponent<SlotData>().GetUseFlag())
            {
                //アイテムを使う
                var b = a.GetComponent<SlotData>().HOGE;
                var c = a.GetComponent<SlotData>().ID;


                //使った報告
                a.GetComponent<SlotData>().UseFlagOff();
            }
        }
    }

    private void GenerateInventory()
    {
        Vector3 pos = new Vector3((-240 + 8 + 8), 265 - 8 - 50 - 8 - 24 - 24, 0);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject obj = Instantiate(Slot);
                obj.transform.parent = this.transform;
                obj.transform.localPosition = pos + new Vector3(j * 48, i * -48, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<Image>().sprite = sprite;

                list.Add(obj);
            }
        }
    }
}
