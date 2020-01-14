using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インベントリスロットのデータ
/// </summary>
public class SlotData : MonoBehaviour
{
    private bool useFlag = false;

    public enum HOGEID
    {
        SKILL,
        ITEM,
        NONE,
    }

    [SerializeField]
    private int id = -1;

    [SerializeField]
    private HOGEID hoge = HOGEID.NONE;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public HOGEID HOGE
    {
        get
        {
            return hoge;
        }

        set
        {
            hoge = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        id = -1;
        hoge = HOGEID.NONE;
    }

    public void OnClick()
    {
        useFlag = true;
    }

    public bool GetUseFlag()
    {
        return useFlag;
    }

    public void UseFlagOff()
    {
        useFlag = false;
    }
}
