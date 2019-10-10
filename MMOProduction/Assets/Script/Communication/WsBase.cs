///////////////////////////////////
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
        protected WebSocket ws = null;
        // サーバーのIP
        // debug
        //private string server_ip = "172.24.52.250";
        private string server_ip = "localhost";

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="_port">ポート</param>
        public bool Connect(int _port)
        {
            if (Retention.IP != "")
            {
                server_ip = Retention.IP;
                Debug.Log(server_ip);
            }

            ws = new WebSocket("ws://" + server_ip + ":" + _port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + _port);
            Debug.Log(ws.ReadyState);
            bool ret = false;
            try
            {
                ws.Connect();
                ret = true;
                Debug.Log("接続");
            }
            catch
            {
                Debug.LogError("サーバーへ接続ができません。");
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
            if (ws != null) ws.Close();
            ws = null;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void WsInit(string _openMsg = "", string _closeMsg = "")
        {
            //ws.OnOpen += (sender, e) => { Debug.Log("WebSocket Open : " + _openMsg); };
            //ws.OnError += (sender, e) => { Debug.LogError("WebSocket Error Message: " + e.Message); };
        }

        public void ForDebug()
        {
            Debug.Log(this.ws.ReadyState);
        }
    }
}