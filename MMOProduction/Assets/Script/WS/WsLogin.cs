﻿/////////////////////////////////
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

        public Action<int> errerAction;

        public static WsLogin Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WsLogin();

                }
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
            base.Destroy("ログインWSの終了");
        }

        public void Send(string _json)
        {
            base.ws.Send(_json);
        }

        public void Receive()
        {
            var context = SynchronizationContext.Current;
            // 受信したデータが正常なものなら発火する
            base.ws.OnMessage += (sender, e) =>
            {
                context.Post(state =>
                {
                    // 受信データからコマンドを取り出す
                    CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
                    // コマンドで受信データサイズを変える
                    // コマンド内容はDatas.csを参照
                    switch (com)
                    {
                        case CommandData.LoginOK: //103
                            Packes.LoginOK login = Json.ConvertToPackets<Packes.LoginOK>(e.Data);
                            // IDの保管
                            UserRecord.ID = login.user_id;
                            ChangeScenetoPlay();
                            break;

                        case CommandData.LoginError: // 104
                            Packes.LoginError logError = Json.ConvertToPackets<Packes.LoginError>(e.Data);
                            errerAction((int)com);
                            break;

                        case CommandData.CreateOK: //105
                            Packes.CreateOK create = Json.ConvertToPackets<Packes.CreateOK>(e.Data);
                            // IDの保管
                            UserRecord.ID = create.user_id;
                            ChangeScenetoPlay();
                            break;

                        // 随時追加

                        default: break;
                    }
                }, e.Data);
            };
        }

        /// <summary>
        /// シーンを切り替える プレイ
        /// </summary>
        private void ChangeScenetoPlay() {
            SceneManager.LoadScene("LoadingScene");
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
            //SceneManager.LoadScene("TitleScene");
        }

    }
}