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

        // ログアウト確認用フラグ　false:logout/true:login
        private bool isLogin = false;

        private bool moveMap = false;
        public bool MoveMap
        {
            get { return moveMap; }
            set { moveMap = value; }
        }


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
        // エネミーの攻撃結果 227
        public Action<Packes.EnemyAttackResult> enemyAttackAction;
        // マップ移動 252
        public Action<Packes.MoveingMapOk> mapAction;
        // 報酬選択 254
        public Action<Packes.SelectRewardOk> rewardAction;
        // 他プレイヤーがログアウトした 707
        public Action<Packes.LogoutStoC> logoutAction;
        // 検索したプレイヤー情報を受け取る 712
        public Action<Packes.FindOfPlayerStoC> findResultsAction;
        // クエストクリア
        public Action<Packes.QuestClear> questClearAction;
        // アクセサリのマスター
        public Action<Packes.LoadingAccessoryMaster> loadingAccessoryMasterAction;
        // キャラクターの新規作成
        public Action<Packes.SaveModelType> saveModelAction;
        // ステータスの更新
        public Action<Packes.GetParameter> getParameterAction;
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
            isLogin = true;
            base.Connect(_port);
            Receive();

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            if (instance == null) return;
            if (isLogin) Logout();

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
            if (base.ws == null) return;
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
                    Debug.Log((int)command);

                    switch (command)
                    {
                        case CommandData.TranslationStoC:   // プレイヤーの移動を受信
                            //Debug.Log("プレイヤーの位置情報 : " + e.Data);
                            moveingAction?.Invoke(Json.ConvertToPackets<Packes.TranslationStoC>(e.Data));
                            break;

                        case CommandData.GetEnemyDataStoC:  // エネミーの位置情報を受信
                            //Debug.Log("敵位置情報 : " + e.Data);
                            enemysAction?.Invoke(Json.ConvertToPackets<Packes.GetEnemyDataStoC>(e.Data));

                            break;

                        case CommandData.StatusStoC:        // プレイヤーのステータス共有
                            //Debug.Log("ステータス共有 : " + e.Data);
                            statusAction?.Invoke(Json.ConvertToPackets<Packes.StatusStoC>(e.Data));
                            break;

                        case CommandData.SaveLoadStoC:      // セーブデータの受信
                            //Debug.Log("セーブデータ : " + e.Data);
                            loadSaveAction?.Invoke(Json.ConvertToPackets<Packes.SaveLoadStoC>(e.Data));
                            break;

                        case CommandData.OtherPlayerList: // 他プレイヤーの一覧を取得
                            //Debug.Log("プレイヤー一覧 : " + e.Data);
                            loadOtherListAction?.Invoke(Json.ConvertToPackets<Packes.OtherPlayerList>(e.Data));
                            break;

                        case CommandData.NewOtherUser:  // 新規入室プレイヤーの取得
                            //Debug.Log("新規入室プレイヤー : " + e.Data);
                            loadOtherAction?.Invoke(Json.ConvertToPackets<Packes.NewOtherUser>(e.Data));
                            break;

                        case CommandData.EnemyAliveStoC:    // 戦闘結果(エネミーは生きている)
                            //Debug.Log("戦闘結果(敵生存の場合) : " + e.Data);
                            enemyAliveAction?.Invoke(Json.ConvertToPackets<Packes.EnemyAliveStoC>(e.Data));
                            break;

                        case CommandData.EnemyDieStoC:      // 戦闘結果(エネミーが死んだ)
                            //Debug.Log("戦闘結果(敵死亡の場合) : " + e.Data);
                            enemyDeadAction?.Invoke(Json.ConvertToPackets<Packes.EnemyDieStoC>(e.Data));
                            break;

                        case CommandData.OtherPlayerUseSkill:   // 他プレイヤーがスキルを使った
                            //Debug.Log("他プレイヤーのスキル使用 : " + e.Data);
                            othersUseSkillAction?.Invoke(Json.ConvertToPackets<Packes.OtherPlayerUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyUseSkillRequest:  // 敵スキルの使用申請
                            //Debug.Log("敵スキルの使用申請 : " + e.Data);
                            enemySkillReqAction?.Invoke(Json.ConvertToPackets<Packes.EnemyUseSkillRequest>(e.Data));
                            break;

                        case CommandData.EnemyUseSkill:     // 敵スキルの使用
                            //Debug.Log("敵スキルの発動 : " + e.Data);
                            enemyUseSkillAction?.Invoke(Json.ConvertToPackets<Packes.EnemyUseSkill>(e.Data));
                            break;

                        case CommandData.EnemyAttackResult: // 敵の攻撃結果の受信
                            //Debug.Log("敵スキルの結果 : " + e.Data);
                            enemyAttackAction?.Invoke(Json.ConvertToPackets<Packes.EnemyAttackResult>(e.Data));
                            break;

                        case CommandData.MoveingMapOk:
                            //Debug.Log("マップ移動結果 : " + e.Data);
                            moveMap = true;
                            mapAction?.Invoke(Json.ConvertToPackets<Packes.MoveingMapOk>(e.Data));
                            break;

                        case CommandData.SelectRewardOk:
                            //Debug.Log("獲得報酬結果 : " + e.Data);
                            rewardAction?.Invoke(Json.ConvertToPackets<Packes.SelectRewardOk>(e.Data));
                            break;

                        case CommandData.LogoutStoC:        // プレイヤーがログアウトした
                            //Debug.Log("プレイヤーのログアウト : " + e.Data);
                            if (Json.ConvertToPackets<Packes.LogoutStoC>(e.Data).user_id == UserRecord.ID) { isLogin = false; }
                            logoutAction?.Invoke(Json.ConvertToPackets<Packes.LogoutStoC>(e.Data));
                            break;

                        case CommandData.FindOfPlayerStoC:  // キャラクターの詳細を取得
                            //Debug.Log("キャラクターの詳細 : " + e.Data);
                            findResultsAction?.Invoke(Json.ConvertToPackets<Packes.FindOfPlayerStoC>(e.Data));
                            break;
                        case CommandData.LoadingAccessoryMaster:
                            loadingAccessoryMasterAction?.Invoke(Json.ConvertToPackets<Packes.LoadingAccessoryMaster>(e.Data));
                            break;
                        case CommandData.QuestClear:
                            questClearAction?.Invoke(Json.ConvertToPackets<Packes.QuestClear>(e.Data));
                            break;
                        case CommandData.SaveModelType:
                            saveModelAction?.Invoke(Json.ConvertToPackets<Packes.SaveModelType>(e.Data));
                            break;
                        case CommandData.GetParameter:
                            getParameterAction?.Invoke(Json.ConvertToPackets<Packes.GetParameter>(e.Data));
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