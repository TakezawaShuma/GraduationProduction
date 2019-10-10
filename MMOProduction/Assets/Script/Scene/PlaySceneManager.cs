//////////////////////////////////////
// プレイシーンのマネージャークラス //
//////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField]
    bool connectFlag = false;
    public GameObject playerPre;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = default(FollowingCamera);


    // ソケット
    private WS.WsPlay wsp = new WS.WsPlay();
    private Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    private SaveData save;

    // コールバック関数をリスト化
    private List<Action<PlayerData, SaveData>> callbackList = new List<Action<PlayerData, SaveData>>();

    private void Awake()
    {
        callbackList.Add(UpdatePlayers);    // 0　プレイヤーの更新
        callbackList.Add(RecvSaveData);     // 1　セーブデータの受け取り
    }

    // Start is called before the first frame update
    void Start()
    {
        // ユーザーID
        var user_id = Retention.ID;
        if (connectFlag)
        {
            // プレイサーバに接続
            //wsp.ConnectionStart(UpdatePlayers, RecvSaveData); // debug
            wsp.ConnectionStart(callbackList);
        }
        Debug.Log("プレイスタート");
    }

    // Update is called once per frame
    void Update()
    {
        MakePlayer();
        if (players.ContainsKey(Retention.ID))
        {
            var playerData = players[Retention.ID].GetComponent<Player>().GetPosition();
            if (Timer())
            {
                if (connectFlag)
                {
                    SendPosition(playerData);
                }
            }
        }
    }

    private void OnDestroy()
    {
        wsp.Destroy();
    }

    private int count = 0;
    private int updateMaxCount = 3;

    private bool Timer() {
        count++;
        if (count > updateMaxCount) {
            count = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 自分以外のユーザーの更新
    /// </summary>
    private void UpdatePlayers(PlayerData _data,SaveData _save)
    {
        Debug.Log("player id :" + Retention.ID.ToString() + "move player id" + _data.id.ToString());

        if (_data.id != 0) {
            if (_data.id != Retention.ID) {
                // ユーザーの更新
                if (players.ContainsKey(_data.id)) {
                    //players[_data.id].GetComponent<OtherPlayers>().UpdataData()
                    players[_data.id].transform.position = new Vector3(_data.x, _data.y, _data.z);
                    Debug.Log("他のユーザーの移動処理");
                }
                // 他のユーザーの作成
                else {
                    var otherPlayer = Instantiate<GameObject>(playerPre);
                    otherPlayer.AddComponent<OtherPlayers>();
                    otherPlayer.GetComponent<OtherPlayers>().Init(_data.x, _data.y, _data.z,_data.dir);
                    players.Add(_data.id, otherPlayer);

                    Debug.Log("他のユーザーの作成");
                };
            }
        }
    }

    /// <summary>
    /// 自分の作成
    /// </summary>
    private void MakePlayer() {
        if (!players.ContainsKey(Retention.ID)) {
            // 自分の作成コンポーネントの追加
            var tmpPlayer = Instantiate<GameObject>(playerPre);
            tmpPlayer.name = "player" + Retention.ID;
            players.Add(Retention.ID, tmpPlayer);
            players[Retention.ID].AddComponent<Player>();
            players[Retention.ID].AddComponent<PlayerController>();
            players[Retention.ID].AddComponent<PlayerSetting>();
            players[Retention.ID].GetComponent<PlayerController>().Init(
                players[Retention.ID].GetComponent<Player>(),
                FollowingCamera,
                players[Retention.ID].GetComponent<PlayerSetting>()
                );
            FollowingCamera.SetTarget(players[Retention.ID]);
        }
    }

    /// <summary>
    /// 自分の作成
    /// </summary>
    private void MakePlayer(SaveData _save)
    {
        if (!players.ContainsKey(Retention.ID))
        {
            // 自分の作成コンポーネントの追加
            var tmpPlayer = Instantiate<GameObject>(playerPre);
            players.Add(Retention.ID, tmpPlayer);
            players[Retention.ID].AddComponent<Player>();
            players[Retention.ID].AddComponent<PlayerController>();
            players[Retention.ID].AddComponent<PlayerSetting>();
            players[Retention.ID].GetComponent<Player>().Init(_save);
            players[Retention.ID].GetComponent<PlayerController>().Init(
                players[Retention.ID].GetComponent<Player>(),
                FollowingCamera,
                players[Retention.ID].GetComponent<PlayerSetting>()
                );
            FollowingCamera.SetTarget(players[Retention.ID]);
        }
    }

    /// <summary>
    /// セーブデータを受け取り入場要請を送信
    /// </summary>
    /// <param name="_save"></param>
    private void RecvSaveData(PlayerData _player, SaveData _save)
    {
        save = _save;
        wsp.SendSaveDataOK();

        // プレイヤーに受け取ったセーブデータを渡す。
        MakePlayer(_save);

    }

    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos)
    {
        wsp.SendPosData(_pos.x, _pos.y, _pos.z, (int)_pos.w);
    }






    /// <summary>
    /// 自分自身の情報を渡す
    /// </summary>
    /// <returns></returns>
    public Player GetPlayerData()
    {
        return players[Retention.ID].GetComponent<Player>();
    } 

    /// <summary>
    /// パーティの情報を渡す
    /// </summary>
    /// <returns></returns>
    public List<OtherPlayers> GetPartyData()
    {
        List<OtherPlayers> party = new List<OtherPlayers>();
        // todo
        // パーティを組んでいる人の情報を渡す
        return party;
    }

    /// <summary>
    /// 自分以外のプレイヤーの情報を渡す
    /// </summary>
    /// <param name="_playerId"></param>
    /// <returns></returns>
    public　OtherPlayers GetOtherPlayer(int _playerId)
    {
        return players[_playerId].GetComponent<OtherPlayers>();
    }

}
