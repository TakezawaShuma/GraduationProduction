using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;


public class NodeJsClient : MonoBehaviour
{
    [SerializeField]
    string serverIp;
    [SerializeField]
    int port;

    // getter&setter
    public string IP { get { return serverIp; } } 
    public int Port { get { return port; }set { port = value; } }

    // パケット情報
    PlayerData sendData = new PlayerData();

    // ソケット
    WebSocket ws;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        //ws = new WebSocket("ws://" + serverIp + ":" + port.ToString());

        //ws.OnOpen += (sender, e) =>
        //  {
        //      Debug.Log("WebSocket Open");
        //  };
        //ws.OnMessage += (sender, e) =>
        //{
        //    Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data : " + e.Data);
        //    Debug.Log(sender.GetType());
        //    Debug.Log("Data: " + e.Data);

        //};
        //ws.OnError += (sender, e) =>
        //{
        //    Debug.Log("WebSocket Error Message: " + e.Message);
        //};
        //ws.OnClose += (sender, e) =>
        //{
        //    Debug.Log("WebSocket Close");
        //};

        //ws.Connect();

        sendData.X = 10.0f;
        sendData.Y = 20.0f;
        sendData.Z = 30.0f;
        sendData.HP = 500.0f;
        sendData.MP = 100.0f;
        sendData.Direction = -32.0f;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {

        if (Input.GetKeyUp("s"))
        {
            //SendToServer(sendData);
        }

    }

    /// <summary>
    /// WebSocketを閉じる
    /// </summary>
    private void OnDestroy()
    {
        //ws.Close();
        ws = null;
    }

    /// <summary>
    /// サーバーにデータを送信する
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public bool SendToServer(PlayerData Data)
    {
        bool sf = true;
        try
        {
            string str = ConvertToJson(Data);
            //ws.Send(str);
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
    private string ConvertToJson(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public void Receive()
    {
    }



}


