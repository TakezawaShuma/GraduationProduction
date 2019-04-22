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

        // 受信データ
        public Packes.RecvPosSync recv_data = new Packes.RecvPosSync();


        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart()
        {
            Connect();
            RelatedToWS();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy()
        {
            SendCutting();
            Debug.Log("ログインシーンの終了");
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
                Debug.Log("Data : " + e.Data);
            // 受信データからコマンドを取り出す
            CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

            // コマンドから受信データサイズを決定
            // コマンド内容はDatas.csを参照
            switch (com)
                {
                    case CommandData.CmdRecvPosSync:
                        recv_data = Receive(e);
                        break;
                    default:
                        break;
                }
                Debug.Log(recv_data.command);
                if (recv_data.Command == CommandData.CmdRecvPosSync)
                {
                // 此処に送られてきた情報を設定する関数を入れる
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
        /// <param name="send_data"></param>
        /// <returns></returns>
        public bool SendPlayData(int hp, int mp, float x, float y, float z)
        {
            Packes.SendPosSync send = SetSendData(hp, mp, x, y, z);
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
            // データの形の確認
            Debug.Log("Data: " + e.Data);
            // JSONをデシリアライズ
            Packes.RecvPosSync recv = JsonUtility.FromJson<Packes.RecvPosSync>(e.Data);
            return recv;

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
        public Packes.SendPosSync SetSendData(int hp,int mp,float x,float y,float z)
        {
            Packes.SendPosSync packet = new Packes.SendPosSync();
            packet.HP = hp;
            packet.MP = mp;
            packet.X = x;
            packet.Y = y;
            packet.Z = z;
            return packet;
        }

    }
}