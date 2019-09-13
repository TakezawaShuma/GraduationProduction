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
        transform.position = e.position;
    }
}
