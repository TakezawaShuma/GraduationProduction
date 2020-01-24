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

    private bool leftClickFlag = false;
    private bool rightClickFlag = false;

    [SerializeField]
    private Sprite defoSprite = null;

    /// <summary>
    /// 押した時の情報を保存する関数
    /// </summary>
    public void MyPointerDownUI()
    {
        clickMousePosition = Input.mousePosition;
        clickThisObjectPosition = this.transform.position;
        clickParentObjectPosition = this.transform.parent.position;

        leftClickFlag = false;
        rightClickFlag = false;

        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.UI)
        {
            leftClickFlag = true;
        }
        if (InputManager.InputMouseCheckDown(1) == INPUT_MODE.UI)
        {
            rightClickFlag = true;
        }
    }
    /// <summary>
    ///その階層内で描画順を一番上にする
    /// </summary>
    public void MyForeground()
    {
        this.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 上の親版
    /// </summary>
    public void MyParentForeground()
    {
        this.transform.parent.SetAsLastSibling();
    }

    /// <summary>
    /// 更に親版
    /// </summary>
    public void MyParentParentForeground()
    {
        this.transform.parent.parent.SetAsLastSibling();
    }

    public void MyParentParentParentForeground()
    {
        this.transform.parent.parent.parent.SetAsLastSibling();
    }

    /// <summary>
    /// インベントリから移動
    /// </summary>
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
            if (hitObject.tag == "Accessory")
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

    /// <summary>
    /// ショートカットから移動
    /// </summary>
    public void MyPointerUpShortcut()
    {
        //入れ替えるか消すか判定
        bool initFlag = true;

        //ショートカット内に入っているか
        if (hitObject != null && hitObject.tag == "Slot")
        {
            //左クリックの場合入れ替える
            if (leftClickFlag)
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

    /// <summary>
    /// アクセサリーから移動
    /// </summary>
    public void MyPointerUpAccessory()
    {
        //入れ替えるか消すか判定
        bool initFlag = true;
        if (hitObject != null && hitObject.tag == "Accessory")
        {
            //左クリックの場合入れ替える
            if (leftClickFlag)
            {
                initFlag = false;
            }
        }

        //データ移動
        if (initFlag)
        {
            Init();
            WS.WsPlay.Instance.Send(new Packes.AccessoryChange(UserRecord.ID, 0, this.GetComponent<SlotId>().id).ToJson());
        }
        else
        {

            WS.WsPlay.Instance.Send(new Packes.AccessoryChange(UserRecord.ID, this.GetComponent<SlotData>().ID, hitObject.GetComponent<SlotId>().id).ToJson());
            WS.WsPlay.Instance.Send(new Packes.AccessoryChange(UserRecord.ID, hitObject.GetComponent<SlotData>().ID, this.GetComponent<SlotId>().id).ToJson());
            Swap();
           
        }
    }

    /// <summary>
    /// 親の座標に戻す
    /// </summary>
    public void MyPositionResetParent()
    {
        this.transform.position = clickParentObjectPosition;
    }

    /// <summary>
    /// 自身の座標に戻す
    /// </summary>
    public void MyPositionResetThis()
    {
        this.transform.position = clickThisObjectPosition;
    }

    /// <summary>
    /// マウスクリック時の座標に戻す
    /// </summary>
    public void MyPositionResetClick()
    {
        this.transform.position = clickMousePosition;
    }

    // 親を移動させるドラッグ時の関数
    public void MyDragUI()
    {
        this.transform.parent.transform.position = Input.mousePosition + (clickParentObjectPosition - clickMousePosition);
    }

    /// <summary>
    /// 自身のみを移動させる
    /// </summary>
    public void MyDragContents()
    {
        this.transform.position = Input.mousePosition;
    }

    private void Swap()
    {
        if (this.transform.parent.name == hitObject.transform.parent.name) return;

        var tempSprite = this.GetComponent<Image>().sprite;
        var tempId = this.GetComponent<SlotData>().ID;
        var tempStatus = this.GetComponent<SlotData>().Status;
        var tempName = this.GetComponent<SlotData>().Name;

        this.GetComponent<Image>().sprite = hitObject.GetComponent<Image>().sprite;
        this.GetComponent<SlotData>().ID = hitObject.GetComponent<SlotData>().ID;
        this.GetComponent<SlotData>().Status = hitObject.GetComponent<SlotData>().Status;
        this.GetComponent<SlotData>().Name = hitObject.GetComponent<SlotData>().Name;

        hitObject.GetComponent<Image>().sprite = tempSprite;
        hitObject.GetComponent<SlotData>().ID = tempId;
        hitObject.GetComponent<SlotData>().Status = tempStatus;
        hitObject.GetComponent<SlotData>().Name = tempName;
    }

    private void Init()
    {
        this.GetComponent<Image>().sprite = defoSprite;
        this.GetComponent<SlotData>().ID = 0;
        this.GetComponent<SlotData>().Status = SlotData.STATUS.NONE;
        this.GetComponent<SlotData>().Name = this.transform.parent.name;
    }

    private void Overwrite()
    {
        hitObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        hitObject.GetComponent<SlotData>().ID = this.GetComponent<SlotData>().ID;
        hitObject.GetComponent<SlotData>().Status = this.GetComponent<SlotData>().Status;
        hitObject.GetComponent<SlotData>().Name = this.GetComponent<SlotData>().Name;
    }

    private bool IsSameName()
    {
        if (hitObject == null) return false;

        if (this.transform.parent.name == hitObject.transform.parent.name) return true;
        return false;
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
