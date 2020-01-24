using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{

    [SerializeField]
    private item_sprites_table images;

    private readonly int MAX_ITEM = 90;
    private readonly int WIDTH = 9;
    private readonly int HEIGHT = 10;
    //管理系
    private List<GameObject> list = new List<GameObject>();

    //生成に必要な物群
    [SerializeField]
    private GameObject Slot = null;

    [SerializeField]
    private Sprite sprite = null;

    private void Awake()
    {
        GenerateInventory();
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
            obj.transform.GetChild(0).gameObject.GetComponent<SlotData>().Init();
            list.Add(obj.transform.GetChild(0).gameObject);
        }
        ImageCheck();
    }

    // TODO: 画像のマスター作成後交換
    public bool AddItem(int _id) {
        if (_id <= 0) return false;
        List<int> ids = new List<int>();
        int i = 0;
        foreach(var obj in list) {
            SlotData slot = obj.GetComponent<SlotData>();
            if (slot.ID <= 0) {
                slot.ID = _id;
                ids.Add(_id);
                break;
            }
            i++;
        }
        ImageCheck(i);
        UserRecord.Inventory = ids;
        return true;
    }

    public void ChangeItems(int[] _ids) {
        List<Packes.AccessoryMasterData> imageName = AccessoryDatas.Find(_ids);
        foreach(var id in _ids) {
            if (id <= 0) continue;
            foreach (var obj in list) {
                SlotData slot = obj.GetComponent<SlotData>();
                if (slot.ID <= 0) {
                    slot.ID = id;
                    break;
                }
            }
        }
        ImageCheck();
    }

    public void ChangeItems(List<int> _ids) {
        List<Packes.AccessoryMasterData> imageName = AccessoryDatas.Find(_ids);
        foreach (var id in _ids) {
            if (id <= 0) continue;
            foreach (var obj in list) {
                SlotData slot = obj.GetComponent<SlotData>();
                if (slot.ID <= 0) {
                    slot.ID = id;
                    break;
                }
            }
        }
        ImageCheck();
    }

    private void ImageCheck() {
        foreach(var obj in list) {
            int id = obj.GetComponent<SlotData>().ID;
            if (id > 0) {
                obj.GetComponent<Image>().sprite = images.FindOne(id);
            }
        }
    }

    private void ImageCheck(int _index) {
        list[_index].GetComponent<Image>().sprite = images.FindOne(list[_index].GetComponent<SlotData>().ID);
    }

    public GameObject FindAccessory(int _id) {
        foreach (var obj in list) {
            int id = obj.GetComponent<SlotData>().ID;
            if (id == _id) return obj;
        }
        return null;
    }
}
