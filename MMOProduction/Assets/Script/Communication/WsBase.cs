///////////////////////////////////
// WebSocketで通信する基底クラス //
///////////////////////////////////

using UnityEngine;
using WebSocketSharp;
using System;

namespace WS
{
    /// <summary>
    /// 通信基礎
    /// </summary>
    public abstract class WsBase
    {
        // ソケット
        protected WebSocket ws;
        // サーバーのIP
        //private string server_ip = "172.24.52.250";
        private string server_ip = "localhost";

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="_port">ポート</param>
        protected void Connect(uint _port)
        {
            ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + _port);
            WsInit();
            ws.Connect();
        }



        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="_msg">削除した際のメッセージ</param>
        protected virtual void Destroy(string _msg = "", bool type = true)
        {
            if (_msg != "")
            {
                if (type) { Debug.Log(_msg); }
                else { Debug.LogWarning(_msg); }
            }
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void WsInit(string _openMsg = "", string _closeMsg = "")
        {
            ws.OnOpen += (sender, e) => { Debug.Log("WebSocket Open : " + _openMsg); };
            ws.OnError += (sender, e) => { Debug.LogError("WebSocket Error Message: " + e.Message); };
            ws.OnClose += (sender, e) => { Destroy("通信が切断されました: " + _closeMsg, false); };
        }
    }
}