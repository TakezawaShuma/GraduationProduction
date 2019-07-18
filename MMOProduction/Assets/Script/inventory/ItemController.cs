////////////////////////////////
// アイテムを制御するクラス？ //
////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
#pragma warning disable 0649
    //アイテムの種類
    public enum ITEM_TYPE
    {
        ALL,               //すべて
        CONSUMPTION,    //消費
        WEAPON,         //武器
        ARMOR,          //防具
        OTHER,           //その他
    }
    //ボタン用Canvas
    [SerializeField, Header("buttonCanvas")]
    private Canvas buttonCanvas;
    //アイテムの種類を選択するボタン
    [SerializeField, Header("すべて")]
    private Button allBtn_;
    [SerializeField, Header("消費アイテム")]
    private Button consumptionBtn_;
    [SerializeField, Header("武器")]
    private Button weaponBtn_;
    [SerializeField, Header("防具")]
    private Button armorBtn_;
    [SerializeField, Header("その他")]
    private Button otherBtn_;


    [SerializeField, Header("アイテム表示用ボタン")]
    private Button button_;
    [SerializeField, Header("パネル")]
    private Image panel_;
    [SerializeField, Header("スクロールバー")]
    private Scrollbar scrollbar_;
    //セレクトパネル
    [SerializeField, Header("セレクトされたアイテムを表示用パネル")]
    private Image selectItempanel_;
    [SerializeField, Header("アイテム名")]
    private Text selectItemName_;
    [SerializeField, Header("説明文")]
    private Text selectItemDetail_;
    private int nowSelectId_ = 0;
    private bool openInventory_ = false;
    ColorBlock selectColor_;
    ColorBlock OtherColor_;

    private List<Item> itemList_ = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        selectColor_ = allBtn_.colors;
        selectColor_.pressedColor = selectColor_.normalColor;
        OtherColor_ = weaponBtn_.colors;

        //インベントリテスト用
        for(int i=0;i<399;i++)
        {
            int id = (i + 1) * 100 + 1;
            id += 10000;
            string type = "";
            int itemType = id / 100;
            if(100 <= itemType && itemType <= 199)
            {
                type = "消費";
            }
            if (200 <= itemType && itemType <= 299)
            {
                type = "武器";
            }
            if (300 <= itemType && itemType <= 399)
            {
                type = "防具";
            }
            if (400 <= itemType && itemType <= 499)
            {
                type = "その他";
            }
            itemList_.Add(new Item("分類 : " +type+ "アイテム名   ID : " + itemType, "",id));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && openInventory_ == false)
        {
            SortItem(ITEM_TYPE.ALL);
            openInventory_ = true;
        }
        if (Input.GetKeyDown(KeyCode.U) && openInventory_ == true)
        {
            CloseInventory();
            openInventory_ = false;
            selectItempanel_.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            scrollbar_.value = 1.0f;
        }

    }

    private void OpenInventory(List<Item> iL)
    {
        //一度すべての子オブジェクトを削除
        foreach (Transform n in panel_.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        int i = 0;
        //アイテム名のテキストの作成 
        foreach (Item d in iL)
        {
            Button prefab = Instantiate(button_, new Vector3(0.0f, -40+(-80*i), 0.0f), Quaternion.identity);

            //Button prefab = Instantiate(button_, new Vector3(0.0f, -40 + (-80 * i), 0.0f), Quaternion.identity) as Button;

            prefab.GetComponentInChildren<Text>().text = d.name_;
            ItemButton itm = new ItemButton(prefab.GetComponentInChildren<Text>().text, "説明文",d.id_,this);
            prefab.onClick.AddListener(itm.OnClick);

            prefab.transform.SetParent(panel_.transform, false);
            i++;
        }
        RectTransform panelSize;
        int num = iL.Count;
        panelSize = panel_.GetComponent<RectTransform>();
        panelSize.sizeDelta = new Vector2(1320,(float)num*80);

        scrollbar_.value = 1.0f;
        {
            //ここでcanvasをtrueに
            buttonCanvas.gameObject.SetActive(true);
        }
        StartCoroutine("BarValue");
    }

    private void CloseInventory()
    {
        //子をすべて削除する
        foreach (Transform n in panel_.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        buttonCanvas.gameObject.SetActive(false);
    }

    public void SelectItem(int id,string name, string detail)
    {
        selectItempanel_.gameObject.SetActive(true);
        nowSelectId_ = id;
        selectItemName_.text = name;
        selectItemDetail_.text = detail;
    }

    //アイテムのソート
    public void SortItem(ITEM_TYPE itemType)
    {
        //ボタンの色を変える
        ChangeColor(itemType);
        //アイテムのリストをコピーする
        //List<Item> list = itemList_;
        List<Item> list = new List<Item>();
        switch (itemType)
        {
            //すべて
            case ITEM_TYPE.ALL:
                list = itemList_;
                OpenInventory(list);
                break;
                //消費アイテム
            case ITEM_TYPE.CONSUMPTION:
                foreach (Item d in itemList_)
                {
                    int id = d.id_ / 100;
                    if(100 <= id && id <= 199)
                    {
                        list.Add(d);
                    }
                }
                OpenInventory(list);
                    break;
            case ITEM_TYPE.WEAPON:
                foreach (Item d in itemList_)
                {
                    int id = d.id_ / 100;
                    if (200 <= id && id <= 299)
                    {
                        list.Add(d);
                    }
                }
                OpenInventory(list);
                break;
            case ITEM_TYPE.ARMOR:
                foreach (Item d in itemList_)
                {
                    int id = d.id_ / 100;
                    if (300 <= id && id <= 399)
                    {
                        list.Add(d);
                    }
                }
                OpenInventory(list);
                break;
            case ITEM_TYPE.OTHER:
                foreach (Item d in itemList_)
                {
                    int id = d.id_ / 100;
                    if (400 <= id && id <= 499)
                    {
                        list.Add(d);
                    }
                }
                OpenInventory(list);
                break;
        }
        scrollbar_.value = 1;
    }



    //ボタンをクリックしたときの処理
    public void OnClickUse()
    {
        Debug.Log(nowSelectId_ + "を使用");
        selectItempanel_.gameObject.SetActive(false);
    }
    public void OnClickCancel()
    {
        selectItempanel_.gameObject.SetActive(false);
    }
    public void OnClickAll()
    {
        SortItem(ITEM_TYPE.ALL);
    }
    public void OnClickConsumption()
    {
        SortItem(ITEM_TYPE.CONSUMPTION);
    }
    public void OnClickWeapon()
    {
        SortItem(ITEM_TYPE.WEAPON);
    }
    public void OnClickArmor()
    {
        SortItem(ITEM_TYPE.ARMOR);
    }
    public void OnClickOther()
    {
        SortItem(ITEM_TYPE.OTHER);
    }

    public void ChangeColor(ITEM_TYPE itemType)
    {
        switch (itemType)
        {
            //すべて
            case ITEM_TYPE.ALL:
                allBtn_.colors = selectColor_;
                consumptionBtn_.colors = OtherColor_;
                weaponBtn_.colors = OtherColor_;
                armorBtn_.colors = OtherColor_;
                otherBtn_.colors = OtherColor_;
                break;
            //消費アイテム
            case ITEM_TYPE.CONSUMPTION:
                allBtn_.colors = OtherColor_;
                consumptionBtn_.colors = selectColor_;
                weaponBtn_.colors = OtherColor_;
                armorBtn_.colors = OtherColor_;
                otherBtn_.colors = OtherColor_;
                break;
            case ITEM_TYPE.WEAPON:
                allBtn_.colors = OtherColor_;
                consumptionBtn_.colors = OtherColor_;
                weaponBtn_.colors = selectColor_;
                armorBtn_.colors = OtherColor_;
                otherBtn_.colors = OtherColor_;
                break;
            case ITEM_TYPE.ARMOR:
                allBtn_.colors = OtherColor_;
                consumptionBtn_.colors = OtherColor_;
                weaponBtn_.colors = OtherColor_;
                armorBtn_.colors = selectColor_;
                otherBtn_.colors = OtherColor_;
                break;
            case ITEM_TYPE.OTHER:
                allBtn_.colors = OtherColor_;
                consumptionBtn_.colors = OtherColor_;
                weaponBtn_.colors = OtherColor_;
                armorBtn_.colors = OtherColor_;
                otherBtn_.colors = selectColor_;
                break;
        }
    }

    //とりあえずのバグ解決用
    IEnumerator BarValue()
    {
        //yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.01f);
        scrollbar_.value = 1;
        //scrollbar_.value = 1;
        yield return null;
    }
}

public class Item
{
    public string name_ = "test";
    public string expTxt_ = "ここに説明文をいれる";
    public int id_ = 0;

    public Item(string name,string expTxt,int id)
    {
        name_ = name;
        expTxt_ = expTxt;
        id_ = id;
    }
    public Item(int id)
    {
        id_ = id;
    }

    public void Use()
    {
        //ここにアイテムを使ったときの関数を呼び出す
    }

}

public class ItemButton
{
    public string name_ = "none";
    public string detail_ = "ここに説明文をいれる";
    public int id_=0;
    ItemController itemScript;
   

    public ItemButton(string name, string detail, int id,ItemController itemController)
    {
        name_ = name;
        detail_ = detail;
        id_ = id;
        itemScript = itemController;
    }

    public void OnClick()
    {
        itemScript.SelectItem(id_, ""+id_, detail_);
    }
}