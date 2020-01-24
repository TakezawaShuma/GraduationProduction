using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インベントリスロットのデータ
/// </summary>
public class SlotData : MonoBehaviour
{
    private bool useFlag = false;

    public enum STATUS
    {
        SKILL,
        ITEM,
        NONE,
    }

    [SerializeField]
    private int id = 0;

    [SerializeField]
    private STATUS status = STATUS.NONE;

    [SerializeField]
    private string name = "";

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

    public STATUS Status
    {
        get
        {
            return status;
        }

        set
        {
            status = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        id = 0;
        status = STATUS.NONE;
        name = this.transform.parent.name;
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
