using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIを動かす
/// </summary>
public class ReceiveEvent : MonoBehaviour
{

    private Vector3 clickMousePosition;
    private Vector3 clickThisObjectPosition;
    private Vector3 clickParentObjectPosition;

    private GameObject hitObject;

    [SerializeField]
    private Sprite defoSprite = null;

    // 押した時
    public void MyPointerDownUI()
    {
        clickMousePosition = Input.mousePosition;
        clickThisObjectPosition = this.transform.position;
        clickParentObjectPosition = this.transform.parent.position;
    }

    public void MyForeground()
    {
        this.transform.SetAsLastSibling();
    }

    public void MyParentForeground()
    {
        this.transform.parent.SetAsLastSibling();
    }

    public void MyParentParentForeground()
    {
        this.transform.parent.parent.SetAsLastSibling();
    }

    // インベントリから移動した後
    public void MyPointerUpInventory()
    {
        if (hitObject != null)
        {
            //ショートカットスロットに登録
            if (hitObject.tag == "Slot")
            {
                hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
                hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
            }

            if(hitObject.tag == "Accessory")
            {
                hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
                hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;

                WS.WsPlay.Instance.Send(new Packes.AccessoryChange(UserRecord.ID, this.GetComponent<SlotData>().ID, hitObject.GetComponent<SlotId>().id).ToJson());
            }

            //インベントリ内で入れ替え
            if (hitObject.tag == "Inventory")
            {
                Swap();
            }
        }
    }

    public void MyPointerUpShortcut()
    {
        if (hitObject != null && hitObject.tag == "Slot")
        {
            Swap();
        }
        else
        {
            this.GetComponent<Image>().sprite = defoSprite;
            this.GetComponent<SlotData>().ID = -1;
            this.GetComponent<SlotData>().HOGE = SlotData.HOGEID.NONE;
        }
    }

    public void MyPositionResetParent()
    {
        this.transform.position = clickParentObjectPosition;
    }

    public void MyPositionResetThis()
    {
        this.transform.position = clickThisObjectPosition;
    }

    public void MyPositionResetClick()
    {
        this.transform.position = clickMousePosition;
    }

    // ドラッグ時の関数
    public void MyDragUI()
    {
        if (InputManager.InputMouseCheck(0) == INPUT_MODE.UI)
        {
            this.transform.parent.transform.position = Input.mousePosition + (clickParentObjectPosition - clickMousePosition);
        }
    }

    public void MyDragContents()
    {
        if (InputManager.InputMouseCheck(0) == INPUT_MODE.UI)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    private void Swap()
    {
        GameObject temp = new GameObject();
        temp.AddComponent<Image>();
        temp.AddComponent<SlotData>();

        temp.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        temp.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
        temp.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;

        this.GetComponent<Image>().sprite = hitObject.GetComponent<Image>().sprite;
        this.GetComponent<SlotData>().ID = hitObject.GetComponent<SlotData>().ID;
        this.GetComponent<SlotData>().HOGE = hitObject.GetComponent<SlotData>().HOGE;

        hitObject.GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
        hitObject.GetComponent<SlotData>().ID = temp.GetComponent<SlotData>().ID;
        hitObject.GetComponent<SlotData>().HOGE = temp.GetComponent<SlotData>().HOGE;

        Destroy(temp);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot" 
            || collision.transform.tag == "Inventory" 
            || collision.transform.tag == "Accessory")
        {
            hitObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Slot" 
            || collision.transform.tag == "Inventory"
            || collision.transform.tag == "Accessory")
        {
            hitObject = null;
        }
    }
}
