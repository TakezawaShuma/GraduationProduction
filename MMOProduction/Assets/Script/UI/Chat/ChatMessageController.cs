﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatMessageController : MonoBehaviour
{
    // チャットログのテキスト
    [Header("チャットログ"), SerializeField]
    private Text chatLogText = default(Text);

    // 入力メッセージ
    [Header("入力フィールド"), SerializeField]
    private InputField messageInput = default(InputField);
    public GameObject InputField { get { return messageInput.gameObject; } }

    // チャットログ
    private List<string> chatLog = new List<string>();

    private ChatController controller = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponentInParent<ChatController>();
        ChatLogUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetChatActiveFlag())
        {
            if (InputManager.InputKeyCheckDown(KeyCode.Return) && messageInput.text != "")
            {
                if (InputManager.InputKeyCheck(KeyCode.LeftControl) || InputManager.InputKeyCheck(KeyCode.RightControl))
                {
                    SendInputMassege(messageInput.text);
                    messageInput.text = "";
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(InputField);
                }
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
    public void AddChatLog(string _addLog)
    {
        if (chatLog.Count > 100)
        {
            chatLog.RemoveAt(0);
        }
        chatLog.Add(_addLog);
        ChatLogUpdate();
    }


    public void Receive(Packes.RecvAllChat _packet)
    {
        string massege = "";
        massege = massege + _packet.user_name + "：" + _packet.message;

        AddChatLog(massege);
    }

    private void SendInputMassege(string _massege)
    {
        string name = UserRecord.Name.ToString();
        Packes.SendAllChat mag = new Packes.SendAllChat(name, _massege);
        WS.WsChat.Instance.Send(Json.ConvertToJson(mag)); 
    }


}
