using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ConnectPlay : MonoBehaviour
{
    // ソケット
    private WebSocket ws;
    //　サーバーのIP
    [SerializeField]
    string server_ip;

    // ログインサーバーのポート
    private int port = 8001;

    // プレイシーンでやり取りするデータ
    private Packes.SendPosSync send_data = new Packes.SendPosSync();

    // 受信データ
    private Packes.RecvPosSync recv_data = new Packes.RecvPosSync();


    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // WebSocketの作成
        ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
        Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);

        RelatedToWS();
        ws.Connect();

        send_data.command = 201;
        send_data.user_id = Retention.ID;
        send_data.X = 10;
        send_data.Y = 20;
        send_data.Z = 30;
        send_data.HP = 500;
        send_data.MP = 300;
        Debug.Log(send_data.user_id);
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        SendToServer(send_data);
    }

    /// <summary>
    /// データを受信したときの処理
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    void RelatedToWS()
    {
        // 接続が確立したら呼ばれる
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open(Play)");
        };
        // データが送られてくると呼ばれる
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Data : " + e.Data);
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

            // コマンドから受信データサイズを決定
            // コマンド内容はDatas.csを参照
            switch (com)
            {
                case CommandData.CmdRecvPosSync:
                    recv_data = Receive(e);
                    break;
                default:
                    break;
            }
            Debug.Log(recv_data.command);
            if (recv_data.Command == CommandData.CmdRecvPosSync)
            {
                // 此処に送られてきた情報を設定する関数を入れる
            }
        };

        // 通信にエラーが発生すると呼ばれる
        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };
        // 通信が切断されソケットが閉じられると呼ばれる
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close(Play)");
        };
    }


    /// <summary>
    /// サーバーにデータを送信する
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public bool SendToServer(Packes.IPacketDatas Data)
    {
        // 送信の成否
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
        return json;
    }

    /// <summary>
    /// データを受信したときの処理
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public Packes.RecvPosSync Receive(MessageEventArgs e)
    {
        // データの形の確認
        Debug.Log("Data: " + e.Data);
        // JSONをデシリアライズ
        Packes.RecvPosSync recv = JsonUtility.FromJson<Packes.RecvPosSync>(e.Data);
        return recv;

    }

    /// <summary>
    /// 送るデータを設定する
    /// </summary>
    /// <param name="data"></param>
    public void SetSendData(Packes.SendPosSync data)
    {
        send_data.HP = data.HP;
        send_data.MP = data.MP;
        send_data.X = data.X;
        send_data.Y = data.Y;
        send_data.Z = data.Z;
    }

}
