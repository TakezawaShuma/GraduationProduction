using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using System;
using System.Threading;

namespace Connect
{
    public class ConnectLogin : ConnectBase
    {
        // シーン切り替え用フラグ
        Scenes scene_flag = Scenes.Non;
        // ログインサーバーのポート
        private const int port = 8000;
        // 受信データ
        private Packes.IPacketDatas i_data = null;
        // ログインコールバック
        private Action<int> login_callback;

        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(Action<int> callback){
            // コールバックの設定
            login_callback = callback;
            base.Connect(port);
            RelatedToWS();
            Send(new Packes.SendItemList().ToJson());
        }

        /// <summary>
        /// WebSocket関係のイベント
        /// </summary>
        private void RelatedToWS() {
            var context = SynchronizationContext.Current;
            base.WsInit();
            ws.OnMessage += (sender, e) => {
                Debug.Log("Data : " + e.Data);
                context.Post(state => {
                    i_data = Receive(e);
                    login_callback(i_data.command);
                    Debug.Log(i_data.command);
                    if (i_data.Command == CommandData.OKConfirmation || i_data.Command == CommandData.CreateReport) {
                        Debug.Log("プレイシーンに移行します。");
                        ChangeScene();
                    }
                }, e.Data);
            };
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy() {
            base.Destroy("ログインWSの終了");
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Packes.IPacketDatas Receive(MessageEventArgs e) {
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));
            // コマンドで受信データサイズを変える
            // コマンド内容はDatas.csを参照
            switch (com) {
                case CommandData.OKConfirmation: //103
                    Debug.Log(e.Data);
                    Packes.OKConfirmation ok = Json.ConvertToPackets<Packes.OKConfirmation>(e.Data);
                    Retention.ID = ok.user_id;
                    return ok;

                case CommandData.MissingConfirmation: // 104
                    Debug.Log(e.Data);
                    return Json.ConvertToPackets<Packes.MissingConfirmation>(e.Data);

                case CommandData.CreateReport: //105
                    Packes.CreateReport create = Json.ConvertToPackets<Packes.CreateReport>(e.Data);
                    // IDの保管
                    Retention.ID = create.user_id;
                    return create;

                case CommandData.Existing: // 106
                    return Json.ConvertToPackets<Packes.Existing>(e.Data);

                default: break;
            }
            return null;
        }        

        /// <summary>
        /// ログインデータを送信する
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendLogin(string user_name, string pass) {
            Packes.Login login_packet = new Packes.Login(user_name, pass);
            // 送信の成否
            try{
                string str = login_packet.ToJson();
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="_data"></param>
        public void Send(string _data) {
            ws.Send(_data);
        }

        /// <summary>
        /// 新規登録データを送信する
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendRegistration(string user_name, string pass)
        {
            Packes.CreateUser create_user = new Packes.CreateUser(user_name, pass);
            try {
                string str = create_user.ToJson();
                Debug.Log(str);
                ws.Send(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// シーンを切り替える
        /// </summary>
        private void ChangeScene() {
            Destroy();
            SceneManager.LoadScene("PlayScene");
        }
    }
}