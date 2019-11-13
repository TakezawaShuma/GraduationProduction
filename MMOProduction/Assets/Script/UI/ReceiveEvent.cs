using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveEvent : MonoBehaviour
{
    private Vector3 startPosition;
    private GameObject hitObject;
    private Vector3 length;

    [SerializeField]
    private Sprite initImage;
    // 押した時
    public void MyPointerDownUI()
    {
        length = Input.mousePosition - this.transform.position;
        

        startPosition = this.transform.position;
        ChangeAlpha(0.5f);
    }

    // 離した時
    public void MyPointerUpUI()
    {
        ChangeAlpha(1.0f);

        if (hitObject != null)
        {
            hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
            hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
        }

        this.transform.position = startPosition;
    }

    public void MyPointerUpUIBack()
    {
        ChangeAlpha(1.0f);
    }

    // ドラッグ時の関数
    public void MyDragUI()
    {
        transform.position = Input.mousePosition- length;
    }

    public void MyDragMoveChild()
    {
        transform.GetChild(0).localPosition = Input.mousePosition + length;
    }


    // Imageのalpha値の変更
    public void ChangeAlpha(float alpha)
    {
        var component = GetComponent<Image>();
        var color = component.color;
        color.a = alpha;
        component.color = color;
    }

    public void MyPointerUpSlot()
    {
        this.transform.position = startPosition;
        if(hitObject == null)
        {
            hitObject.GetComponent<Image>().sprite = initImage;
            hitObject.GetComponent<SlotData>().ID = -1;
            hitObject.GetComponent<SlotData>().HOGE = SlotData.HOGEID.NONE;
        }
        else if (hitObject != null)
        {
            hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
            hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot")
        {
            hitObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot")
        {
            hitObject = null;
        }
    }
}
