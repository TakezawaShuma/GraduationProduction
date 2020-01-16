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

    //その階層の先頭にする
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

    public void MyParentParentParentForeground()
    {
        this.transform.parent.parent.parent.SetAsLastSibling();
    }

    // インベントリから移動した後
    public void MyPointerUpInventory()
    {
        if (hitObject != null)
        {
            //ショートカットスロットに登録
            if (hitObject.tag == "Slot")
            {
                Overwrite();
            }

            //装備に登録
            if(hitObject.tag == "Accessory")
            {
                Overwrite();
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
        //入れ替えるか消すか判定
        bool initFlag = true;
        if (hitObject != null && hitObject.tag == "Slot")
        {
            //左クリックの場合入れ替える
            if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
            {
                initFlag = false;
            }
        }

        if (initFlag)
        {
            Init();
        }
        else
        {
            Swap();
        }
    }

    public void MyPointerUpAccessory()
    {
        //入れ替えるか消すか判定
        bool initFlag = true;
        if (hitObject != null && hitObject.tag == "Accessory")
        {
            //左クリックの場合入れ替える
            if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
            {
                initFlag = false;
            }
        }

        //データ移動
        if (initFlag)
        {
            Init();
        }
        else
        {
            Swap();
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
        if (this.name == hitObject.name) return;

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
        Debug.Log("Swap");
    }

    private void Init()
    {
        this.GetComponent<Image>().sprite = defoSprite;
        this.GetComponent<SlotData>().ID = -1;
        this.GetComponent<SlotData>().HOGE = SlotData.STATUS.NONE;
        Debug.Log("Init");
    }

    private void Overwrite()
    {
        hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
        hitObject.GetComponent<SlotData>().HOGE = this.GetComponent<SlotData>().HOGE;
        Debug.Log("Overwrite");
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
