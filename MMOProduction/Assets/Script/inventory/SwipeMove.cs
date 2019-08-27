﻿//////////////////////////////////
// D＆Dで操作できるようにした？ //
//////////////////////////////////


using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // ドラックが開始したとき呼ばれる.
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(1);
        if (eventData.button != PointerEventData.InputButton.Left) return;
    }

    // ドラック中に呼ばれる.
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        this.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }

    // ドラックが終了したとき呼ばれる.
    public void OnEndDrag(PointerEventData eventData)
    {

    }


}