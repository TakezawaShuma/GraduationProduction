using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class DressedList
{
    public bool[] list = new bool[9];

    public DressedList(bool[] list)
    {
        this.list = list;
    }
}

public class TakeOff : MonoBehaviour
{
    [SerializeField]
    Command command = null;

    [SerializeField, Header("服")]
    private GameObject[] clothes = null;

    public enum ClothesState
    {
        Normal,
        Tanktop,
        Bikini,
        Nude,

        MaxNum,
    }

    private int currentClothes = (int)ClothesState.Normal;

    [SerializeField, Header("服の状態")]
    private DressedList[] isDressed = new DressedList[(int)ClothesState.MaxNum];

    private int clothesNum;

    // Start is called before the first frame update
    void Start()
    {
        command.SetAction(Next);
        clothesNum = clothes.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        currentClothes += 1;

        if(currentClothes >= (int)ClothesState.MaxNum)
        {
            currentClothes = (int)ClothesState.Normal;
        }

        ChangeClothes((ClothesState)currentClothes);
    }

    private void ChangeClothes(ClothesState clothesState)
    {
        switch(clothesState)
        {
            case ClothesState.Normal:
                for(int i = 0; i < clothesNum; i++)
                {
                    clothes[i].SetActive(isDressed[(int)ClothesState.Normal].list[i]);
                }
                break;
            case ClothesState.Tanktop:
                for (int i = 0; i < clothesNum; i++)
                {
                    clothes[i].SetActive(isDressed[(int)ClothesState.Tanktop].list[i]);
                }
                break;
            case ClothesState.Bikini:
                for (int i = 0; i < clothesNum; i++)
                {
                    clothes[i].SetActive(isDressed[(int)ClothesState.Bikini].list[i]);
                }
                break;
            case ClothesState.Nude:
                for (int i = 0; i < clothesNum; i++)
                {
                    clothes[i].SetActive(isDressed[(int)ClothesState.Nude].list[i]);
                }
                break;
        }
    }
}
