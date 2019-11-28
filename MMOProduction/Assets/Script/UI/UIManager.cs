using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI全般を管理するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ui = null;

    [SerializeField]
    private KeyCode[] key = null;

    [SerializeField]
    private ChatController chat = null;

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
                if (InputManager.InputKeyCheckDown(key[i])&&!chat.GetChatActiveFlag())
                {
                    if (ui[i].activeInHierarchy)
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
