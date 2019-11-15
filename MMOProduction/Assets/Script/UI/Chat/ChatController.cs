//////////////////////////////////////
// チャット全体のコントロールクラス //
//////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    [SerializeField]
    bool connectFlag = false;

    // チャット全体のフレーム
    [SerializeField]
    private GameObject chatFlame = default(GameObject);

    // チャットログのテキスト
    [SerializeField]
    private Text chatLogText = default(Text);

    // 入力メッセージ
    [SerializeField]
    private InputField inputMassege = default(InputField);

    // チャットログ
    private List<string> chatLog = new List<string>();

    // チャットサーバー
    WS.WsChat wsc = null; 


    // チャット画面表示フラグ
    private bool chatActiveFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        chatFlame.SetActive(chatActiveFlag);
        if (connectFlag)
        {
            wsc = WS.WsChat.Instance;
            wsc.allChatAction = Receive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // チャットのアクティブ化
        if (Input.GetKeyUp(KeyCode.Return) && chatActiveFlag == false)
        {
            chatFlame.SetActive(true);
            chatActiveFlag = true;
            //chatFlame.GetComponent<ChatLogController>().Reset();
        }
        else if (Input.GetKeyUp(KeyCode.Return) && chatActiveFlag == true)
        {
            chatFlame.SetActive(false);
            chatActiveFlag = false;
        }

        if (chatActiveFlag)
        {
            ChatLogUpdate();
            if (Input.GetKeyDown(KeyCode.Return) && inputMassege.text != "")
            {
                SendInputMassege(inputMassege.text);
                inputMassege.text = "";
            }
        }
    }

    private void OnDestroy()
    {
        if (connectFlag) { wsc.Destroy(); }
    }

    // チャットログの更新
    private void ChatLogUpdate()
    {
        chatLogText.text = "";
        for (int i = 0; i < chatLog.Count; i++)
        {
            chatLogText.text += chatLog[i] + "\n";
        }
    }

    // チャットログの追加
    public  void AddChatLog(string _addLog)
    {
        if (chatLog.Count > 100)
        {
            chatLog.RemoveAt(0);
        }
        chatLog.Add(_addLog);
    }

    public void Receive(Packes.RecvAllChat _packet)
    {
        string massege = "";
        massege = massege + _packet.user_name + "：" + _packet.message;
        
        AddChatLog(massege);
    }

    private void SendInputMassege(string _massege)
    {
        string name = UserRecord.ID.ToString();
        Packes.SendAllChat mag = new Packes.SendAllChat(name, _massege);
        if (connectFlag) { wsc.Send(Json.ConvertToJson(mag)); }
        string mas = name + "：" + _massege;
    }

    public bool GetChatActiveFlag()
    {
        return chatActiveFlag;
    }
}
