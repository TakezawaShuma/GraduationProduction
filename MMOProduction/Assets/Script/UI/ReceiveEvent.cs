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
        length = this.transform.position - Input.mousePosition;


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
        transform.position = Input.mousePosition + length;
    }

    public void MyDragMoveChild()
    {
        transform.GetChild(0).position = Input.mousePosition + length;
    }


    // Imageのalpha値の変更
    public void ChangeAlpha(float alpha)
    {
        var component = GetComponent<Image>();
        var color = component.color;
        color.a = alpha;
        component.color = color;
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
