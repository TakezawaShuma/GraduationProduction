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

    [SerializeField]
    private ChatController chat;

    bool startFlag = false;
    bool updateFlag = true;

    // ソケット
    private WS.WsPlay wsp = null;
    private Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    private Dictionary<int, OtherPlayers> others = new Dictionary<int, OtherPlayers>();
    private Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    private SaveData save;

    // コールバック関数をリスト化
    private List<Action<string>> callbackList = new List<Action<string>>();

    private GameObject player_;

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
            wsp.moveingAction = UpdatePlayers;  // 202
            wsp.enemysAction = RegisterEnemies; // 204
            wsp.statusAction = UpdateStatus;            // 206
            wsp.loadSaveAction = RecvSaveData;  // 210
            wsp.loadFinAction = LoadFinish;     // 212
            wsp.enemyAliveAction = AliveEnemy;  // 221
            wsp.enemyDeadAction = DeadEnemy;    // 222
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
            Packes.GetEnemyDataStoC packet = new Packes.GetEnemyDataStoC();
            packet.enemys.Add(new Packes.EnemyReceiveData(0, 0, 10, 1, 10, 0, 0, 100));
            Debug.Log("敵テスト");
            RegisterEnemies(packet);
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
                        //SendStatus(100, 40, 100, 60, 10001001);
                        LoadFinish(null);
                    }
                }
            }

            // Debug
            if (Input.GetKeyDown(KeyCode.T))
            {
                wsp.Send(new Packes.Attack(0, Retention.ID, 0, 0).ToJson());
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                wsp.Send(new Packes.Attack(1, Retention.ID, 0, 0).ToJson());
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
        //if(Retention.ID == receive.user_id) {
        //    player_ = Instantiate<GameObject>(playerPre);
        //    player_.transform.position = new Vector3(5, 0, 15);
        //    player_.name = "player" + Retention.ID;
        //}

        if (!players.ContainsKey(Retention.ID))
        {
            
            // 自分の作成コンポーネントの追加
            var tmpPlayer = Instantiate<GameObject>(playerPre);
            tmpPlayer.transform.position = new Vector3(5, 0, 15);
            tmpPlayer.name = "player" + Retention.ID;
            players.Add(Retention.ID, tmpPlayer);
            players[Retention.ID].AddComponent<Player>();
            players[Retention.ID].AddComponent<PlayerController>();
            players[Retention.ID].AddComponent<PlayerSetting>();
            players[Retention.ID].GetComponent<PlayerController>().Init(
                players[Retention.ID].GetComponent<Player>(),
                FollowingCamera,
                players[Retention.ID].GetComponent<PlayerSetting>(),
                chat
                );
            FollowingCamera.SetTarget(players[Retention.ID]);
            
        }
    }


    // コールバック系統↓

    /// <summary>
    /// 自分以外のユーザーの更新　→　moveingAction
    /// </summary>
    private void UpdatePlayers(Packes.TranslationStoC _packet)
    {
        Debug.Log("ユーザーの移動系のコールバック");
        Packes.TranslationStoC data = _packet;
        //Debug.Log("ID:" + data.user_id);

        if (data.user_id != 0)
        {
            if (data.user_id != Retention.ID)
            {
                // 他ユーザーの更新
                if (others.ContainsKey(data.user_id))
                {
                    others[data.user_id].UpdataData(0, 0, data.x, data.y, data.z, data.dir);
                    Debug.Log("他のユーザーの移動処理");
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 他のユーザーの作成
                else
                {
                    var otherPlayer = Instantiate<GameObject>(playerPre, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
                    otherPlayer.name = "otherPlayer" + data.user_id;
                    otherPlayer.AddComponent<OtherPlayers>();
                    others.Add(data.user_id, otherPlayer.GetComponent<OtherPlayers>());
                    Debug.Log(otherPlayer.transform.position);

                    Debug.Log("他のユーザーの作成");
                }
            }
        }
    }


    /// <summary>
    /// エネミーの情報の更新と作成　→ enemysAction
    /// </summary>
    /// <param name="_str"></param>
    private void RegisterEnemies(Packes.GetEnemyDataStoC _packet)
    {
        Debug.Log("エネミーの作成");

        // todo
        // エネミーの作成と更新

        List<Packes.EnemyReceiveData> list = _packet.enemys;
        foreach(var ene in list)
        {
            if(ene.unique_id!=Retention.ID)
            {
                // 敵の更新
                if(enemies.ContainsKey(ene.unique_id))
                {
                    enemies[ene.unique_id].UpdataData(ene.hp, 0, ene.x, ene.y, ene.z, ene.dir);
                }
                // 敵の作成
                else
                {
                    var newEnemy = Instantiate<GameObject>(testEnemyPre, new Vector3(ene.x, ene.y, ene.z), Quaternion.Euler(0, ene.dir, 0));
                    newEnemy.name = "Enemy:" + ene.master_id;
                    Enemy enemy = newEnemy.AddComponent<Enemy>();
                    enemies.Add(ene.unique_id, enemy);
                    Debug.Log("敵の新規作成");
                }
            }
        }
        //Packes.TranslationStoC data = new Packes.TranslationStoC();
        //Debug.Log("ID:" + data.user_id);
        //if (data.user_id != 0)
        //{
        //    if (data.user_id != Retention.ID)
        //    {
        //        // 敵の更新
        //        if (players.ContainsKey(data.user_id))
        //        {
        //            players[data.user_id].GetComponent<Enemy>().UpdataData(0, 0, data.x, data.y, data.z, data.dir);
        //            Debug.Log("敵の移動処理");
        //        }
        //        // todo 他プレイヤーの更新と作成を関数分けする
        //        // 敵の作成
        //        else
        //        {
        //            var otherPlayer = Instantiate<GameObject>(testEnemyPre, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
        //            otherPlayer.name = "enemy" + data.user_id;
        //            otherPlayer.AddComponent<Enemy>();
        //            players.Add(data.user_id, otherPlayer);
        //            Debug.Log(otherPlayer.transform.position);

        //            Debug.Log("敵の作成");
        //        }
        //    }
        //}
    }

    /// <summary>
    /// ステータスの更新　→ statusAction
    /// </summary>
    private void UpdateStatus(Packes.StatusStoC _packet)
    {
        // todo
    }

    /// <summary>
    /// セーブデータを受け取り入場要請を送信　→　loadSaveAction
    /// </summary>
    /// <param name="_save"></param>
    private void RecvSaveData(Packes.LoadSaveData _packet)
    {
        Debug.Log("セーブデータの取得");
        //SaveData data = JsonUtility.FromJson<SaveData>(_str);

        //save = data;

        wsp.Send(new Packes.LoadingFinishCtoS().ToJson());
        //wsp.SendSaveDataOK();

        // プレイヤーに受け取ったセーブデータを渡す。
        //MakePlayer(data);

    }

    /// <summary>
    /// セーブデータの読み込みが完了したら呼ばれる　→ loadFinAction
    /// </summary>
    /// <param name="_packet"></param>
    private void LoadFinish(Packes.LoadingFinishStoC _packet)
    {
        updateFlag = true;
        Debug.Log("LoadFinish");
        wsp.Send(new Packes.GetEnemysDataCtoS(0, Retention.ID).ToJson());
        // プレイヤーのインスタンスを取る

    }


    private void AliveEnemy(Packes.EnemyAliveStoC _packet)
    {
        // todo
        // 戦闘で計算後エネミーが生きていたら
        // HPを減らすや状態の更新

        Debug.Log("敵は生存している");
    }

    private void DeadEnemy(Packes.EnemyDieStoC _packet)
    {
        // todo
        // 戦闘で計算後エネミーが死亡していたら
        // HPを0にして死亡エフェクトやドロップアイテムの取得

        Debug.Log("敵は死んだ！！！");
    }


    // --------------------送信関係--------------------

    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos)
    {
        wsp.Send(new Packes.TranslationCtoS(Retention.ID, _pos.x, _pos.y, _pos.z, _pos.w).ToJson());
    }
    
    private void SendStatus(int _maxHp,int _hp,int _maxMp,int _mp ,int _status)
    {
        wsp.Send(new Packes.StatusCtoS(Retention.ID, _maxHp, _hp, _maxMp, _mp, _status).ToJson());
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
        return others[_playerId].GetComponent<OtherPlayers>();
    }

}
