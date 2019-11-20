//////////////////////////////////////
// プレイシーンのマネージャークラス //
//////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField]
    bool connectFlag = false;
    public GameObject playerPre = null;

    [SerializeField]
    private GameObject otherPlayerPre_;

    [SerializeField, Header("テストの敵")]
    private GameObject testEnemyPre = null;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = default(FollowingCamera);

    [SerializeField]
    private ChatController chat = default(ChatController);

    bool updateFlag = true;

    // ソケット
    private WS.WsPlay wsp = null;
    private Player player = null;
    private Dictionary<int, OtherPlayers> others = new Dictionary<int, OtherPlayers>();
    private Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    private Dictionary<int, CharacterBase> charcters = new Dictionary<int, CharacterBase>();

    // コールバック関数をリスト化
    private List<Action<string>> callbackList = new List<Action<string>>();
    

    private GameObject newEnemy = null;

    private Ready ready;
    [SerializeField]
    private Vector3 playerSpawnPos = new Vector3(0, 0, 0);

    private GameObject userPlayer;

    public GameObject USER
    {
        get { return userPlayer; }
    }

    private void Awake()
    {
        ready = Ready.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ユーザーID
        var user_id = UserRecord.ID;
        if (connectFlag)
        {
            // プレイサーバに接続
            wsp = WS.WsPlay.Instance;
            wsp.moveingAction = UpdatePlayersPostion;           // 202
            wsp.enemysAction = RegisterEnemies;                 // 204
            wsp.statusAction = UpdateStatus;                    // 206
            wsp.loadSaveAction = RecvSaveData;                  // 210
            wsp.loadFinAction = LoadFinish;                     // 212
            wsp.enemyAliveAction = AliveEnemy;                  // 221
            wsp.enemyDeadAction = DeadEnemy;                    // 222
            wsp.enemySkillReqAction = EnemyUseSkillRequest;     // 225
            wsp.enemyUseSkillAction = EnemyUseSkill;            // 226
            wsp.enemyAttackAction = EnemyAttackResult;          // 227
            wsp.logoutAction = Logout;                          // 707
            // セーブデータを要請する。
            wsp.Send(new Packes.DataLoading(UserRecord.ID).ToJson());

        }
        MakePlayer(new Vector3(-210, 5, -210));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        ready.ReadyGO();
        if (updateFlag)
        {
            if (player != null)
            {
                var playerData = player.GetPosition();
                if (Timer())
                {
                    if (connectFlag)
                    {
                        SendPosition(playerData);
                        SendStatus(UserRecord.ID, Packes.OBJECT_TYPE.PLAYER);
                        SendEnemyPosReq();
                    }
                }
            }
        }
        // debug
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    DeadEnemy(new Packes.EnemyDieStoC(0, 100)); // 100番を殺す
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Packes.GetEnemyDataStoC v = new Packes.GetEnemyDataStoC();
        //    v.enemys.Add(new Packes.EnemyReceiveData(100, 0, -210, 1, -210, 0, 0, 10));
        //    RegisterEnemies(v); // 100番を生み出す
        //}
        //if (Input.GetKeyDown(KeyCode.Comma))
        //{
        //    enemies[100].PlayTriggerAnimetion("Attack"); // 敵の攻撃モーションの再生
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))   // 敵へ攻撃
        //{
        //    wsp.Send(Json.ConvertToJson(new Packes.Attack(0, UserRecord.ID, 0, 0)));
        //    Debug.Log("プレイヤーの攻撃");
        //}
        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    AliveEnemy(new Packes.EnemyAliveStoC(100, 10, 0));
        //}
    }

    /// <summary>
    /// .exeの終了関数
    /// </summary>
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }


    private void OnApplicationQuit()
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


    public GameObject miniMapCameraPrefab_;

    /// <summary>
    /// 自分の作成
    /// </summary>
    private void MakePlayer(Vector3 _save,string _name= "player0")
    {
        _name = "player" + UserRecord.ID;
        if (player == null)
        {
            var tmp = Instantiate<GameObject>(playerPre, this.transform);
            tmp.transform.position = new Vector3(_save.x, _save.y, _save.z);
            tmp.name = (UserRecord.Name != "") ? UserRecord.Name : _name;
            tmp.tag = "Player";
            tmp.transform.localScale = new Vector3(2, 2, 2);
            tmp.AddComponent<Player>();
            tmp.AddComponent<PlayerController>();
            tmp.AddComponent<PlayerSetting>();
            tmp.GetComponent<PlayerController>().Init(tmp.GetComponent<Player>(), FollowingCamera, tmp.GetComponent<PlayerSetting>(), chat, tmp.GetComponent<Animator>());
            player = tmp.GetComponent<Player>();
            tmp.GetComponent<NameUI>().TEXT.enabled = false;
            FollowingCamera.SetTarget(tmp);
            userPlayer = tmp;

            // ミニマップのカメラの作成
            var miniMapTmp = Instantiate<GameObject>(miniMapCameraPrefab_);
            miniMapTmp.GetComponent<MiniMapController>().Init(tmp);
        }
    }


    // コールバック系統↓

    /// <summary>
    /// 自分以外のユーザーの更新　→　moveingAction
    /// </summary>
    private void UpdatePlayersPostion(Packes.TranslationStoC _packet)
    {
        Packes.TranslationStoC data = _packet;

        if (data.user_id != 0)
        {
            if (data.user_id != UserRecord.ID)
            {
                // 他ユーザーの更新
                if (others.ContainsKey(data.user_id))
                {
                    if (others[data.user_id] != null)
                    {
                        others[data.user_id].UpdatePostionData(data.x, data.y, data.z, data.dir);
                    }
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 他のユーザーの作成
                else
                {
                    var otherPlayer = Instantiate<GameObject>(otherPlayerPre_, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
                    otherPlayer.name = "otherPlayer" + data.user_id;
                    otherPlayer.tag = "OtherPlayer";
                    otherPlayer.transform.localScale = new Vector3(2, 2, 2);
                    var other = otherPlayer.AddComponent<OtherPlayers>();
                    other.Init(data.x, data.y, data.z, data.dir);
                    otherPlayer.GetComponent<NameUI>().NameSet(data.user_id.ToString());
                    others.Add(data.user_id, other);
                    charcters.Add(data.user_id, other);
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
        List<Packes.EnemyReceiveData> list = _packet.enemys;
        
        foreach (var ene in list)
        {
            if(ene.unique_id!=UserRecord.ID)
            {
                // 敵の更新
                if (enemies.ContainsKey(ene.unique_id))
                {
                    if (enemies[ene.unique_id] != null)
                    {
                        enemies[ene.unique_id].UpdatePostionData(ene.x, ene.y, ene.z, ene.dir);
                    }
                }
                // 敵の作成
                else
                {
                    newEnemy = Instantiate<GameObject>(testEnemyPre, new Vector3(ene.x, ene.y, ene.z), Quaternion.Euler(0, ene.dir, 0));
                    newEnemy.name = "Enemy:" + ene.master_id + "->" + ene.unique_id;
                    //newEnemy.GetComponent<Rigidbody>().useGravity = true;
                    Enemy enemy = newEnemy.AddComponent<Enemy>();
                    enemy.Init(ene.x, ene.y, ene.z, ene.dir);
                    enemies[ene.unique_id] = enemy;
                    enemies[ene.unique_id].ID = ene.unique_id;
                    charcters[ene.unique_id] = enemy;
                    charcters[ene.unique_id].ID = ene.unique_id;
                    Debug.Log("エネミーの新規せいせ");
                }
            }
        }
    }

    /// <summary>
    /// ステータスの更新　→ statusAction
    /// </summary>
    private void UpdateStatus(Packes.StatusStoC _packet)
    {
        foreach(var tmp in _packet.status)
        {
            if (tmp.charctor_id == UserRecord.ID)
            {
                player.UpdateStatus( tmp.hp, tmp.mp, tmp.status);
            }
            else
            {
                charcters[tmp.charctor_id].UpdateStatusData(tmp.hp, tmp.mp, tmp.status);
            }
        }


    }

    /// <summary>
    /// セーブデータを受け取る→　loadSaveAction
    /// </summary>
    /// <param name="_save"></param>
    private void RecvSaveData(Packes.LoadSaveData _packet)
    {

        wsp.Send(new Packes.LoadingFinishCtoS().ToJson());
        //wsp.SendSaveDataOK();

        // プレイヤーに受け取ったセーブデータを渡す。
        MakePlayer(new Vector3(5, 1, 15));

    }

    /// <summary>
    /// セーブデータの読み込みが完了したら呼ばれる　→ loadFinAction
    /// </summary>
    /// <param name="_packet"></param>
    private void LoadFinish(Packes.LoadingFinishStoC _packet)
    {
        updateFlag = true;
        wsp.Send(new Packes.GetEnemysDataCtoS(0, UserRecord.ID).ToJson());
        // プレイヤーのインスタンスを取る

    }

    /// <summary>
    /// 戦闘処理生存 → enemyAliveAction
    /// </summary>
    /// <param name="_packet"></param>
    private void AliveEnemy(Packes.EnemyAliveStoC _packet)
    {
        // todo
        // 戦闘で計算後エネミーが生きていたら
        // HPを減らすや状態の更新

        Debug.Log("敵は生存している");
        enemies[_packet.unique_id].PlayTriggerAnimetion("Take Damage");
    }

    /// <summary>
    /// 戦闘処理死亡 → enemyDeadAction
    /// </summary>
    /// <param name="_packet"></param>
    private void DeadEnemy(Packes.EnemyDieStoC _packet)
    {
        // todo
        // 戦闘で計算後エネミーが死亡していたら
        // HPを0にして死亡エフェクトやドロップアイテムの取得
        enemies[_packet.unique_id].PlayTriggerAnimetion("Die");
        player.GetComponent<PlayerController>().Lock = false;
        enemies.Remove(_packet.unique_id);
        //charcters.Remove(_packet.unique_id);
        Debug.Log(charcters.Count);
        Debug.Log("敵は死んだ！！！");
    }

    /// <summary>
    /// エネミーのスキル使用リクエストを検知 → enemySkillReqAction
    /// </summary>
    /// <param name="_packet"></param>
    private void EnemyUseSkillRequest(Packes.EnemyUseSkillRequest _packet)
    {
        Debug.Log("敵のスキル発動を検知したよ");
    }

    /// <summary>
    /// エネミーのスキルの発動 → enemyUseSkillAction
    /// </summary>
    /// <param name="_packet"></param>
    private void EnemyUseSkill(Packes.EnemyUseSkill _packet)
    {
        Debug.Log("敵のスキルが発動したよ");
        enemies[_packet.enemy_id].PlayTriggerAnimetion("Attack");
    }

    /// <summary>
    /// エネミーの攻撃結果 → enemyAttackAction
    /// </summary>
    /// <param name="_packet"></param>
    private void EnemyAttackResult(Packes.EnemyAttackResult _packet)
    {
        Debug.Log("敵の攻撃が" + _packet.user_id + "にヒット");
    }

    /// <summary>
    /// 他プレイヤーがログアウトした時 → logoutAction
    /// </summary>
    /// <param name="_packet"></param>
    private void Logout(Packes.LogoutStoC _packet)
    {
        Destroy(others[_packet.user_id].gameObject);
        charcters.Remove(_packet.user_id);
        others.Remove(_packet.user_id);
        Debug.Log(_packet.user_id + "さんがログアウトしたよ！");
    }


    // --------------------送信関係--------------------

    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos)
    {
        wsp.Send(new Packes.TranslationCtoS(UserRecord.ID, _pos.x, _pos.y, _pos.z, _pos.w).ToJson());
    }

    /// <summary>
    /// ステータスの要求
    /// </summary>
    /// <param name="_target_id">欲しい相手のID</param>
    /// <param name="_type">相手のタイプ</param>
    private void SendStatus(int _target_id, Packes.OBJECT_TYPE _type)
    {
        wsp.Send(new Packes.StatusCtoS(UserRecord.ID, _target_id, (int)_type).ToJson());
    }

    /// <summary>
    /// エネミーの情報の要求
    /// </summary>
    private void SendEnemyPosReq()
    {
        wsp.Send(new Packes.GetEnemysDataCtoS(0, UserRecord.ID).ToJson());
    }

    /// <summary>
    /// 自分自身の情報を渡す
    /// </summary>
    /// <returns></returns>
    public Player GetPlayerData()
    {
        return player;
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
        return others[_playerId];
    }

}
