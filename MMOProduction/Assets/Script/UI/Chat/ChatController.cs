//////////////////////////////////////
// チャット全体のコントロールクラス //
//////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatController : MonoBehaviour
{
    [SerializeField]
    bool connectFlag = false;

    // チャット全体のフレーム
    [Header("チャットフレーム"), SerializeField]
    private ChatMessageController chatFlame = null;
    
    // チャットサーバー
    WS.WsChat wsc = null; 


    // チャット画面表示フラグ
    private bool chatActiveFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        chatFlame.gameObject.SetActive(chatActiveFlag);
        if (connectFlag)
        {
            wsc = WS.WsChat.Instance;
            WS.WsChat.Instance.allChatAction = chatFlame.Receive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // チャットのアクティブ化
        if (Input.GetKeyUp(KeyCode.Return) && chatActiveFlag == false)
        {
            chatActiveFlag = true;
            chatFlame.gameObject.SetActive(chatActiveFlag);
            EventSystem.current.SetSelectedGameObject(chatFlame.GetComponent<ChatMessageController>().InputField);
        }
        else if (Input.GetKeyUp(KeyCode.Return) && chatActiveFlag == true)
        {
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                chatActiveFlag = false;
                chatFlame.gameObject.SetActive(chatActiveFlag);
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private void OnDestroy()
    {
        if (connectFlag) { wsc.Destroy(); }
    }
    
    public bool GetChatActiveFlag()
    {
        return chatActiveFlag;
    }
}
