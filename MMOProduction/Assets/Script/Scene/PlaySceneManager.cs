﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    public GameObject playerPre;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera;


    // ソケット
    private Connect.ConnectPlay ws = new Connect.ConnectPlay();
    private Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        // ユーザーID
        var user_id = Retention.ID;
        ws.ConnectionStart(UpdatePlayers);
    }

    // Update is called once per frame
    void Update()
    {
        MakePlayer();
        if (players.ContainsKey(Retention.ID)){
            var playerData = players[Retention.ID].transform.position;
            if (Timer())
            ws.SendPosData(playerData.x, playerData.y, playerData.z, 0);
        }
    }

    private void OnDestroy()
    {
       
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
    private void UpdatePlayers(PlayerData _data)
    {
        Debug.Log("player id :" + Retention.ID.ToString() + "move player id" + _data.id.ToString());

        if (_data.id != 0) {
            if (_data.id != Retention.ID) {
                // ユーザーの更新
                if (players.ContainsKey(_data.id)) {
                    players[_data.id].transform.position = new Vector3(_data.x, _data.y, _data.z);
                    Debug.Log("他のユーザーの移動処理");
                }
                // 他のユーザーの作成
                else {
                    var otherPlayer = Instantiate<GameObject>(playerPre);
                    players.Add(_data.id, otherPlayer);
                    players[_data.id].transform.position = new Vector3(_data.x, _data.y, _data.z);

                    Debug.Log("他のユーザーの作成");
                };
            }
        }
    }

    /// <summary>
    /// プレイヤーの作成
    /// </summary>
    private void MakePlayer() {
        if (!players.ContainsKey(Retention.ID)) {
            // 自分の作成コンポーネントの追加
            var tmpPlayer = Instantiate<GameObject>(playerPre);
            players.Add(Retention.ID, tmpPlayer);
            players[Retention.ID].AddComponent<Player>();
            players[Retention.ID].AddComponent<PlayerControllerV2>();
            players[Retention.ID].AddComponent<PlayerSetting>();
            players[Retention.ID].GetComponent<PlayerControllerV2>().Init(
                players[Retention.ID].GetComponent<Player>(),
                FollowingCamera,
                players[Retention.ID].GetComponent<PlayerSetting>()
                );
            FollowingCamera.SetTarget(players[Retention.ID]);
        }
    }
}
