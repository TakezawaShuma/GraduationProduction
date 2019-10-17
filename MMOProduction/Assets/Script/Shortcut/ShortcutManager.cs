using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShortcutManager : MonoBehaviour
{
    [SerializeField, Header("ショートカットボタン")]
    private Button[] buttons = new Button[10];


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            int a = i;
            buttons[i].GetComponent<ShortcutButton>().SetShortcut(() => Debug.Log(a), i.ToString(), null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 10; i++)
        {
            if(Input.GetKeyDown(i.ToString()))
            {
                buttons[i].onClick.Invoke();
            }
        }
    }
}
