using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;

public abstract class ConnectBase 
{
    // ソケット
    protected WebSocket ws;
    // サーバーのIP
    protected string server_ip;
    // サーバーのポート
    protected int port;

    // 接続開始
    protected abstract void ConnectionStart();

    protected　virtual void Connect()
    {
        ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
        Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);
        try
        {
            ws.Connect();
        }
        catch
        {
            Debug.Log("サーバーへ接続ができません。");
        }
    }

    protected abstract void Destroy();



}
