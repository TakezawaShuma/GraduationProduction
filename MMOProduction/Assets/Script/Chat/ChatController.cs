using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    // チャット全体のフレーム
    [SerializeField]
    private GameObject chatFlame;

    // チャットログのテキスト
    [SerializeField]
    private Text chatLogText;

    // 入力メッセージ
    [SerializeField]
    private InputField inputMassege;


    // チャットログ
    private List<string> chatLog = new List<string>();

    // チャットサーバー
    //WS.WsChat wc = new WS.WsChat();


    // チャット画面表示フラグ
    private bool chatActiveFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        chatFlame.SetActive(false);
        //wc.ConnectionStart(Receive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) && chatActiveFlag == false)
        {
            chatFlame.SetActive(true);
            chatActiveFlag = true;
            //chatFlame.GetComponent<ChatLogController>().Reset();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && chatActiveFlag == true)
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

    public void Receive(WS.ChatMassege _cm)
    {
        string massege = "";

        massege = massege + _cm.name + "：" + _cm.massege;



        AddChatLog(massege);
    }

    private void SendInputMassege(string _massege)
    {

            Debug.Log("えんたー");
            string name = Retention.ID.ToString();
            //wc.SendMessage(name, _massege);
            string mas = name + "：" + _massege;
            AddChatLog(mas);

        
    }

}
