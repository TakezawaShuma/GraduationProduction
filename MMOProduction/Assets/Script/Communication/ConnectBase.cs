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
    private string server_ip = "172.24.52.250";
    //private string server_ip = "localhost";

    /// <summary>
    /// 接続
    /// </summary>
    /// <param name="_port">ポート</param>
    public void Connect(int _port) {
        ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
        Debug.Log("IPアドレス : " + server_ip + "ポート : " + _port);
        try{
            ws.Connect();
        } catch {
            Debug.Log("サーバーへ接続ができません。");
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="_msg">削除した際のメッセージ</param>
    protected virtual void Destroy(string _msg = ""){
        if (_msg != "") Debug.Log(_msg);
        ws.Close();
        ws = null;
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void WsInit(string _openMsg = "",string _closeMsg = "") {
        ws.OnOpen += (sender, e) => { Debug.Log("WebSocket Open : " + _openMsg); };
        ws.OnError += (sender, e) => { Debug.LogError("WebSocket Error Message: " + e.Message); };
        ws.OnClose += (sender, e) => { Debug.Log("WebSocket Close : " + _closeMsg); };
    }
}
