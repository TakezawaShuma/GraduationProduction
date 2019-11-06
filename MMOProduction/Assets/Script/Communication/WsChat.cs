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
    }
}