using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ConnectLogin : MonoBehaviour
{
    // ソケット
    private WebSocket ws;

    //　サーバーのIP
    [SerializeField]
    string server_ip;

    // ログインサーバーのポート
    private int port = 8000;

    // ログイン用データ
    Packes.Login login_packet = new Packes.Login();

    // 受信データ
    private Packes.IPacketDatas i_data = null;

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
        Debug.Log("start" + " : " + server_ip + " : " + port);


        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open(Login)");
        };
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Data : " + e.Data);
            i_data = Receive(e);
            Debug.Log(i_data.command);
            if (i_data.Command == CommandData.CmdOKConfirmation)
            {
                SceneManager.LoadScene("PlayScene");
            }
        };
        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close(Login)");
        };
        ws.Connect();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            login_packet.command = 102;
            login_packet.username = "tsit";
            login_packet.pass = "trident";
            SendToServer(login_packet);
        }
        if (Input.GetKeyDown("s"))
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    public Packes.IPacketDatas Receive(MessageEventArgs e)
    {
        Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data : " + e.Data);
        Debug.Log("Data: " + e.Data);

        CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
        Debug.Log(com);

        switch(com)
        {
            case CommandData.CmdOKConfirmation:
                Debug.Log(e.Data);
                Packes.OKConfirmation ok = JsonUtility.FromJson<Packes.OKConfirmation>(e.Data);
                Retention.ID = ok.user_id;
                return ok;
            case CommandData.CmdMissingConfirmation:
                Debug.Log(e.Data);
                Packes.MissingConfirmation miss = JsonUtility.FromJson<Packes.MissingConfirmation>(e.Data);
                return miss;
            default:
                break;
        }
        return null;

    }

    /// <summary>
    /// サーバーにデータを送信する
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public bool SendToServer(Packes.IPacketDatas Data)
    {
        bool sf = true;
        try
        {
            string str = ConvertToJson(Data);
            ws.Send(str);
            Debug.Log(str);
        }
        catch
        {
            Debug.Log("送信に失敗しました。");
            return false;
        }
        return sf;
    }

    /// <summary>
    /// パケットをJSON形式に変換する
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string ConvertToJson(Packes.IPacketDatas data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        return json;
    }
}
