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
    public class WsChat:WsBase
    {


        //// ログインサーバーのポート
        //private const int port = 8009;
        // 受信データ
        //private Packes.IPacketDatas i_data = null;
        // チャットコールバック
        private Action<string, string> chatReceive_callback;


        public WsChat(uint _port)
        {
            Init(_port);
        }

        private void Init(uint _port)
        {
            base.Connect(_port);
        }

        public void Destroy()
        {
            base.Destroy("チャット切断");
        }

        public void Send(string _json)
        {
            //Debug.Log(_json);
            base.ws.Send(_json);
        }

        private void Receive()
        {
            var context = SynchronizationContext.Current;
            // 受信したデータが正常なものなら発火する
            base.ws.OnMessage += (sender, e) =>
            {
                context.Post(state =>
                {
                    // 受信したデータからコマンドを取り出す
                    var command = (CommandData)int.Parse(e.Data.Substring(11, 3));
                    switch (command)
                    {
                        default:
                            break;
                    }

                }, e.Data);
            };
        }

        //public void ConnectionStart(Action<string, string> _callback)
        //{
        //    chatReceive_callback = _callback;
        //    base.Connect(port);
        //    RelatedToWS();
        //}


        ///// <summary>
        ///// WebSocket関係のイベント
        ///// </summary>
        //private void RelatedToWS() {
        //     var context = SynchronizationContext.Current;
        //    base.WsInit();
        //    //ws.OnMessage += (sender, e) => {
        //    //    // データ形の確認
        //    //    Debug.Log("Data : " + e.Data);
        //    //    context.Post(state =>{
        //    //        Receive(e);
        //    //    }, e.Data);
        //    //};

        //}

        ///// <summary>
        ///// 終了処理
        ///// </summary>
        //public void Destroy(){
        //    Debug.Log("ログインシーンの終了");
        //    base.Destroy("チャット終了");
        //}

        ///// <summary>
        ///// データを受信したときの処理
        ///// </summary>
        ///// <param name="e"></param>
        ///// <returns></returns>
        //private Packes.IPacketDatas Receive(MessageEventArgs e){
        //    // 受信データからコマンドを取り出す
        //    CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
        //    // コマンドで受信データサイズを変える
        //    // コマンド内容はDatas.csを参照
        //    switch (com){
        //        case CommandData.RecvAllChat:
        //            // JSONをデシリアライズ
        //            Packes.RecvAllChat chat = JsonUtility.FromJson<Packes.RecvAllChat>(e.Data);
        //            chatReceive_callback(chat.user_name, chat.message);
        //            break;
        //    }
        //    return null;
        //}

        //public bool SendMessage(string _name,string _msg){
        //    try {
        //        string str = Json.ConvertToJson(new Packes.SendAllChat(_name, _msg));
        //        ws.Send(str);
        //        Debug.Log(str);
        //    } catch {
        //        Debug.Log("送信に失敗しました。");
        //        return false;
        //    }
        //    return true;
        //}
    }
}