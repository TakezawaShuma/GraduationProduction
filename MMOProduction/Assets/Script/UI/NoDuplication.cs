using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoDuplication : MonoBehaviour
{
    [SerializeField]
    private GameObject[] accessorySlot = null;

    private string[] cAccessorySlot = null;

    [SerializeField]
    private Sprite defaultSprite = null;
    // Start is called before the first frame update
    void Start()
    {

        cAccessorySlot = new string[accessorySlot.Length];

        for (int i = 0; i < accessorySlot.Length; i++)
        {
            cAccessorySlot[i] = accessorySlot[i].GetComponent<SlotData>().Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int index = -1;
        // 更新があるか調べる
        for (int i = 0; i < accessorySlot.Length; i++)
        {
            if (!IsHoge(accessorySlot[i].GetComponent<SlotData>().Name, cAccessorySlot[i]))
            {
                index = i;
                break;
            }
        }

        if (index < 0)
        {
            Current();
            return;
        }

        // 更新箇所と重複したデータが無いか探索し修正
        for (int i = 0; i < accessorySlot.Length; i++)
        {
            if (i != index)
            {
                if (IsHoge(accessorySlot[index].GetComponent<SlotData>().Name, accessorySlot[i].GetComponent<SlotData>().Name))
                {
                    accessorySlot[i].GetComponent<Image>().sprite = defaultSprite;
                    accessorySlot[i].GetComponent<SlotData>().Init();
                }
            }
        }

        // 更新前の物を持っておく
        Current();
    }

    private bool IsHoge(string name1, string name2)
    {
        if (name1 == name2)
        {
            return true;
        }

        return false;
    }

    private void Current()
    {
        for (int i = 0; i < accessorySlot.Length; i++)
        {
            cAccessorySlot[i] = accessorySlot[i].GetComponent<SlotData>().Name;
        }
    }
}
