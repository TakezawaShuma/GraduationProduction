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

        int i = 0;
        foreach(var obj in list) {
            SlotData slot = obj.GetComponent<SlotData>();
            if (slot.ID == -1) {
                slot.ID = _id;
                break;
            }
            i++;
        }
        ImageCheck(i);
        return true;
    }

    public void ChangeItems(int[] _ids) {
        List<Packes.AccessoryMasterData> imageName = AccessoryDatas.Find(_ids);
        foreach(var id in _ids) {
            if (id <= 0) continue;
            foreach (var obj in list) {
                SlotData slot = obj.GetComponent<SlotData>();
                if (slot.ID == -1) {
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
                if (slot.ID == -1) {
                    slot.ID = id;
                    Debug.Log(slot.ID.ToString() + " : " + id.ToString());
                    break;
                }
            }
        }
        ImageCheck();
    }

    private void ImageCheck() {
        foreach(var obj in list) {
            if(obj.GetComponent<SlotData>().ID != -1) {
                obj.GetComponent<Image>().sprite = sprite;
            }
        }
    }

    private void ImageCheck(int _index) {
        list[_index].GetComponent<Image>().sprite = sprite;
    }
}
