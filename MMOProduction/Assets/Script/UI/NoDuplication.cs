using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoDuplication : MonoBehaviour
{
    [SerializeField]
    private GameObject[] accessorySlot = null;

    private GameObject[] cAccessorySlot = null;

    [SerializeField]
    private Sprite defaultSprite = null;
    // Start is called before the first frame update
    void Start()
    {
        cAccessorySlot = accessorySlot;
    }

    // Update is called once per frame
    void Update()
    {
        int index = -1;
        // 更新があるか調べる
        for (int i = 0; i < accessorySlot.Length; i++)
        {
            if (!IsHoge(accessorySlot[i].GetComponent<SlotData>(), cAccessorySlot[i].GetComponent<SlotData>()))
            {
                index = i;
                break;
            }
        }

        // 更新箇所と重複したデータが無いか探索し修正
        foreach (var slot in accessorySlot)
        {
            if (IsHoge(accessorySlot[index].GetComponent<SlotData>(), slot.GetComponent<SlotData>()))
            {
                slot.GetComponent<Image>().sprite = defaultSprite;
                slot.GetComponent<SlotData>().Init();
            }
        }

        // 更新前の物を持っておく
        cAccessorySlot = accessorySlot;
    }

    private bool IsHoge(SlotData slot, SlotData cSlot)
    {
        if (slot.Name == cSlot.Name)
        {
            return true;
        }

        return false;
    }
}
