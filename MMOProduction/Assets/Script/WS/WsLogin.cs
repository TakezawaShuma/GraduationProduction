/////////////////////////////////
// ログイン用のWebSocketクラス //
/////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using System;
using System.Threading;

namespace WS
{
    public class WsLogin : WsBase
    {
        //ログインサーバーのポート
        private uint port = 8000;
        private static WsLogin instance = null;


        public Action<Packes.LoginError> errerAction;
        public Action<Packes.CreateOK> createAction;
        public Action<Packes.LoginOK> loginAction;
        // マップのマスター
        public Action<Packes.LoadingMapMaster> loadingMapMasterAction;
        // アクセサリのマスター
        public Action<Packes.LoadingAccessoryMaster> loadingAccessoryMasterAction;
        // クエストのマスター
        public Action<Packes.LoadingQuestMaster> loadingQuestMasterAction;
        public static WsLogin Instance
        {
            get
            {
                if (instance == null) instance = new WsLogin();
                return instance;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_port"></param>
        private WsLogin()
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

        /// </summary>
        /// <summary>
        /// 終了処理
        public void Destroy()
        {
            if (instance == null) return;
            base.Destroy("ログインWSの終了");
            instance = null;
        }

        public override void Send(string _json)
        {
            base.ws.Send(_json);
        }

        protected override void Receive()
        {
            var context = SynchronizationContext.Current;
            // 受信したデータが正常なものなら発火する
            base.ws.OnMessage += (sender, e) =>
            {
                context.Post(state =>
                {
                    // 受信データからコマンドを取り出す
                    CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
                    //Debug.Log(e.Data);
                    // コマンドで受信データサイズを変える
                    // コマンド内容はDatas.csを参照
                    switch (com)
                    {
                        case CommandData.LoginOK: //103
                            Packes.LoginOK login = Json.ConvertToPackets<Packes.LoginOK>(e.Data);
                            loginAction?.Invoke(login);
                            break;

                        case CommandData.LoginError: // 104
                            Packes.LoginError logError = Json.ConvertToPackets<Packes.LoginError>(e.Data);
                            errerAction?.Invoke(Json.ConvertToPackets<Packes.LoginError>(e.Data));
                            break;

                        case CommandData.CreateOK: //105
                            Packes.CreateOK create = Json.ConvertToPackets<Packes.CreateOK>(e.Data);
                            createAction?.Invoke(create);
                            break;

                        case CommandData.LoadingMapMaster:
                            Packes.LoadingMapMaster mapMaster = Json.ConvertToPackets<Packes.LoadingMapMaster>(e.Data);
                            loadingMapMasterAction?.Invoke(mapMaster);
                            break;

                        case CommandData.LoadingAccessoryMaster:
                            Packes.LoadingAccessoryMaster accessory = Json.ConvertToPackets<Packes.LoadingAccessoryMaster>(e.Data);
                            loadingAccessoryMasterAction?.Invoke(accessory);
                            break;

                        case CommandData.QuestMasterDataList:
                            Packes.LoadingQuestMaster quest = Json.ConvertToPackets<Packes.LoadingQuestMaster>(e.Data);
                            loadingQuestMasterAction?.Invoke(quest);
                            break;
                        // 随時追加

                        default: break;
                    }
                }, e.Data);
            };
        }


        /// <summary>
        /// 通信切断された時呼ばれるコールバック
        /// </summary>
        private void OnClosed()
        {
            // todo
            ChangeScene2Title();
        }
        /// <summary>
        /// シーンを切り替える　タイトル
        /// </summary>
        private void ChangeScene2Title()
        {
            Destroy();
        }

    }
}