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
        // ログインサーバーのポート
        private uint port = 8001;
        private static WsPlay instance = null;


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
        // エネミーが生存している 221
        public Action<Packes.EnemyAliveStoC> enemyAliveAction;
        // エネミーが死んだ 222
        public Action<Packes.EnemyDieStoC> enemyDeadAction;

        public Action<Packes.LogoutStoC> logoutAction;

        public static WsPlay Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WsPlay();

                }
                return instance;
            }
        }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_port"></param>
        private WsPlay()
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

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            Send(new Packes.LogoutCtoS(UserRecord.ID).ToJson());
            base.Destroy("プレイの終了");
        }

        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="_json"></param>
        public void Send(string _json)
        {
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
                            moveingAction(posSync);
                            break;

                        case CommandData.GetEnemyDataStoC:
                            Packes.GetEnemyDataStoC init = Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data);
                            enemysAction(init);
                            break;

                        case CommandData.StatusStoC:
                            Packes.StatusStoC status = Json.ConvertToPackets<Packes.StatusStoC>(e.Data);
                            statusAction(status);
                            break;

                        case CommandData.LoadSaveData:
                            Packes.LoadSaveData save = Json.ConvertToPackets<Packes.LoadSaveData>(e.Data);
                            loadSaveAction(save);
                            break;

                        case CommandData.LoadingFinishStoC:
                            Packes.LoadingFinishStoC loadFin = Json.ConvertToPackets<Packes.LoadingFinishStoC>(e.Data);
                            loadFinAction(loadFin);
                            break;

                        case CommandData.EnemyAliveStoC:
                            Packes.EnemyAliveStoC alive = Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data);
                            enemyAliveAction(alive);
                            break;

                        case CommandData.EnemyDieStoC:
                            enemyDeadAction(Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data));
                            break;
                        case CommandData.LogoutStoC:
                            logoutAction(Json.ConvertToPackets<Packes.LogoutStoC>(e.Data));
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