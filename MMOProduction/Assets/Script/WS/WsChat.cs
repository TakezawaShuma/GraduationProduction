﻿/////////////////////////////////
// チャット用のWebSocketクラス //
/////////////////////////////////

using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;

namespace WS
{
    public struct ChatMassege
    {
        public ChatMassege(string _name, string _mass) { name = _name; massege = _mass; }
        public string name;
        public string massege;
    }
    public class WsChat : WsBase
    {
        // チャットサーバーのポート
        private uint port = 8009;
        private static WsChat instance = null;
        // チャットコールバック
        public Action<Packes.RecvAllChat> allChatAction;

        public static WsChat Instance
        {
            get { if (instance == null) { instance = new WsChat(); } return instance; }
        }


        public WsChat()
        {
            Init(port);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="_port"></param>
        private void Init(uint _port)
        {
            base.Connect(_port);
            Receive();
        }


        public void Destroy()
        {
            base.Destroy("チャット切断");
            instance = null;
        }

        public override void Send(string _json)
        {
            if (base.ws.ReadyState == WebSocketState.Open)
            {
                base.ws.Send(_json);
            }
        }

        protected override void Receive()
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
                        case CommandData.RecvAllChat:
                            allChatAction(Json.ConvertToPackets<Packes.RecvAllChat>(e.Data));
                            break;
                    }
                }, e.Data);
            };
        }
    }
}