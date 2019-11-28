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
        private bool logoutFlag = false;


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
        // 他のプレイヤーがスキルを使った 224
        public Action<Packes.OtherPlayerUseSkill> othersUseSkillAction;
        // エネミーの攻撃準備 225
        public Action<Packes.EnemyUseSkillRequest> enemySkillReqAction;
        // エネミーが攻撃する 226
        public Action<Packes.EnemyUseSkill> enemyUseSkillAction;
        // エネミーの攻撃結果
        public Action<Packes.EnemyAttackResult> enemyAttackAction;
        // 他プレイヤーがログアウトした 707
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
            logoutFlag = false;
            base.Connect(_port);
            Receive();

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            if (!logoutFlag) { Send(new Packes.LogoutCtoS(UserRecord.ID).ToJson()); }
            base.Destroy("プレイの終了");
            instance = null;
        }

        public void Logout()
        {
            Send(new Packes.LogoutCtoS(UserRecord.ID).ToJson());
            Debug.Log("ログアウト要請送信");
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
                    Debug.Log((int)command);

                    switch (command)
                    {
                        case CommandData.TranslationStoC:   // プレイヤーの移動を受信
                            //Packes.TranslationStoC posSync = Json.ConvertToPackets<Packes.TranslationStoC>(e.Data);
                            //moveingAction(posSync);
                            moveingAction(Json.ConvertToPackets<Packes.TranslationStoC>(e.Data));
                            break;

                        case CommandData.GetEnemyDataStoC:  // エネミーの位置情報を受信
                            //Packes.GetEnemyDataStoC init = Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data);
                            //enemysAction(init);
                            //Debug.Log("敵の位置情報");
                            enemysAction(Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data));
                            break;

                        case CommandData.StatusStoC:        // プレイヤーのステータス共有
                            //Packes.StatusStoC status = Json.ConvertToPackets<Packes.StatusStoC>(e.Data);
                            //statusAction(status);
                            //Debug.Log("ステータス共有");
                            statusAction(Json.ConvertToPackets<Packes.StatusStoC>(e.Data));
                            break;

                        case CommandData.LoadSaveData:      // セーブデータの受信
                            //Packes.LoadSaveData save = Json.ConvertToPackets<Packes.LoadSaveData>(e.Data);
                            //loadSaveAction(save);
                            loadSaveAction(Json.ConvertToPackets<Packes.LoadSaveData>(e.Data));
                            break;

                        case CommandData.LoadingFinishStoC: // セーブデータの受信関係の終了
                            //Packes.LoadingFinishStoC loadFin = Json.ConvertToPackets<Packes.LoadingFinishStoC>(e.Data);
                            //loadFinAction(loadFin);
                            loadFinAction(Json.ConvertToPackets<Packes.LoadingFinishStoC>(e.Data));
                            break;

                        case CommandData.EnemyAliveStoC:    // 戦闘結果(エネミーは生きている)
                            //Packes.EnemyAliveStoC alive = Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data);
                            //enemyAliveAction(alive);
                            enemyAliveAction(Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data));
                            break;

                        case CommandData.EnemyDieStoC:      // 戦闘結果(エネミーが死んだ)
                            //Packes.EnemyDieStoC enemyDie = Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data);
                            //enemyDeadAction(enemyDie);
                            enemyDeadAction(Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data));
                            break;

                        case CommandData.OtherPlayerUseSkill:   // 他プレイヤーがスキルを使った
                            //Packes.OtherPlayerUseSkill other = Json.ConvertToPackets<Packes.OtherPlayerUseSkill>(e.Data);
                            //othersUseSkillAction(other);
                            othersUseSkillAction(Json.ConvertToPackets<Packes.OtherPlayerUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyUseSkillRequest:  // 敵スキルの使用申請
                            //Packes.EnemyUseSkillRequest skillReq = Json.ConvertToPackets<Packes.EnemyUseSkillRequest>(e.Data);
                            //enemySkillReqAction(skillReq);
                            enemySkillReqAction(Json.ConvertToPackets<Packes.EnemyUseSkillRequest>(e.Data));
                            break;

                        case CommandData.EnemyUseSkill:     // 敵スキルの使用
                            Debug.Log("スキルを使用します。");
                            //Packes.EnemyUseSkill skillUse = Json.ConvertToPackets<Packes.EnemyUseSkill>(e.Data);
                            //enemyUseSkillAction(skillUse);
                            enemyUseSkillAction(Json.ConvertToPackets<Packes.EnemyUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyAttackResult: // 敵の攻撃結果の受信
                            Debug.Log("攻撃結果の受信");
                            //Packes.EnemyAttackResult attackRes = Json.ConvertToPackets<Packes.EnemyAttackResult>(e.Data);
                            //enemyAttackAction(attackRes);
                            enemyAttackAction(Json.ConvertToPackets<Packes.EnemyAttackResult>(e.Data));
                            break;

                        case CommandData.LogoutStoC:        // 他プレイヤーがログアウトした
                            //Packes.LogoutStoC logout = Json.ConvertToPackets<Packes.LogoutStoC>(e.Data);
                            //logoutAction(logout);
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