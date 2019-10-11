using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject[] prefabs;

    enum Window
    {
        CHARACTOR,
        ITEM,
        SKILL,
        QEST,
        PARTY,
        CHAT,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  PushKeyEvent(GameObject obj)
    {
        if(obj.activeInHierarchy != true)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}
