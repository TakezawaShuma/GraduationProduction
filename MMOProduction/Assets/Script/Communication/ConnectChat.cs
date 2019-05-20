using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using System;

namespace Connect
{
    public class ConnectChat
    {
        // ソケット
        private WebSocket ws;

        // サーバーのIP
        private const string server_ip = "172.24.52.250";
        // ログインサーバーのポート
        private const int port = 8009;

        // 受信データ
        private Packes.IPacketDatas i_data = null;

        // チャットコールバック
        private Action<string, string> chat_callback;


        public void ConnectionStart(Action<string,string> _callback)
        {
            chat_callback = _callback;

        }

        /// <summary>
        /// 接続処理
        /// </summary>
        private void Connect()
        {
            ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);

            // 接続開始
            try
            {
                ws.Connect();
            }
            catch
            {
                Debug.Log("サーバーへ接続ができません。");
            }

        }

        /// <summary>
        /// WebSocket関係のイベント
        /// </summary>
        private void RelatedToWS()
        {
            // 接続が確立したら呼ばれる
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket Open(Login)");
            };
            // データが送られてくると呼ばれる
            ws.OnMessage += (sender, e) =>
            {
  
            };

            // 通信にエラーが発生すると呼ばれる
            ws.OnError += (sender, e) =>
            {
                Debug.LogError("WebSocket Error Message: " + e.Message);
            };

            // 通信が切断されソケットが閉じられると呼ばれる
            ws.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket Close(Login)");
            };
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            Debug.Log("ログインシーンの終了");
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Packes.IPacketDatas Receive(MessageEventArgs e)
        {
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

            // コマンドで受信データサイズを変える
            // コマンド内容はDatas.csを参照
            switch (com)
            {
                case CommandData.CmdChat:
                    // JSONをデシリアライズ
                    Packes.Chat chat = JsonUtility.FromJson<Packes.Chat>(e.Data);
                    break;
            }
            return null;

        }

        public bool SendMessage(string user_name,string message)
        {
            Packes.Chat chat_packet = new Packes.Chat();
            
            try
            {
                //string str = ConvertToJson(chat_packet);
                //ws.Send(str);
                //Debug.Log(str);
            }
            catch
            {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

    }
}