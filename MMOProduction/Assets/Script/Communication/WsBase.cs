﻿///////////////////////////////////
// WebSocketで通信する基底クラス //
///////////////////////////////////

using UnityEngine;
using WebSocketSharp;

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
        private string server_ip = "172.24.52.250";
        //private string server_ip = "localhost";

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="_port">ポート</param>
        public bool Connect(int _port)
        {
            bool ret = false;
            ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + _port);
            try
            {
                ws.Connect();
                ret = true;
            }
            catch
            {
                Debug.Log("サーバーへ接続ができません。");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="_msg">削除した際のメッセージ</param>
        protected virtual void Destroy(string _msg = "")
        {
            if (_msg != "") Debug.Log(_msg);
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void WsInit(string _openMsg = "", string _closeMsg = "")
        {
            ws.OnOpen += (sender, e) => { Debug.Log("WebSocket Open : " + _openMsg); };
            ws.OnError += (sender, e) => { Debug.LogError("WebSocket Error Message: " + e.Message); };
        }
    }
}