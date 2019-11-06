using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData : MonoBehaviour
{
    public enum HOGEID
    {
        SKILL,
        ITEM,
        NONE,
    }

    [SerializeField]
    private int id;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
