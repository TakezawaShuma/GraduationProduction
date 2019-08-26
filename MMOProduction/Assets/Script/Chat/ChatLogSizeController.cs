using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatLogSizeController : MonoBehaviour, IDragHandler
{ 
    private RectTransform rect;

    public void OnDrag(PointerEventData e)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rect.sizeDelta = rect.sizeDelta + e.scrollDelta;
        }
    }
}
