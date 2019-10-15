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
        // サーバーのIP
        // debug
        //private string server_ip = "172.24.52.250";
        private string server_ip = "localhost";

        private Action reconect_callback;


        public void InitializeWebSocket(Action _func)
        {
            reconect_callback = _func;
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="_port">ポート</param>
        public void Connect(int _port)
        {
            if (Retention.IP != "")
            {
                server_ip = Retention.IP;
                Debug.Log(server_ip);
            }

            ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + _port);
            WsInit();

                ws.Connect();
             
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="_msg">削除した際のメッセージ</param>
        protected virtual void Destroy(string _msg = "")
        {
            if (_msg != "") Debug.Log(_msg);
            if (ws != null) ws.Close();
            ws = null;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void WsInit(string _openMsg = "", string _closeMsg = "")
        {
            ws.OnOpen += (sender, e) => { Debug.Log("WebSocket Open : " + _openMsg); };
            ws.OnError += (sender, e) => { Debug.LogError("WebSocket Error Message: " + e.Message); };
            ws.OnClose += (sender, e) => { Debug.Log("WebSocket Close"); };
        }

        public void ForDebug()
        {
            Debug.Log(this.ws.ReadyState);
        }


    }
}