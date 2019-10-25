using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenerateInventory : MonoBehaviour
{
    public GameObject Slot;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(-224, 200 - (10*48), 0);
        for (int i = 10; i > 0; i--)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject obj = Instantiate(Slot);
                obj.transform.parent = this.transform;
                obj.transform.localPosition = pos + new Vector3(j * 48, i * 48, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<Image>().color = new Color(Random.Range(0, 255)/255.0f, Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, 1);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //アイテムのデータ数によってスロットを変更する予定
    }
}
