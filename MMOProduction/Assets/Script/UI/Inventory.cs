using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    private readonly int MAX_ITEM = 90;
    private readonly int WIDTH = 9;
    private readonly int HEIGHT = 10;
    //管理系
    private List<Image> list = new List<Image>();

    private List<SlotData> sloatDatas_ = new List<SlotData>();

    //生成に必要な物群
    [SerializeField]
    private GameObject Slot = null;

    [SerializeField]
    private Sprite sprite = null;

    // Start is called before the first frame update
    void Start()
    {
        GenerateInventory();
    }

    void Update()
    {
    }

    private void GenerateInventory()
    {
        Vector3 pos = new Vector3((-240 + 8 + 8), 265 - 8 - 50 - 8 - 24 - 24, 0);

        int x = 0;int y = 0;
        for(int i = 0; i < MAX_ITEM; i++) {
            x = i % HEIGHT;
            y = i / HEIGHT;

            GameObject obj = Instantiate(Slot, transform);
            obj.transform.localPosition = pos + new Vector3(x * 48, y * -48, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.name = "Inventory" + i;
            list.Add(obj.transform.GetChild(0).GetComponent<Image>());
        }
        ImageCheck();
    }

    // TODO: 画像のマスター作成後交換
    public bool AddItem(int _id) {
        if (sloatDatas_.Count >= MAX_ITEM) return false;
        if (_id <= 0) return false;

        SlotData data = new SlotData();
        data.ID = _id;
        sloatDatas_.Add(data);
        return true;
    }

    public void ChangeItems(int[] _ids) {
        List<Packes.AccessoryMasterData> imageName = AccessoryDatas.Find(_ids);
        foreach(var id in _ids) {
            if (id <= 0) continue;

            SlotData data = new SlotData();
            data.ID = id;
            sloatDatas_.Add(data);
        }
        ImageCheck();
    }

    public void ChangeItems(List<int> _ids) {
        List<Packes.AccessoryMasterData> imageName = AccessoryDatas.Find(_ids);
        foreach(var id in _ids) {
            if (id <= 0) continue;
            SlotData data = new SlotData();
            data.ID = id;
            sloatDatas_.Add(data);
        }
        ImageCheck();
    }

    private void ImageCheck() { 
        for(int i = 0; i < sloatDatas_.Count; i++) {
            list[i].sprite = sprite;
        }
    }
}
