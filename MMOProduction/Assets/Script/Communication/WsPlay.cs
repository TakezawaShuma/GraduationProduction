/////////////////////////////
// プレイ用WebSocketクラス //
/////////////////////////////

using System;
using System.Threading;
using UnityEngine;
using WebSocketSharp;
using System.Threading.Tasks;

namespace WS
{
    public class WsPlay : WsBase
    {
        //// ログインサーバーのポート
        //private int port = 8001;
        // 位置同期 202
        public Action<Packes.TranslationStoC> moveingAction;
        // エネミーの位置同期と状態 204
        public Action<Packes.GetEnemyDataStoC> enemysAction;
        // 状態 206
        public Action<Packes.StatusStoC> statusAction;
        // セーブ読み込み 210
        public Action<Packes.LoadSaveData> loadSaveAction;
        // ロード終了 212
        public Action<Packes.LoadingFinishStoC> loadFinAction;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_port"></param>
        public WsPlay(uint _port)
        {
            Init(_port);
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

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            Send(new Packes.LogoutCtoS(Retention.ID).ToJson());
            base.Destroy("プレイの終了");
        }

        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="_json"></param>
        public void Send(string _json)
        {
            Debug.Log("Send data : " + int.Parse(_json.Substring(11,3)));
            if (base.ws.ReadyState == WebSocketState.Open)
            {
                base.ws.Send(_json);
            }
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
                        case CommandData.TranslationStoC:
                            Packes.TranslationStoC posSync = Json.ConvertToPackets<Packes.TranslationStoC>(e.Data);
                            Debug.Log("command : " + posSync.command + " , user_id : " + posSync.user_id + 
                                " , position : (" + posSync.x + "," + posSync.y + "," + posSync.z + ") , direction : " + posSync.dir);
                            moveingAction(posSync);
                            break;

                        case CommandData.GetEnemyDataStoC:
                            Packes.GetEnemyDataStoC init = Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data);
                            Debug.Log("command : " + init.command + " , enemys"+init.enemys.Count);
                            enemysAction(init);
                            break;

                        case CommandData.StatusStoC:
                            Packes.StatusStoC status = Json.ConvertToPackets<Packes.StatusStoC>(e.Data);
                            Debug.Log("command : " + status.command + " , user_id : " + status.user_id);
                            statusAction(status);
                            break;

                        case CommandData.LoadSaveData:
                            Packes.LoadSaveData save = Json.ConvertToPackets<Packes.LoadSaveData>(e.Data);
                            Debug.Log("command : " + save.command);
                            loadSaveAction(save);
                            break;

                        case CommandData.LoadingFinishStoC:
                            Packes.LoadingFinishStoC loadFin = Json.ConvertToPackets<Packes.LoadingFinishStoC>(e.Data);
                            Debug.Log("command : " + loadFin.command);
                            loadFinAction(loadFin);
                            break;
                            
                        case CommandData.LogoutStoC:
                            break;
                        // 随時追加
                        default:
                            break;
                    }

                }, e.Data);
            };

        }
    }
}