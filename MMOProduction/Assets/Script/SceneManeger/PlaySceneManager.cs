//////////////////////////////////////
// プレイシーンのマネージャークラス //
//////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlaySceneManager : SceneManagerBase
{
    public GameObject playerPre = null;

    [SerializeField,Header("キャラクターモデルリスト")]
    private character_table characterModel = null;
    [Header("敵のマスターデータ"), SerializeField]
    private enemy_table enemyTable;
    [Header("スキルの全データ"), SerializeField]
    private skill_table skillTabe;


    [SerializeField,Header("プレイヤーの名前表示UI")]
    private GameObject nameUI = null;
    [SerializeField, Header("エネミーのステータスUI")]
    private GameObject enemyStatusCanvas = null;

 
    [SerializeField, Header("テストの敵")]
    private GameObject testEnemyPre = null;
    [SerializeField]
    private GameObject otherPlayerPre_ = null;



    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = default(FollowingCamera);

    [SerializeField]
    private ChatController chat = default(ChatController);
    
 
    [SerializeField]
    private PlayerUI playerUI = null;

    [SerializeField]
    private GameUserSetting userSeeting = null;

    bool updateFlag = false;

    // 通信やその他で不具合が生じた場合の再試行用カウンター
    int countOfTrials = 0;

    // ソケット
    private WS.WsPlay wsp = null;

    // プレイヤー情報
    private Player player = null;
    /// <summary> 自分自身の情報を渡す </summary>
    public Player Player { get { return player; } }

    // プレイヤー以外のキャラクター情報
    private Dictionary<int, NonPlayer> charcters = new Dictionary<int, NonPlayer>();

    // コールバック関数をリスト化
    private List<Action<string>> callbackList = new List<Action<string>>();
    

    private Ready ready;


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

            wsp.loadSaveAction = ReceiveSaveData;               // 212
            wsp.loadOtherListAction = ReceiveOtherListData;     // 214
            wsp.loadOtherAction = ReceiveOtherData;             // 215

            wsp.enemyAliveAction = AliveEnemy;                  // 221
            wsp.enemyDeadAction = DeadEnemy;                    // 222
            wsp.enemySkillReqAction = EnemyUseSkillRequest;     // 225
            wsp.enemyUseSkillAction = EnemyUseSkill;            // 226
            wsp.enemyAttackAction = EnemyAttackResult;          // 227

            wsp.logoutAction = Logout;                          // 707
            wsp.findResultsAction = ReceivingFindResults;       // 712

            // セーブデータを要請する。
            wsp.Send(new Packes.SaveLoadCtoS(UserRecord.ID).ToJson());
            Debug.Log("自分のID->" + UserRecord.ID);
        }
        if (!connectFlag) { MakePlayer(new Vector3(-210, 5, -210), playerPre); }
    }


    /// <summary>
    /// フレーム数を数える
    /// </summary>
    /// <returns></returns>
    private bool Timer()
    {
        count++;
        if (count > UPDATE_MAX_COUNT)
        {
            count = 0;
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        InputManager.Update();
        if (InputManager.InputKeyCheck(KeyCode.Escape)) Quit();
        if (updateFlag)
        {
            ready.ReadyGO();
            if (player != null)
            {
                var playerData = player.PositionV4;
                if (Timer())
                {
                    if (connectFlag)
                    {
                        SendPosition(playerData);
                        //SendStatus(UserRecord.ID, Packes.OBJECT_TYPE.PLAYER);
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
    /// エディター上でプレイを停止する
    /// </summary>
    private void OnApplicationQuit()
    {
        if (connectFlag) { wsp.Destroy(); }
    }

    private int count = 0;
    private const int UPDATE_MAX_COUNT = 3;


    public GameObject miniMapCameraPrefab_;

    /// <summary>
    /// 自分の作成
    /// </summary>
    private bool MakePlayer(Vector3 _save,GameObject _playerModel, string _name= "player0")
    {
        bool ret = false;
        _name = (_name == "player0") ? _name = "player" + UserRecord.ID : _name;
        
        // プレイヤーが作られた事がないなら
        if (player == null)
        {
            var tmp = Instantiate<GameObject>(_playerModel, this.transform);
            tmp.transform.position = new Vector3(_save.x, _save.y, _save.z);
            tmp.name = (UserRecord.Name != "") ? UserRecord.Name : _name;
            tmp.tag = "Player";
            tmp.transform.localScale = new Vector3(2, 2, 2);

            Player playerComponent = tmp.AddComponent<Player>();
            PlayerController playerCComponent = tmp.AddComponent<PlayerController>();
            PlayerSetting playerSetComponent = tmp.AddComponent<PlayerSetting>();

            playerCComponent.Init(playerComponent, FollowingCamera, playerSetComponent, chat, tmp.GetComponent<Animator>());
            userSeeting.Init(tmp);
            player = playerComponent;
            FollowingCamera.Target = tmp;
            //userPlayer = tmp;

            // ミニマップのカメラの作成
            var miniMapTmp = Instantiate<GameObject>(miniMapCameraPrefab_, this.transform);
            miniMapTmp.GetComponent<MiniMapController>().Init(tmp);

            playerUI.PLAYER_CMP = playerComponent;
            playerUI.PLAYER_NAME = UserRecord.Name;

            ret = true;
        }
        return ret;
    }


    // コールバック系統↓

    /// <summary>
    /// 自分以外のユーザーの位置更新　→　moveingAction
    /// </summary>
    private void UpdatePlayersPostion(Packes.TranslationStoC _packet)
    {
        if (_packet.user_id != 0)
        {
            if (_packet.user_id != UserRecord.ID)
            {
                // 他ユーザーの更新
                if (charcters.ContainsKey(_packet.user_id))
                {
                    if (charcters[_packet.user_id] != null)
                    {
                        charcters[_packet.user_id].UpdatePostionData(_packet.x, _packet.y, _packet.z, _packet.dir);
                    }
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 他のユーザーの作成
                else
                {
                    // リストに登録されていないIDが来たときの処理
                    // そのIDは何なのか確認をとる
                    Debug.Log("この人誰？->" + _packet.user_id);
                    wsp.Send(new Packes.FindOfPlayerCtoS(UserRecord.ID,_packet.user_id, 0).ToJson());
                }
            }
        }
    }

    /// <summary>
    /// 他プレイヤーの作成
    /// </summary>
    /// <param name="_packet">作成に必要なデータ</param>
    private void CreateOtherPlayers(Packes.OtherPlayersData _packet)
    {
        GameObject avatar = characterModel.FindModel(CheckModel(_packet.model_id));


        var otherPlayer = Instantiate<GameObject>
                          (avatar,
                          Vector3.zero,
                          Quaternion.Euler(0, 0, 0),
                          this.transform);                                  // 本体生成

        GameObject name = Instantiate(nameUI, otherPlayer.transform);       // プレイヤーアイコン生成
        var other = otherPlayer.AddComponent<OtherPlayers>();               // OtherPlayerの追加
        name.GetComponent<OtherUserNameUI>().UserName = otherPlayer.name
                                                      = other.Name
                                                      = _packet.name;       // 名前の共通化

        otherPlayer.tag = "OtherPlayer";                                    // タグ
        otherPlayer.transform.localScale = new Vector3(2, 2, 2);
        other.Init(0, 0, 0, 0, _packet.user_id, skillTabe);
        charcters[_packet.user_id] = other;                                 // キャラクター管理に登録
        Debug.Log("他キャラ生成" + _packet.user_id);
    }

    private int CheckModel(int _id) {
        return (_id == 0) ? 101 : _id;
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
                if (charcters.ContainsKey(ene.unique_id))
                {
                    if (charcters[ene.unique_id] != null)
                    {
                        charcters[ene.unique_id].UpdatePostionData(ene.x, ene.y, ene.z, ene.dir);
                    }
                }
                // 敵の作成
                else
                {
                    // 登録外のIDが来たら新規作成する
                    CreateEnemys(ene);
                }
            }
        }
    }

    /// <summary>
    /// エネミー作成
    /// </summary>
    /// <param name="_ene">作成に必要なデータ</param>
    private void CreateEnemys(Packes.EnemyReceiveData _ene)
    {
        GameObject enemyModel = enemyTable.FindModel(_ene.master_id);

        GameObject newEnemy = Instantiate<GameObject>
                              (testEnemyPre,
                              Vector3.zero,
                              Quaternion.Euler(0, 0, 0));
        GameObject stutasCanvas = Instantiate(enemyStatusCanvas, newEnemy.transform);

        Enemy enemy = newEnemy.AddComponent<Enemy>();

        stutasCanvas.GetComponent<UIEnemyHP>().MAX_HP = enemy.HP = _ene.hp;
        stutasCanvas.GetComponent<UIEnemyHP>().Off();

        newEnemy.name = "Enemy:" + _ene.master_id + "->" + _ene.unique_id;
        enemy.Init(_ene.x, _ene.y, _ene.z, _ene.dir, _ene.unique_id,skillTabe);
        enemy.UI_HP = stutasCanvas.GetComponent<UIEnemyHP>();
        charcters[_ene.unique_id] = enemy;
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
                if(charcters.ContainsKey(tmp.charctor_id))
                charcters[tmp.charctor_id].UpdateStatusData(tmp.hp, tmp.mp, tmp.status);
            }
        }


    }


    /// <summary>
    /// セーブデータを受け取る→　loadSaveAction
    /// </summary>
    /// <param name="_save"></param>
    private void ReceiveSaveData(Packes.SaveLoadStoC _packet)
    {
        //GameObject model = playerPre;
        GameObject model = characterModel.FindModel(CheckModel(_packet.model_id));

        if (MakePlayer(new Vector3(_packet.x, _packet.y, _packet.z), model))
        {
            wsp.Send(new Packes.LoadingOK(UserRecord.ID).ToJson());
            Debug.Log("セーブデータを受けとり自分を作成した。→ロード完了");
            updateFlag = true;
        }
        else
        {
            if (countOfTrials < 3)
            {
                // セーブデータを再度要請する
                StartCoroutine(ReSend(wsp, new Packes.SaveLoadCtoS(UserRecord.ID).ToJson()));
                countOfTrials++;
            }
            else { countOfTrials = 0; }

        }
    }

    /// <summary>
    /// 他プレイヤーの一覧取得 → loadOtherListAction
    /// </summary>
    /// <param name="_packet"></param>
    private void ReceiveOtherListData(Packes.OtherPlayerList _packet)
    {
        Debug.Log("他プレイヤーの一覧取得");
        // ほかプレイヤー一覧の生成
        foreach (var tmp in _packet.players)
        {
            CreateOtherPlayers(tmp);
        }
    }

    /// <summary>
    /// 新規入室者の登録 → loadOtherAction
    /// </summary>
    /// <param name="_packet"></param>
    private void ReceiveOtherData(Packes.NewOtherUser _packet)
    {

        Debug.Log("新規さん入室");
        Packes.OtherPlayersData tmp = new Packes.OtherPlayersData
                                            (_packet.user_id,
                                            _packet.x,
                                            _packet.y,
                                            _packet.z,
                                            _packet.model_id,
                                            _packet.name);
        // 新規入室プレイヤーの作成
        CreateOtherPlayers(tmp);

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
        Enemy enemy = charcters[_packet.unique_id].GetComponent<Enemy>();
        enemy.PlayTriggerAnimetion("Take Damage");
        enemy.HP = _packet.hp;
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
        Enemy enemy = charcters[_packet.unique_id].GetComponent<Enemy>();
        enemy.HP = 0;
        enemy.PlayTriggerAnimetion("Die");

        PlayerController pc = player.GetComponent<PlayerController>();
        int target = pc.Target.GetComponentInParent<Enemy>().ID;

        Debug.Log("TargetID : " + target + " , DieEnemyID : " + _packet.unique_id);
        if (target == _packet.unique_id)
        {
            pc.RemoveTarget();
        }
        charcters.Remove(_packet.unique_id);
    }


    /// <summary>
    /// 他プレイヤーのスキルを再生 → ???
    /// </summary>
    /// <param name="_paket"></param>
    private void OthersUseSkills(Packes.OtherPlayerUseSkill _paket)
    {
        // todo
        // 他プレイヤーがスキルを使ったときの処理
    }

    /// <summary>
    /// エネミーのスキル使用リクエストを検知 → enemySkillReqAction
    /// </summary>
    /// <param name="_packet"></param>
    private void EnemyUseSkillRequest(Packes.EnemyUseSkillRequest _packet)
    {
        Debug.Log("敵のスキル発動を検知したよ");
        // todo
        // 敵がスキルを使ったときの準備モーション等の処理
    }

    /// <summary>
    /// エネミーのスキルの発動 → enemyUseSkillAction
    /// </summary>
    /// <param name="_packet"></param>
    private void EnemyUseSkill(Packes.EnemyUseSkill _packet)
    {
        Debug.Log("敵のスキルが発動したよ");
        Enemy enemy = charcters[_packet.enemy_id].GetComponent<Enemy>();
        // enemy.PlayTriggerAnimetion("Attack");
        enemy.PlayAttackAnimation(_packet.skill_id);
        if (_packet.target_id == UserRecord.ID)
        {
            enemy.Attacked(player.gameObject,_packet.skill_id);
        }
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
    /// プレイヤーがログアウトした時 → logoutAction
    /// </summary>
    /// <param name="_packet"></param>
    private void Logout(Packes.LogoutStoC _packet)
    {
        if (_packet.user_id == UserRecord.ID)
        {
            Debug.Log("ログアウトしたよ");
            if (connectFlag) { wsp.Destroy(); }
            ChangeScene("LoginScene");
        }
        else
        {
            Destroy(charcters[_packet.user_id].gameObject);
            charcters.Remove(_packet.user_id);
            Debug.Log(_packet.user_id + "さんがログアウトしたよ！");
        }
    }

    /// <summary>
    /// 検索結果のユーザーを作成 → findResultsAction
    /// </summary>
    /// <param name="_packet"></param>
    private void ReceivingFindResults(Packes.FindOfPlayerStoC _packet)
    {
        Debug.Log("検索の結果他プレイヤーを作成 -> " + _packet.user_id);
        Packes.OtherPlayersData tmp = new Packes.OtherPlayersData(_packet.user_id, _packet.x, _packet.y, _packet.z, _packet.model_id, _packet.name);
        CreateOtherPlayers(tmp);
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
        return charcters[_playerId].GetComponent<OtherPlayers>();
    }
}
