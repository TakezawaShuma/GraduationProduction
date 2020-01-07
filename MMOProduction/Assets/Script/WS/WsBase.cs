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
        protected WebSocket ws = null;



        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="_port">ポート</param>
        protected void Connect(uint _port)
        {
            string server_ip = ExternalFileAccess.ReadFile("../ip.txt");
            if (ws == null) ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
            if (ws.ReadyState == WebSocketState.Open && ws != null)
            {
                Debug.LogWarning("すでに接続済み"); return;
            }
            WsInit(_port.ToString());
            ws.Connect();
        }



        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="_msg">削除した際のメッセージ</param>
        protected virtual void Destroy(string _msg = "", bool type = true)
        {
            if (ws != null) return;
            if (ws.ReadyState != WebSocketState.Closed)
            {
                if (_msg != "")
                {
                    if (type) { Debug.Log(_msg); }
                    else { Debug.LogWarning(_msg); }
                }

                ws.Close();
                ws = null;
            }
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

        /// <summary>
        /// 送信関数
        /// </summary>
        /// <param name="_json"></param>
        public abstract void Send(string _json);

        protected abstract void Receive();

    }
}