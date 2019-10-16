using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveEvent : MonoBehaviour
{
    private Vector3 startPosition;
    GameObject hitObject;

    // 押した時
    public void MyPointerDownUI()
    {
        startPosition = this.transform.position;
        ChangeAlpha(0.2f);
    }

    // 離した時
    public void MyPointerUpUI()
    {
        if (hitObject != null)
        {
            //hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            hitObject.GetComponent<Image>().color = this.GetComponent<Image>().color;
        }
        this.transform.position = startPosition;
        ChangeAlpha(1.0f);
    }

    // ドラッグ時の関数
    public void MyDragUI()
    {
        transform.position = Input.mousePosition;
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
            Debug.Log("UI衝突");
            hitObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot")
        {
            Debug.Log("UI衝突");
            hitObject = null;
        }
    }
}
