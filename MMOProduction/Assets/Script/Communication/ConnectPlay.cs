using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Connect
{
    public class ConnectPlay 
    {
        // ソケット
        private WebSocket ws;

        //　サーバーのIP
        string server_ip = "172.24.52.250";
        // ログインサーバーのポート
        private int port = 8001;


        public System.Action<int, int, int, float, float, float,float> play_collback;

        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(System.Action<int, int, int, float, float, float,float> callback)
        {
            play_collback = callback;
            Connect();
            RelatedToWS();
            SendInData();
        }
        

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            SendCutting();
            Debug.Log("プレイシーンの終了");
            ws.Close();
            ws = null;
        }

        /// <summary>
        /// 接続処理
        /// </summary>
        private void Connect()
        {
            // WebSocketの作成
            ws = new WebSocket("ws://" + server_ip + ":" + port.ToString());
            Debug.Log("IPアドレス : " + server_ip + "ポート : " + port);
            // 接続が確立したら呼ばれる
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket Open(Play)");
            };
            ws.Connect();
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void RelatedToWS()
        {
            // データが送られてくると呼ばれる
            ws.OnMessage += (sender, e) =>
            {
                // データ形の確認
                Debug.Log("Data : " + e.Data);
                // 受信データからコマンドを取り出す
                CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

                // コマンドから受信データサイズを決定
                // コマンド内容はDatas.csを参照
                switch (com)
                {
                    case CommandData.CmdRecvPosSync:
                        // 受信データ
                        Packes.RecvPosSync recv_data = new Packes.RecvPosSync();
                        recv_data = Receive(e);
                        play_collback(recv_data.user_id, recv_data.hp, recv_data.mp, recv_data.x, recv_data.y, recv_data.z, recv_data.dir);
                        break;
                    case CommandData.CmdRecvInitialLogin:
                        Packes.RecvInitialLogin recv_in = new Packes.RecvInitialLogin();
                        recv_in = ReceiveIn(e);
                        play_collback(recv_in.user_id, 0, 0, recv_in.x, recv_in.y, recv_in.z, 0);
                        break;
                    default:
                        break;
                }

            };

            // 通信にエラーが発生すると呼ばれる
            ws.OnError += (sender, e) =>
            {
                Debug.Log("WebSocket Error Message: " + e.Message);
            };
            // 通信が切断されソケットが閉じられると呼ばれる
            ws.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket Close(Play)");
            };
        }


        /// <summary>
        /// サーバーにデータを送信する
        /// </summary>
        /// <returns></returns>
        public bool SendPosData(int hp, int mp, float x, float y, float z,int dir)
        {
            Packes.SendPosSync send = SetSendData(hp, mp, x, y, z, dir);
            // 送信の成否
            bool sf = true;
            try
            {
                string str = ConvertToJson(send);
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

        /// <summary>
        /// プレイシーンに入った事を報告
        /// </summary>
        /// <returns></returns>
        public bool SendInData()
        {
            Packes.SendInitialLogin send = new Packes.SendInitialLogin();
            send.user_id = Retention.ID;
            try
            {
                string str = ConvertToJson(send);
                ws.Send(str);
                Debug.Log(str);
            }
            catch
            {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }
        /// <summary>
        /// ログアウトデータを送信する
        /// </summary>
        /// <returns></returns>
        private bool SendCutting()
        {
            Packes.Finished finished_packet = new Packes.Finished();

            // 送信の成否
            bool sf = true;
            try
            {
                string str = ConvertToJson(finished_packet);
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

        /// <summary>
        /// 送るデータを設定する
        /// </summary>
        /// <param name="data"></param>
        private Packes.SendPosSync SetSendData(int hp, int mp, float x, float y, float z, int dir)
        {
            Packes.SendPosSync packet = new Packes.SendPosSync();
            packet.user_id = Retention.ID;
            packet.hp = hp;
            packet.mp = mp;
            packet.x = x;
            packet.y = y;
            packet.z = z;
            packet.dir = dir;
            return packet;
        }
        /// <summary>
        /// パケットをJSON形式に変換する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ConvertToJson(Packes.IPacketDatas data)
        {
            string json = JsonUtility.ToJson(data);
            return json;
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Packes.RecvPosSync Receive(MessageEventArgs e)
        {
            // JSONをデシリアライズ
            Packes.RecvPosSync recv = JsonUtility.FromJson<Packes.RecvPosSync>(e.Data);
            return recv;

        }
        /// <summary>
        /// 初期ログイン確認
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Packes.RecvInitialLogin ReceiveIn(MessageEventArgs e)
        {

            Packes.RecvInitialLogin init = JsonUtility.FromJson<Packes.RecvInitialLogin>(e.Data);
            return init;

        }


    }
}