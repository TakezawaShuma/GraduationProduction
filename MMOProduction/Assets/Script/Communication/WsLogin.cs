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
        //private const int port = 8000;

        public Action<int> errerAction;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_port"></param>
        public WsLogin(uint _port)
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
                            //Debug.Log(e.Data);
                            Packes.LoginOK login = Json.ConvertToPackets<Packes.LoginOK>(e.Data);
                            // IDの保管
                            Retention.ID = login.user_id;
                            //Debug.Log("command : " + login.command + " , id : " + login.user_id);
                            //Debug.Log("プレイシーンに移行します。");
                            ChangeScenetoPlay();
                            break;

                        case CommandData.LoginError: // 104
                            Packes.LoginError logError = Json.ConvertToPackets<Packes.LoginError>(e.Data);

                            //Debug.Log("command : " + logError.command);
                            errerAction((int)com);
                            break;

                        case CommandData.CreateOK: //105
                            Packes.CreateOK create = Json.ConvertToPackets<Packes.CreateOK>(e.Data);
                            // IDの保管
                            Retention.ID = create.user_id;
                            //Debug.Log("command : " + create.command + " , id : " + create.user_id);
                            //Debug.Log("プレイシーンに移行します。");
                            ChangeScenetoPlay();
                            break;

                        // 随時追加

                        default: break;
                    }
                }, e.Data);
            };
        }


        ///// <summary>
        ///// ログインデータを送信する
        ///// </summary>
        ///// <param name="Data"></param>
        ///// <returns></returns>
        //public bool SendLogin(string user_name, string pass) {
        //    Packes.Login login_packet = new Packes.Login(user_name, pass);
        //    // 送信の成否
        //    try {
        //        string str = login_packet.ToJson();
        //        ws.Send(str);
        //        Debug.Log(str);
        //    } catch {
        //        Debug.Log("送信に失敗しました。");
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 送信
        ///// </summary>
        ///// <param name="_data"></param>
        //public void Send(string _data) {
        //    ws.Send(_data);
        //}

        ///// <summary>
        ///// 新規登録データを送信する
        ///// </summary>
        ///// <param name="Data"></param>
        ///// <returns></returns>
        //public bool SendRegistration(string user_name, string pass)
        //{
        //    Packes.CreateUser create_user = new Packes.CreateUser(user_name, pass);
        //    try {
        //        string str = create_user.ToJson();
        //        Debug.Log(str);
        //        ws.Send(str);
        //    } catch {
        //        Debug.Log("送信に失敗しました。");
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// シーンを切り替える プレイ
        /// </summary>
        private void ChangeScenetoPlay() {
            SceneManager.LoadScene("PlayScene");
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