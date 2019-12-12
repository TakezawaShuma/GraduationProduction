////////////////////////
// チャット欄を動かす //
////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatLogMove : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData e)
    {
        if (e.position.y < Screen.height && e.position.y > 0 &&
            e.position.x < Screen.width  && e.position.x > 0)
        {
            transform.position = e.position;
        }
    }
}
