using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIを動かす
/// </summary>
public class ReceiveEvent : MonoBehaviour
{

    private Vector3 clickPosition;

    private GameObject hitObject;

    // 押した時
    public void MyPointerDownUI()
    {
        clickPosition = this.transform.position;
    }

    // 離した時
    public void MyPointerUpUI()
    {
        if (hitObject != null)
        {
            //ショートカットスロットに登録
            if(hitObject.tag == "Slot")
            {
                hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
                hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
            }

            //インベントリ内で入れ替え
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

        //初期位置に戻す
        this.transform.position = clickPosition;
    }

    // ドラッグ時の関数
    public void MyDragUI()
    {
        if(Input.GetMouseButton(0))
        {
            this.transform.parent.transform.position = Input.mousePosition;
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
