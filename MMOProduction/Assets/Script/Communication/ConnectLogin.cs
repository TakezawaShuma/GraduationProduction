using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Net;
using System;

namespace Connect
{
    public class ConnectLogin 
    {
        // ソケット
        private WebSocket ws;

        // シーン切り替え用フラグ
        Scenes scene_flag = Scenes.Non;

        //　サーバーのIP
        private const string server_ip = "172.24.52.250";

        // ログインサーバーのポート
        private const int port = 8000;

        // 受信データ
        private Packes.IPacketDatas i_data = null;

        // ログインコールバック
        private Action<int> login_callback;

        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(Action<int> callback)
        {
            // コールバックの設定
            //login_callback = callback;
            Connect();
            RelatedToWS();

            Send(702);
        }

        /// <summary>
        /// 接続処理
        /// </summary>
        private void Connect()
        {
            ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
            Debug.Log("IPアドレス : " + server_ip + " ポート : " + port);
            try
            {
                ws.Connect();
            }
            catch 
            {
                Debug.Log("サーバーへ接続ができません。");
            }
        }

        /// <summary>
        /// WebSocket関係のイベント
        /// </summary>
        private void RelatedToWS()
        {

            // 接続が確立したら呼ばれる
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket Open(Login)");
            };
            // データが送られてくると呼ばれる
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Data : " + e.Data);
                i_data = Receive(e);
                login_callback(i_data.command);
                Debug.Log(e.Data);
                if (i_data.Command == CommandData.CmdOKConfirmation || i_data.Command == CommandData.CmdCreateReport)
                {
                    Debug.Log("プレイシーンに移行します。");
                    scene_flag = Scenes.Play;
                }
            };

            // 通信にエラーが発生すると呼ばれる
            ws.OnError += (sender, e) =>
            {
                Debug.LogError("WebSocket Error Message: " + e.Message);
            };

            // 通信が切断されソケットが閉じられると呼ばれる
            ws.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket Close(Login)");
            };
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            Debug.Log("ログインシーンの終了");
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Packes.IPacketDatas Receive(MessageEventArgs e)
        {
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

            // コマンドで受信データサイズを変える
            // コマンド内容はDatas.csを参照
            switch (com)
            {
                case CommandData.CmdOKConfirmation: //103
                    Debug.Log(e.Data);
                    Packes.OKConfirmation ok = Json.ConvertToPackets<Packes.OKConfirmation>(e.Data);
                    // IDの保管
                    Retention.ID = ok.user_id;
                    Debug.Log(Retention.ID);
                    return ok;

                case CommandData.CmdMissingConfirmation: // 104
                    Debug.Log(e.Data);
                    Packes.MissingConfirmation miss = Json.ConvertToPackets<Packes.MissingConfirmation>(e.Data);
                    return miss;

                case CommandData.CmdCreateReport: //105
                    Packes.CreateReport create = Json.ConvertToPackets<Packes.CreateReport>(e.Data);
                    // IDの保管
                    Retention.ID = create.user_id;
                    Debug.Log(Retention.ID);
                    return create;

                case CommandData.CmdExisting: // 106
                    Packes.Existing existing = Json.ConvertToPackets<Packes.Existing>(e.Data);
                    return existing;

                default:
                    break;
            }
            return null;

        }

        /// <summary>
        /// ログインデータを送信する
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendLogin(string user_name, string pass)
        {
            Packes.Login login_packet = new Packes.Login(user_name, pass);

            // 送信の成否
            bool sf = true;
            try
            {
                string str = Json.ConvertToJson(login_packet);
                ws.Send(str);
                Debug.Log(str);
            }
            catch
            {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return sf;
        }

        public bool Send(int com)
        {
            Packes.IPacketDatas pack = null;
            switch (com)
            {
                case 702:
                    pack = new Packes.SendItemList();
                    break;
                case 703:
                    pack = new Packes.SendSkillList();
                    break;
                default:
                    break;
            }

            if (pack != null)
            {
                string str = Json.ConvertToJson(pack);
                ws.Send(str);
            }
            return true;
        }

        /// <summary>
        /// 新規登録データを送信する
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendRegistration(string user_name, string pass)
        {
            Packes.CreateUser create_user = new Packes.CreateUser(user_name, pass);

            // 送信の成否
            bool sf = true;
            try
            {
                string str = Json.ConvertToJson(create_user);
                Debug.Log(str);
                ws.Send(str);
            }
            catch
            {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return sf;
        }


        /// <summary>
        /// シーンを切り替える
        /// </summary>
        public void ChangeScene()
        {
            switch (scene_flag)
            {
                case Scenes.Login:
                    break;
                case Scenes.Play:
                    // ソケット削除
                    Destroy();
                    SceneManager.LoadScene("DebugPlay");
                    break;
                case Scenes.Non:
                    break;
                default:
                    break;
            }
        }
    }
}