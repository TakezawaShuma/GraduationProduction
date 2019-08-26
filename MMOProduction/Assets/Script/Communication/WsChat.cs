/////////////////////////////////
// チャット用のWebSocketクラス //
/////////////////////////////////

using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;

namespace WS
{   public struct ChatMassege
    {
        public ChatMassege(string _name, string _mass) { name = _name; massege = _mass; }
        public string name;
        public string massege;
    }
    public class WsChat
    {
        // ソケット
        private WebSocket ws;


        // サーバーのIP
        private const string server_ip = "172.24.52.250";
        //private const string server_ip = "localhost";
        // ログインサーバーのポート
        private const int port = 8009;
        // 受信データ
        //private Packes.IPacketDatas i_data = null;
        // チャットコールバック
        private Action<ChatMassege> chatReceive_callback;        

        public void ConnectionStart(Action<ChatMassege> _callback){
            chatReceive_callback = _callback;
            Connect();
        }

        /// <summary>
        /// 接続処理
        /// </summary>
        private void Connect(){
            ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);
            // 接続開始
            try{
                ws.Connect();
            } catch {
                Debug.Log("サーバーへ接続ができません。");
            }
        }

        /// <summary>
        /// WebSocket関係のイベント
        /// </summary>
        private void RelatedToWS() {
            ws.OnOpen += (sender, e) => {
                Debug.Log("WebSocket Open(Login)");
            };
            ws.OnMessage += (sender, e) => {
                var context = SynchronizationContext.Current;
                // データ形の確認
                Debug.Log("Data : " + e.Data);
                context.Post(state =>{
                    Receive(e);
                }, e.Data);
            };
            ws.OnError += (sender, e) => {
                Debug.LogError("WebSocket Error Message: " + e.Message);
            };
            ws.OnClose += (sender, e) =>{
                Debug.Log("WebSocket Close(Login)");
            };
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy(){
            Debug.Log("ログインシーンの終了");
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Packes.IPacketDatas Receive(MessageEventArgs e){
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
            // コマンドで受信データサイズを変える
            // コマンド内容はDatas.csを参照
            switch (com){
                case CommandData.Chat:
                    // JSONをデシリアライズ
                    Packes.Chat chat = JsonUtility.FromJson<Packes.Chat>(e.Data);
                    ChatMassege cm = new ChatMassege(chat.user_name, chat.message);
                    chatReceive_callback(cm);
                    break;
            }
            return null;
        }

        public bool SendMessage(string _name,string _msg){
            try {
                string str = Json.ConvertToJson(new Packes.Chat(_name, _msg));
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }
    }
}