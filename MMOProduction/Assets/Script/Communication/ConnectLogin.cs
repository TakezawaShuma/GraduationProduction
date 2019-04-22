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

    // シーン切り替え用フラグ
    Scenes scene_flag = Scenes.Non;

    //　サーバーのIP
    [SerializeField]
    string server_ip;

    // ログインサーバーのポート
    private int port = 8000;

    // ログイン用データ
    Packes.Login login_packet = new Packes.Login();

    // 受信データ
    private Packes.IPacketDatas i_data = null;


    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
        Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);

        RelatedToWS();
        ws.Connect();
    }

    /// <summary>
    /// ログインの送信
    /// </summary>
    /// <param name="_id">ユーザー名</param>
    /// <param name="_pass">パス</param>
    public void Login(string _id,string _pass)
    {
        login_packet.command = 102;
        login_packet.username = _id;
        login_packet.pass = _pass;
        SendToServer(login_packet);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        switch(scene_flag)
        {
            case Scenes.Login:
                SceneManager.LoadScene("LoginScene");
                break;
            case Scenes.Play:
                SceneManager.LoadScene("PlayScene");
                break;
            case Scenes.Non:
                break;
            default:
                break;
        }
        if (Input.GetKeyDown("s"))
        {
            scene_flag = Scenes.Login;
        }
    }
    /// <summary>
    /// オブジェクト破棄/シーン終了時に実行
    /// </summary>
    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }

    /// <summary>
    /// WebSocket関係のイベント
    /// </summary>
    void RelatedToWS()
    {

        // 接続が確立したら呼ばれる
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open(Login)");
        };

        // データが送られてくると呼ばれる
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Data : " + e.Data);
            i_data = Receive(e);
            Debug.Log(i_data.command);
            if (i_data.Command == CommandData.CmdOKConfirmation)
            {
                Debug.Log("プレイシーンに移行します。");
                scene_flag = Scenes.Play;
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
            Debug.Log("WebSocket Close(Login)");
        };
    }

    /// <summary>
    /// データを受信したときの処理
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public Packes.IPacketDatas Receive(MessageEventArgs e)
    {
        // データの形の確認
        Debug.Log("Data: " + e.Data);

        // 受信データからコマンドを取り出す
        CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

        // コマンドで受信データサイズを変える
        // コマンド内容はDatas.csを参照
        switch(com)
        {
            case CommandData.CmdOKConfirmation:
                Debug.Log(e.Data);
                Packes.OKConfirmation ok = JsonUtility.FromJson<Packes.OKConfirmation>(e.Data);
                // IDの保管
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
}
