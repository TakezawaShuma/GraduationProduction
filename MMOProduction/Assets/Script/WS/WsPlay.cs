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
        // セーブデータの読み込み 212
        public Action<Packes.SaveLoadStoC> loadSaveAction;
        // プレイシーンにいる他ユーザーの一覧 214
        public Action<Packes.OtherPlayerList> loadOtherListAction;
        // 新しく入室してきたプレイヤーの情報 215
        public Action<Packes.NewOtherUser> loadOtherAction;
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
        // 検索したプレイヤー情報を受け取る 712
        public Action<Packes.FindOfPlayerStoC> findResultsAction;

        // アクセサリのマスター
        public Action<Packes.LoadingAccessoryMaster> loadingAccessoryMasterAction;

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
            UserRecord.DiscardAll();
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
        public override void Send(string _json)
        {
            //Debug.Log(int.Parse(_json);
            if (base.ws.ReadyState == WebSocketState.Open)
            {
                base.ws.Send(_json);
            }
        }

        /// <summary>
        /// 受信処理
        /// </summary>
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
                    //Debug.Log((int)command);

                    switch (command)
                    {
                        case CommandData.TranslationStoC:   // プレイヤーの移動を受信
                            //Debug.Log("プレイヤーの位置情報 : " + e.Data);
                            //Packes.TranslationStoC posSync = Json.ConvertToPackets<Packes.TranslationStoC>(e.Data);
                            //moveingAction?.Invoke(posSync);
                            moveingAction?.Invoke(Json.ConvertToPackets<Packes.TranslationStoC>(e.Data));
                            break;

                        case CommandData.GetEnemyDataStoC:  // エネミーの位置情報を受信
                            //Debug.Log("敵位置情報 : " + e.Data);
                            //Packes.GetEnemyDataStoC init = Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data);
                            //enemysAction?.Invoke(init);
                            enemysAction?.Invoke(Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data));

                            break;

                        case CommandData.StatusStoC:        // プレイヤーのステータス共有
                            //Debug.Log("ステータス共有 : " + e.Data);
                            //Packes.StatusStoC status = Json.ConvertToPackets<Packes.StatusStoC>(e.Data);
                            //statusAction?.Invoke(status);

                            statusAction?.Invoke(Json.ConvertToPackets<Packes.StatusStoC>(e.Data));
                            break;

                        case CommandData.SaveLoadStoC:      // セーブデータの受信
                            //Debug.Log("セーブデータ : " + e.Data);
                            //Packes.LoadSaveData save = Json.ConvertToPackets<Packes.LoadSaveData>(e.Data);
                            //loadSaveAction?.Invoke(save);
                            loadSaveAction?.Invoke(Json.ConvertToPackets<Packes.SaveLoadStoC>(e.Data));
                            break;

                        case CommandData.OtherPlayerList: // 他プレイヤーの一覧を取得
                            //Debug.Log("プレイヤー一覧 : " + e.Data);
                            //Packes.OtherPlayerList otherlist = Json.ConvertToPackets<Packes.OtherPlayerList>(e.Data);
                            //loadOtherListAction?.Invoke(otherlist);
                            loadOtherListAction?.Invoke(Json.ConvertToPackets<Packes.OtherPlayerList>(e.Data));
                            break;

                        case CommandData.NewOtherUser:  // 新規入室プレイヤーの取得
                            //Debug.Log("新規入室プレイヤー : " + e.Data);
                            //Packes.NewOtherUser loadFin = Json.ConvertToPackets<Packes.NewOtherUser>(e.Data);
                            //loadOtherAction?.Invoke(loadFin);
                            loadOtherAction?.Invoke(Json.ConvertToPackets<Packes.NewOtherUser>(e.Data));
                            break;

                        case CommandData.EnemyAliveStoC:    // 戦闘結果(エネミーは生きている)
                            //Debug.Log("戦闘結果(敵生存の場合) : " + e.Data);
                            //Packes.EnemyAliveStoC alive = Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data);
                            //enemyAliveAction?.Invoke(alive);
                            enemyAliveAction?.Invoke(Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data));
                            break;

                        case CommandData.EnemyDieStoC:      // 戦闘結果(エネミーが死んだ)
                            //Debug.Log("戦闘結果(敵死亡の場合) : " + e.Data);
                            //Packes.EnemyDieStoC enemyDie = Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data);
                            //enemyDeadAction?.Invoke(enemyDie);
                            enemyDeadAction?.Invoke(Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data));
                            break;

                        case CommandData.OtherPlayerUseSkill:   // 他プレイヤーがスキルを使った
                            //Debug.Log("他プレイヤーのスキル使用 : " + e.Data);
                            //Packes.OtherPlayerUseSkill other = Json.ConvertToPackets<Packes.OtherPlayerUseSkill>(e.Data);
                            //othersUseSkillAction?.Invoke(other);
                            othersUseSkillAction?.Invoke(Json.ConvertToPackets<Packes.OtherPlayerUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyUseSkillRequest:  // 敵スキルの使用申請
                            //Debug.Log("敵スキルの使用申請 : " + e.Data);
                            //Packes.EnemyUseSkillRequest skillReq = Json.ConvertToPackets<Packes.EnemyUseSkillRequest>(e.Data);
                            //enemySkillReqAction?.Invoke(skillReq);
                            enemySkillReqAction?.Invoke(Json.ConvertToPackets<Packes.EnemyUseSkillRequest>(e.Data));
                            break;

                        case CommandData.EnemyUseSkill:     // 敵スキルの使用
                            //Debug.Log("敵スキルの発動 : " + e.Data);
                            //Packes.EnemyUseSkill skillUse = Json.ConvertToPackets<Packes.EnemyUseSkill>(e.Data);
                            //enemyUseSkillAction?.Invoke(skillUse);
                            enemyUseSkillAction?.Invoke(Json.ConvertToPackets<Packes.EnemyUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyAttackResult: // 敵の攻撃結果の受信
                            //Debug.Log("敵スキルの結果 : " + e.Data);
                            //Packes.EnemyAttackResult attackRes = Json.ConvertToPackets<Packes.EnemyAttackResult>(e.Data);
                            //enemyAttackAction?.Invoke(attackRes);
                            enemyAttackAction?.Invoke(Json.ConvertToPackets<Packes.EnemyAttackResult>(e.Data));
                            break;

                        case CommandData.LogoutStoC:        // 他プレイヤーがログアウトした
                            //Debug.Log("プレイヤーのログアウト : " + e.Data);
                            //Packes.LogoutStoC logout = Json.ConvertToPackets<Packes.LogoutStoC>(e.Data);
                            //logoutAction?.Invoke(logout);
                            if (Json.ConvertToPackets<Packes.LogoutStoC>(e.Data).user_id == UserRecord.ID) { logoutFlag = true; }
                            logoutAction?.Invoke(Json.ConvertToPackets<Packes.LogoutStoC>(e.Data));
                            break;

                        case CommandData.FindOfPlayerStoC:  // 
                            //Packes.FindOfPlayerStoC find = Json.ConvertToPackets<Packes.FindOfPlayerStoC>(e.Data);
                            //findResultsAction?.Invoke(find);

                            findResultsAction?.Invoke(Json.ConvertToPackets<Packes.FindOfPlayerStoC>(e.Data));
                            break;
                        case CommandData.RecvAccessory:
                            loadingAccessoryMasterAction?.Invoke(Json.ConvertToPackets<Packes.LoadingAccessoryMaster>(e.Data));
                            break;
                        // 随時追加
                        default:
                            Debug.Log(e.Data);
                            break;
                    }

                }, e.Data);
            };

        }
    }
}