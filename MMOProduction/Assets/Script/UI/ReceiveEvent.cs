using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveEvent : MonoBehaviour
{
    private Vector3 startPosition;
    GameObject hitObject;

    private Vector3 length;
    // 押した時
    public void MyPointerDownUI()
    {
        length = Input.mousePosition - this.transform.position;
        startPosition = this.transform.position;
    }

    // 離した時
    public void MyPointerUpUI()
    {
        if (hitObject != null)
        {
            if(hitObject.tag == "Slot")
            {
                hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
                hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
            }

            if(hitObject.tag == "Inventory")
            {
                GameObject temp = hitObject;
                temp.AddComponent<Image>();

                temp.GetComponent<Image>().sprite = hitObject.GetComponent<Image>().sprite;
                temp.GetComponent<SlotData>().ID = hitObject.GetComponent<SlotData>().ID;
                temp.GetComponent<SlotData>().HOGE = hitObject.GetComponent<SlotData>().HOGE;

                hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
                hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;

                this.GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
                this.GetComponent<SlotData>().ID = temp.GetComponent<SlotData>().ID;
                this.GetComponent<SlotData>().HOGE = temp.GetComponent<SlotData>().HOGE;
            }
        }

        this.transform.position = startPosition;
    }

    // ドラッグ時の関数
    public void MyDragUI()
    {
        if(hitObject.GetComponent<SlotData>().ID != -1)
        {
            transform.position = Input.mousePosition - length;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot" || collision.transform.tag == "Inventory")
        {
            hitObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot" || collision.transform.tag == "Inventory")
        {
            hitObject = null;
        }
    }
}
