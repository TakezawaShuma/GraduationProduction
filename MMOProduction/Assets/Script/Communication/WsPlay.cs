/////////////////////////////
// プレイ用WebSocketクラス //
/////////////////////////////


using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using WebSocketSharp;
using System.Threading.Tasks;

namespace WS
{
    public class WsPlay : WsBase
    {
        // ログインサーバーのポート
        private int port = 8001;
        public Action<PlayerData> player_collback;
        public Action<SaveData> save_collback;

        private Action<PlayerData, SaveData> player_call;
        private Action<PlayerData, SaveData> save_call;
        private List<Action<PlayerData, SaveData>> callback_list;


        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(Action<PlayerData> _player, Action<SaveData> _save)
        {
            player_collback = _player;
            save_collback = _save;
            base.Connect(port);

            RelatedToWS();
            SendRequestSaveData();

        }

        /// <summary>
        /// 初期処理を纏めた
        /// </summary>
        public void ConnectionStart(List<Action<PlayerData, SaveData>> _callback)
        {
            callback_list = _callback;
           

            base.Connect(port);

            RelatedToWS();
            SendRequestSaveData();

        }

        public void Update()
        { 
            base.ForDebug();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Destroy() {
            SendCutting();
            base.Destroy("プレイの終了");
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
                            Packes.RecvPosSync recv_pos = JsonUtility.FromJson<Packes.RecvPosSync>(e.Data);
                            Debug.Log("command : "+recv_pos.command.ToString()+" listen user_id : " + recv_pos.user_id.ToString());
                            //player_collback(new PlayerData(recv_pos)); //debug
                            callback_list[0](new PlayerData(recv_pos), new SaveData());
                        break;
                        case CommandData.RecvInitialLogin:
                            Packes.RecvInitialLogin recv_in = JsonUtility.FromJson<Packes.RecvInitialLogin>(e.Data);
                            Debug.Log("command : " + recv_in.command.ToString());
                            //player_collback(new PlayerData(recv_in.user_id, recv_in.x, recv_in.y, recv_in.z, 0)); // debug
                            callback_list[0](new PlayerData(recv_in.user_id, recv_in.x, recv_in.y, recv_in.z, 0), new SaveData());
                            break;
                        case CommandData.RecvSaveData:
                            Packes.RecvSaveData recv_save = JsonUtility.FromJson<Packes.RecvSaveData>(e.Data);
                            Debug.Log("command : " + recv_save.command.ToString());
                            //save_collback(new SaveData(recv_save.weapon, recv_save.position, recv_save.lv, recv_save.exp)); // debug
                            callback_list[1](new PlayerData(), new SaveData(recv_save.weapon, recv_save.position, recv_save.lv, recv_save.exp));
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
        /// 位置情報の送信
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
            try {
                string str = new Packes.SendInitialLogin(Retention.ID).ToJson();
                ws.Send(str);
                Debug.Log(str);
            } catch {
                Debug.Log("送信に失敗しました。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// セーブデータを要請する
        /// </summary>
        public void SendRequestSaveData()
        {
            try
            {
                string str = new Packes.SaveDataRequ().ToJson();
                ws.Send(str);
            }catch{
                Debug.Log("セーブデータ要請失敗しました。");
            }
        }

        /// <summary>
        /// セーブデータを送信する
        /// </summary>
        public void SendSaveDataOK()
        {
            try
            {
                string str = new Packes.DataLoadComplete().ToJson();
                ws.Send(str);
            }catch{
                Debug.Log("読み込み完了報告失敗");
            }
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