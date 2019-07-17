using System;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

namespace Connect
{
    public class ConnectPlay : ConnectBase
    {
        // ログインサーバーのポート
        private int port = 8001;
        public Action<PlayerData> play_collback;

        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(Action<PlayerData> callback){
            play_collback = callback;
            base.Connect(port);
            RelatedToWS();
            SendInData();
        }       

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy() {
            SendCutting();
            base.Destroy();
        }

        /// <summary>
        /// データを受信したときの処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void RelatedToWS(){
            var context = SynchronizationContext.Current;

            // データが送られてくると呼ばれる
            ws.OnMessage += (sender, e) =>
            {
                // データ形の確認
                Debug.Log("Data : " + e.Data);
                context.Post(state =>
                {
                    // 受信データからコマンドを取り出す
                    CommandData com = (CommandData)int.Parse(e.Data.Substring(11, 3));

                    // コマンドから受信データサイズを決定
                    // コマンド内容はDatas.csを参照
                    switch (com)
                    {
                        case CommandData.RecvPosSync:
                            // 受信データ
                            Packes.RecvPosSync recv_data = JsonUtility.FromJson<Packes.RecvPosSync>(e.Data);
                            Debug.Log("listen user_id : " + recv_data.user_id.ToString());
                            play_collback(new PlayerData(recv_data));
                        break;
                        case CommandData.RecvInitialLogin:
                            Packes.RecvInitialLogin recv_in = JsonUtility.FromJson<Packes.RecvInitialLogin>(e.Data);
                            play_collback(new PlayerData(recv_in.user_id, recv_in.x, recv_in.y, recv_in.z, 0));
                            break;
                        default:
                            break;
                    }
                },e.Data);
            };
            base.WsInit();
        }

        /// <summary>
        /// ステータスの送信
        /// </summary>
        /// <returns></returns>
        public bool SendStatus(int _hp,int _mp,int _status) {
            try {
                string json = new Packes.SendStatus(_hp, _mp, _status).ToJson();
                ws.Send(json);
                Debug.Log(json);
            } catch {
                Debug.Log("Send Err");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 位置の送信
        /// </summary>
        /// <returns></returns>
        public bool SendPosData(float x, float y, float z,int dir)
        {
            try {
                string str = new Packes.SendPosSync(Retention.ID, x, y, z, dir).ToJson();
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// プレイシーンに入った事を報告
        /// </summary>
        /// <returns></returns>
        public bool SendInData()
        {
            Packes.SendInitialLogin send = new Packes.SendInitialLogin();
            try {
                string str = send.ToJson();
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ログアウトデータを送信する
        /// </summary>
        /// <returns></returns>
        private bool SendCutting() {
            Packes.Finished finished_packet = new Packes.Finished();
            try {
                string str = finished_packet.ToJson();
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }
    }
}