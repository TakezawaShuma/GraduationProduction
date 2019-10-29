using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ui;

    [SerializeField]
    private KeyCode[] key;

    // Start is called before the first frame update
    void Start()
    {
        if (ui != null)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ui != null)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                if(Input.GetKeyDown(key[i]))
                {
                    if(ui[i].activeInHierarchy)
                    {
                        ui[i].SetActive(false);
                    }
                    else
                    {
                        ui[i].SetActive(true);
                    }
                }
            }
        }
    }
}
