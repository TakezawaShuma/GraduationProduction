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

    [SerializeField, Header("テストの敵")]
    private GameObject testEnemyPre;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = default(FollowingCamera);

    bool startFlag = false;
    bool updateFlag = true;

    // ソケット
    private WS.WsPlay wsp = null;
    private Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
    private SaveData save;

    // コールバック関数をリスト化
    private List<Action<string>> callbackList = new List<Action<string>>();

    private void Awake()
    {

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
            wsp = new WS.WsPlay(8001);
            wsp.loadSaveAction = RecvSaveData;
            wsp.moveingAction = UpdatePlayers;
            wsp.statusAction = null;
            wsp.Send(new Packes.DataLoading(Retention.ID).ToJson());

        }
        Debug.Log("プレイスタート");
        MakePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Packes.TranslationStoC packet = new Packes.TranslationStoC();
            packet.user_id = 1;
            packet.x = 0;
            packet.y = 0.5f;
            packet.z = 15;
            packet.dir = 0;
            Debug.Log("テスト");
            UpdatePlayers(packet);
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            Packes.TranslationStoC packet = new Packes.TranslationStoC();
            packet.user_id = 1;
            packet.x = 10;
            packet.y = 0.5f;
            packet.z = 20;
            packet.dir = 180;
            Debug.Log("テスト");
            UpdatePlayers(packet);
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            Packes.TranslationStoC packet = new Packes.TranslationStoC();
            packet.user_id = 100;
            packet.x = 0;
            packet.y = 0.2f;
            packet.z = 30;
            packet.dir = 0;
            Debug.Log("敵テスト");
            RegisterEnemies(packet);
        }
        else if(Input.GetKeyDown(KeyCode.V))
        {
            Packes.TranslationStoC packet = new Packes.TranslationStoC();
            packet.user_id = 100;
            packet.x = 10;
            packet.y = 0.2f;
            packet.z = 10;
            packet.dir = 180;
            Debug.Log("敵テスト");
            RegisterEnemies(packet);
        }

        if(startFlag)
        {
            wsp.Send(new Packes.InitLogin(Retention.ID).ToJson());
            startFlag = false;
            updateFlag = true;
        }
        if (updateFlag)
        {
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
    }

    private void OnDestroy()
    {
        if (connectFlag) { wsp.Destroy(); }
    }

    private int count = 0;
    private int updateMaxCount = 3;

    private bool Timer()
    {
        count++;
        if (count > updateMaxCount)
        {
            count = 0;
            return true;
        }
        return false;
    }




    /// <summary>
    /// 自分の作成
    /// </summary>
    private void MakePlayer()
    {
        if (!players.ContainsKey(Retention.ID))
        {
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
            //players[Retention.ID].GetComponent<Player>().Init(_save);
            players[Retention.ID].GetComponent<PlayerController>().Init(
                players[Retention.ID].GetComponent<Player>(),
                FollowingCamera,
                players[Retention.ID].GetComponent<PlayerSetting>()
                );
            FollowingCamera.SetTarget(players[Retention.ID]);
        }
    }



    // コールバック系統↓

    /// <summary>
    /// 自分以外のユーザーの更新
    /// </summary>
    private void UpdatePlayers(Packes.TranslationStoC _packet)
    {
        Debug.Log("ユーザーの移動系のコールバック");
        Packes.TranslationStoC data = _packet;
        Debug.Log("ID:" + data.user_id);

        if (data.user_id != 0)
        {
            if (data.user_id != Retention.ID)
            {
                // 他ユーザーの更新
                if (players.ContainsKey(data.user_id))
                {
                    players[data.user_id].GetComponent<OtherPlayers>().UpdataData(0, 0, data.x, data.y, data.z, data.dir);
                    Debug.Log("他のユーザーの移動処理");
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 他のユーザーの作成
                else
                {
                    var otherPlayer = Instantiate<GameObject>(playerPre, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
                    otherPlayer.name = "otherPlayer" + data.user_id;
                    otherPlayer.AddComponent<OtherPlayers>();
                    players.Add(data.user_id, otherPlayer);
                    Debug.Log(otherPlayer.transform.position);

                    Debug.Log("他のユーザーの作成");
                }
            }
        }
    }


    /// <summary>
    /// エネミーの情報の更新と作成
    /// </summary>
    /// <param name="_str"></param>
    private void RegisterEnemies(Packes.TranslationStoC _packet)
    {
        // todo
        // エネミーの作成と更新

        Packes.TranslationStoC data = _packet;
        Debug.Log("ID:" + data.user_id);

        if (data.user_id != 0)
        {
            if (data.user_id != Retention.ID)
            {
                // 敵の更新
                if (players.ContainsKey(data.user_id))
                {
                    players[data.user_id].GetComponent<Enemy>().UpdataData(0, 0, data.x, data.y, data.z, data.dir);
                    Debug.Log("敵の移動処理");
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 敵の作成
                else
                {
                    var otherPlayer = Instantiate<GameObject>(testEnemyPre, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
                    otherPlayer.name = "enemy" + data.user_id;
                    otherPlayer.AddComponent<Enemy>();
                    players.Add(data.user_id, otherPlayer);
                    Debug.Log(otherPlayer.transform.position);

                    Debug.Log("敵の作成");
                }
            }
        }
    }




    /// <summary>
    /// セーブデータを受け取り入場要請を送信
    /// </summary>
    /// <param name="_save"></param>
    private void RecvSaveData(Packes.LoadSaveData _packet)
    {
        Debug.Log("セーブデータの取得");
        //SaveData data = JsonUtility.FromJson<SaveData>(_str);

        //save = data;

        wsp.Send(new Packes.LoadingFinish().ToJson());
        startFlag = true;
        //wsp.SendSaveDataOK();

        // プレイヤーに受け取ったセーブデータを渡す。
        //MakePlayer(data);

    }
    


    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos)
    {
        wsp.Send(new Packes.TranslationCtoS(Retention.ID, _pos.x, _pos.y, _pos.z, 0).ToJson());
        //wsp.SendPosData(_pos.x, _pos.y, _pos.z, (int)_pos.w);
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
