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

    private const int UPDATE_MAX_COUNT = 3;


    [SerializeField, Header("キャラクターモデルリスト")]
    private character_table characterModel = null;
    [Header("敵のマスターデータ"), SerializeField]
    private enemy_table enemyTable = null;
    [Header("スキルの全データ"), SerializeField]
    private skill_table skillTabe = null;


    [SerializeField, Header("プレイヤーの名前表示UI")]
    private GameObject nameUI = null;
    [SerializeField, Header("エネミーのステータスUI")]
    private GameObject enemyStatusCanvas = null;


    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = default(FollowingCamera);


    [SerializeField]
    private PlayerUI playerUI = null;

    [SerializeField]
    private GameUserSetting userSeeting = null;

    [SerializeField]
    private PlayerSetting playerSetting = null;

    [SerializeField]
    private CheatCommand cheatCommand = null;

    [SerializeField]
    private GameObject miniMapCameraPrefab_;

    [SerializeField]
    private UIManager uiManager = null;

    [SerializeField]
    private QuestResult questResult = null;

    [SerializeField]
    private StatusManager statusManager = null;

    public QuestResult QuestResult
    {
        get { return questResult; }
    }

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
    private Dictionary<int, NonPlayer> characters = new Dictionary<int, NonPlayer>();


    private Ready ready;



    public StageTable stages;

    //private bool isLogout = true;

    private void Awake()
    {
        // ステージの作成
        GameObject stage = Instantiate<GameObject>(stages.FindPrefab(UserRecord.MapID), transform);
        if (UserRecord.Inventory.Count != 0) inventory_.ChangeItems(UserRecord.Inventory);

        stage.transform.position = Vector3.zero;

        if (connectFlag)
        {
            wsp = WS.WsPlay.Instance;
            wsp.MoveMap = false;
        }
        ready = Ready.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ユーザーID
        var user_id = UserRecord.ID;

        if (connectFlag)
        {
            SettingCallback();
            if (!UserRecord.FAST)
            {

                // セーブデータを要請する。
                wsp.Send(new Packes.SaveLoadCtoS(UserRecord.ID).ToJson());
                wsp.Send(new Packes.LoadingAccessoryMasterSend(UserRecord.ID).ToJson());
                UserRecord.FAST = true;

            }
            else
            {
                if (MakePlayer(new Vector3(-210, 0, -210), UserRecord.MODEL))
                {
                    updateFlag = true;
                    ready.ReadyGO();
                }
            }

        }
        else { MakePlayer(new Vector3(-210, 5, -210), characterModel.FindModel(CheckModel(0))); }
        questResult.SetScenes(this);
    }


    private int count = 0;
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
        // 強制終了
        if (InputManager.InputKeyCheck(KeyCode.Escape)) Quit();
        // 
        if (ready.CheckReady())
        {
            //ready.ReadyGO();
            if (player != null)
            {
                var playerData = player.PositionV4;
                if (Timer())
                {
                    if (connectFlag)
                    {
                        wsp.Send(new Packes.GetParameterSend(UserRecord.ID).ToJson());
                        SendPosition(playerData, (int)player.animationType);
                        SendStatus(UserRecord.ID, Packes.OBJECT_TYPE.PLAYER);
                        SendEnemyPosReq();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F2)) questResult.SetQuestFailed(Time.time);
        if (Input.GetKeyDown(KeyCode.F3)) questResult.SetQuestCrear(Time.time);
        if (Input.GetKeyDown(KeyCode.F4)) wsp.WsStatus();
        if (Input.GetKeyDown(KeyCode.F12)) SendMoveMap(MapID.Field);
        if (Input.GetKeyDown(KeyCode.F11)) SendMoveMap(MapID.Base);
    }

    public void QuestRetire()
    {
        if (questResult.IsResult != 1)
        {
            questResult.SetQuestFailed(Time.time);
        }
    }

    public void OnDestroy()
    {
        if (!connectFlag) return;

        if (wsp != null)
        {
            if (!wsp.MoveMap) wsp.Destroy();

        }
    }


    /// <summary>
    /// 自分の作成
    /// </summary>
    private bool MakePlayer(Vector3 _save, GameObject _playerModel, string _name = "player0")
    {
        bool ret = false;
        _name = (_name == "player0") ? _name = "player" + UserRecord.ID : _name;

        // プレイヤーが作られた事がないなら
        if (player == null)
        {
            MapDatas.MapData mapdata = MapDatas.FindOne(UserRecord.MapID);
            var tmp = Instantiate<GameObject>(_playerModel, this.transform);
            tmp.transform.position = new Vector3(mapdata.x, mapdata.y, mapdata.z);
            tmp.name = (UserRecord.Name != "") ? UserRecord.Name : _name;
            tmp.tag = "Player";
            //tmp.transform.localScale = new Vector3(2, 2, 2);

            if (cheatCommand != null) cheatCommand.PLAYER = tmp;

            player = tmp.AddComponent<Player>();
            PlayerController playerCComponent = tmp.AddComponent<PlayerController>();

            playerCComponent.Init(player, FollowingCamera, playerSetting, uiManager.GetChat(), tmp.GetComponent<Animator>());
            userSeeting.Init(tmp);
            FollowingCamera.Target = tmp;


            // ミニマップのカメラの作成
            var miniMapTmp = Instantiate<GameObject>(miniMapCameraPrefab_, this.transform);
            miniMapTmp.GetComponent<MiniMapController>().Init(tmp);

            // プレイヤーUIを設定
            playerUI.PLAYER_CMP = player;
            playerUI.PLAYER_NAME = UserRecord.Name;

            statusManager.PLAYER = player;

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
        if (_packet.user_id == 0) return;
        if (_packet.user_id == UserRecord.ID) return;


        // 他ユーザーの更新
        if (characters.ContainsKey(_packet.user_id))
        {
            if (characters[_packet.user_id] != null)
            {
                NonPlayer nonPlayer = characters[_packet.user_id];
                //Debug.Log(_packet.ToJson());
                nonPlayer.UpdatePostionData(_packet.x, _packet.y, _packet.z, _packet.dir);
                if ((PlayerAnim.PARAMETER_ID)_packet.animation == PlayerAnim.PARAMETER_ID.ATTACK) nonPlayer.GetComponent<OtherPlayers>().Weapon.SetActive(true);
                nonPlayer.ChangeAnimationType((PlayerAnim.PARAMETER_ID)_packet.animation);
            }
        }
        // todo 他プレイヤーの更新と作成を関数分けする
        // 他のユーザーの作成
        else
        {
            // リストに登録されていないIDが来たときの処理
            // そのIDは何なのか確認をとる
            wsp.Send(new Packes.FindOfPlayerCtoS(UserRecord.ID, _packet.user_id, 0).ToJson());
        }
    }

    /// <summary>
    /// 他プレイヤーの作成
    /// </summary>
    /// <param name="_packet">作成に必要なデータ</param>
    private void CreateOtherPlayers(Packes.OtherPlayersData _packet)
    {
        if (this == null) return;
        GameObject avatar = characterModel.FindModel(CheckModel(_packet.model_id));

        var otherPlayer = Instantiate<GameObject>
                          (avatar,
                          Vector3.zero,
                          Quaternion.Euler(0, 0, 0),
                          this.transform);                                  // 本体生成
        var mapData = MapDatas.FindOne(UserRecord.ID);
        otherPlayer.transform.position = new Vector3(mapData.x, mapData.y, mapData.z);
        GameObject name = Instantiate(nameUI, otherPlayer.transform);       // プレイヤーアイコン生成
        var other = otherPlayer.AddComponent<OtherPlayers>();               // OtherPlayerの追加
        //other.Weapon = otherPlayer.gameObject.FindDeep("sword", true);
        name.GetComponent<OtherUserNameUI>().UserName = otherPlayer.name
                                                      = other.Name
                                                      = _packet.name;       // 名前の共通化

        otherPlayer.tag = "OtherPlayer";                                    // タグ
        //otherPlayer.transform.localScale = new Vector3(2, 2, 2);
        other.Init(0, 0, 0, 0, _packet.user_id, skillTabe);
        other.Type = CharacterType.Other;
        characters[_packet.user_id] = other;                                 // キャラクター管理に登録
        Debug.Log("他キャラ生成" + _packet.user_id);
    }

    /// <summary>
    /// モデルIDのチェック
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private int CheckModel(int _id)
    {
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
            if (ene.unique_id != UserRecord.ID)
            {
                // 敵の更新
                if (characters.ContainsKey(ene.unique_id))
                {
                    if (characters[ene.unique_id] != null)
                    {
                        characters[ene.unique_id].UpdatePostionData(ene.x, ene.y, ene.z, ene.dir);
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
        if (enemyModel == null) return;

        GameObject newEnemy = Instantiate<GameObject>
                              (enemyModel,
                              Vector3.zero,
                              Quaternion.Euler(0, 0, 0));
        GameObject stutasCanvas = Instantiate(enemyStatusCanvas, newEnemy.transform);

        Enemy enemy = newEnemy.AddComponent<Enemy>();

        stutasCanvas.GetComponent<UIEnemyHP>().MAX_HP = enemy.HP = _ene.hp;
        stutasCanvas.GetComponent<UIEnemyHP>().Off();
        stutasCanvas.GetComponent<UIEnemyHP>().SetEnemyID(_ene.master_id);

        newEnemy.name = "Enemy:" + _ene.master_id + "->" + _ene.unique_id;
        enemy.Init(_ene.x, _ene.y, _ene.z, _ene.dir, _ene.unique_id, skillTabe);
        enemy.UI_HP = stutasCanvas.GetComponent<UIEnemyHP>();
        enemy.Type = CharacterType.Enemy;
        characters[_ene.unique_id] = enemy;
    }

    /// <summary>
    /// ステータスの更新　→ statusAction
    /// </summary>
    private void UpdateStatus(Packes.StatusStoC _packet)
    {
        foreach (var tmp in _packet.status)
        {
            if (tmp.charcter_id == UserRecord.ID)
            {
                player.UpdateStatus(tmp.max_hp, tmp.hp, tmp.max_mp, tmp.mp, tmp.status);
            }
            else
            {
                if (characters.ContainsKey(tmp.charcter_id))
                    characters[tmp.charcter_id].UpdateStatusData(tmp.hp, tmp.mp, tmp.status);
            }
        }


    }

    [SerializeField]
    private Inventory inventory_ = null;
    [SerializeField]
    NoDuplication accessory = null;

    /// <summary>
    /// セーブデータを受け取る→　loadSaveAction
    /// </summary>
    /// <param name="_save"></param>
    private void ReceiveSaveData(Packes.SaveLoadStoC _packet)
    {
        UserRecord.MODEL = characterModel.FindModel(CheckModel(_packet.model_id));

        if (MakePlayer(new Vector3(_packet.x, _packet.y, _packet.z), UserRecord.MODEL))
        {
            wsp.Send(new Packes.LoadingOK(UserRecord.ID).ToJson());
            updateFlag = true;
            inventory_.ChangeItems(_packet.accessorys);
            UserRecord.Inventory = _packet.accessorys;
            UserRecord.Accessorys = _packet.wearing_accessory;
            accessory.SetAccessorys(inventory_, _packet.wearing_accessory);
            ready.ReadyGO();
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
        Debug.Log("一覧作成");
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
        if (!characters.ContainsKey(_packet.unique_id)) return;
        if (characters[_packet.unique_id].Type != CharacterType.Enemy) return;

        Debug.Log("敵は生存している");
        Enemy enemy = characters[_packet.unique_id].GetComponent<Enemy>();
        //enemy.PlayTriggerAnimetion("Take Damage");
        enemy.enemyAnimType = EnemyAnim.PARAMETER_ID.DAMAGE;
        enemy.HP = _packet.hp;
        
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc.Target != null) {
            int target = pc.Target.GetComponentInParent<Enemy>().ID;
            if (target == _packet.unique_id) {
                var damageValue = (int)_packet.damage_value;
                uiManager.GetComponent<Damage>().CreateDamageUI(new Vector3(0, 0, 0), damageValue);
            }
        }
    }

    /// <summary>
    /// 戦闘処理死亡 → enemyDeadAction
    /// </summary>
    /// <param name="_packet"></param>
    private void DeadEnemy(Packes.EnemyDieStoC _packet)
    {
        Debug.Log(_packet.ToJson());
        // todo
        // 戦闘で計算後エネミーが死亡していたら
        // HPを0にして死亡エフェクトやドロップアイテムの取得
        if (!characters.ContainsKey(_packet.unique_id)) return;
        if (characters[_packet.unique_id].Type != CharacterType.Enemy) return;

        Enemy enemy = characters[_packet.unique_id].GetComponent<Enemy>();
        enemy.HP = 0;

        PlayerController pc = player.GetComponent<PlayerController>();
        if(pc.Target != null) { 
            int target = pc.Target.GetComponentInParent<Enemy>().ID;
            if (target == _packet.unique_id) {
                var damageValue = (int)_packet.damage_value;
                uiManager.GetComponent<Damage>().CreateDamageUI(new Vector3(0, 0, 0), damageValue);
                pc.RemoveTarget();
            }
        }
        characters.Remove(_packet.unique_id);
        //enemy.PlayTriggerAnimetion("Die");
        enemy.enemyAnimType = EnemyAnim.PARAMETER_ID.DIE;

        if (_packet.drop != 0) {
            inventory_.AddItem(_packet.drop);
        }
    }

    /// <summary>
    /// 他プレイヤーのスキルを再生 → ???
    /// </summary>
    /// <param name="_paket"></param>
    private void OthersUseSkills(Packes.OtherPlayerUseSkill _packet)
    {
        // todo
        // 他プレイヤーがスキルを使ったときの処理
        OtherPlayers other = characters[_packet.user_id].GetComponent<OtherPlayers>();
        other.animationType = PlayerAnim.PARAMETER_ID.ATTACK;
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
        Enemy enemy = characters[_packet.enemy_id].GetComponent<Enemy>();
        // enemy.PlayTriggerAnimetion("Attack");
        Debug.Log("スキルID : " + _packet.skill_id);
        enemy.enemyAnimType = EnemyAnim.PARAMETER_ID.ATTACK;
        //if (_packet.target_id == UserRecord.ID)
        //{
        if (enemy.Attacked(player.gameObject, _packet.skill_id))
        {
            Debug.Log("攻撃がヒットしたよ");
            wsp.Send(new Packes.Attack(UserRecord.ID, _packet.enemy_id, 0, 0).ToJson());
        }
        //}
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
    /// クエストがクリアしたときクリアUIを表示後ベースに移動 → questClearAction
    /// </summary>
    /// <param name="_data"></param>
    public void QuestClearAction(Packes.QuestClear _data)
    {
        // TODO: クリア時の実装
        questResult.SetQuestCrear(Time.time);
        Debug.Log("quese clear !!!");
    }



    /// <summary>
    /// マップ移動の許可が出たので移動を開始する → mapAction
    /// </summary>
    /// <param name="_packet"></param>
    private void MovingMap(Packes.MoveingMapOk _packet)
    {
        UserRecord.MapID = (MapID)_packet.mapId;
        UserRecord.Inventory = inventory_.AllItem();
        UserRecord.Accessorys = accessory.AllAccessory();
        ChangeScene("LoadingScene");
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

            UserRecord.DiscardAll();
            if (connectFlag)
            {
                wsp.Destroy();
            }
            ChangeScene("LoadingScene");
        }
        else if (UserRecord.ID != 0 && characters.ContainsKey(_packet.user_id))
        {
            Destroy(characters[_packet.user_id].gameObject);
            characters.Remove(_packet.user_id);
            Debug.Log(_packet.user_id + "さんがログアウトしたよ！");
        }
    }

    /// <summary>
    /// 検索結果のユーザーを作成 → findResultsAction
    /// </summary>
    /// <param name="_packet"></param>
    private void ReceivingFindResults(Packes.FindOfPlayerStoC _packet)
    {
        Debug.Log("検索した。"+ _packet.user_id+ _packet.name);
        if (characters.ContainsKey(_packet.user_id)) { Debug.Log("もういるよ"); return;}

        Debug.Log("検索の結果他プレイヤーを作成 -> " + _packet.user_id);
        CreateOtherPlayers(new Packes.OtherPlayersData(_packet.user_id, _packet.x, _packet.y, _packet.z, _packet.model_id, _packet.name));

    }




    // --------------------送信関係--------------------


    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos, int _anima)
    {
        wsp.Send(new Packes.TranslationCtoS(UserRecord.ID, _pos.x, _pos.y, _pos.z, _pos.w, _anima).ToJson());
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
        wsp.Send(new Packes.GetEnemysDataCtoS((int)UserRecord.MapID, UserRecord.ID).ToJson());
    }

    /// <summary>
    /// マップの移動申請
    /// </summary>
    /// <param name="_mapId"></param>
    public void SendMoveMap(MapID _mapId)
    {
        wsp.Send(new Packes.MoveingMap(UserRecord.ID, (int)_mapId).ToJson());
    }

    // ----------------ゲッター＆セッター--------------------------

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
    public OtherPlayers GetOtherPlayer(int _playerId)
    {
        return characters[_playerId].GetComponent<OtherPlayers>();
    }

    // ステータスの更新
    private void GetParameterAction(Packes.GetParameter _data)
    {
        player.INT = _data.intelligence;
        player.STR = _data.str;
        player.AGI = _data.agi;
        player.DEX = _data.dex;
        player.MND = _data.mnd;
        player.VIT = _data.vit;
    }

    private void PlayerDie(Packes.PlayerDie _data)
    {
        // TODO: プレイヤー死亡処理
        Debug.Log("はやかわ　たいき 1998年 4月 19日");
        SendMoveMap(MapID.Base);
    }

    // 他プレイヤーのマップ移動
    private void MoveingMapExitOtherAction(Packes.MoveingMapExitOther _data) {
        Destroy(characters[_data.user_id].gameObject);
        characters.Remove(_data.user_id);
        Debug.Log(_data.user_id + "さんが移動したよ！");
    }

    /// <summary>
    /// コールバックを設定
    /// </summary>
    private void SettingCallback()
    {
        wsp.moveingAction = UpdatePlayersPostion;                   // 202
        wsp.enemysAction = RegisterEnemies;                         // 204
        wsp.statusAction = UpdateStatus;                            // 206

        wsp.getParameterAction = GetParameterAction;
        wsp.loadSaveAction = ReceiveSaveData;                       // 212
        wsp.loadOtherListAction = ReceiveOtherListData;             // 214
        wsp.loadOtherAction = ReceiveOtherData;                     // 215
        wsp.playerDieAction = PlayerDie;

        wsp.enemyAliveAction = AliveEnemy;                          // 221
        wsp.enemyDeadAction = DeadEnemy;                            // 222
        wsp.enemySkillReqAction = EnemyUseSkillRequest;             // 225
        wsp.enemyUseSkillAction = EnemyUseSkill;                    // 226
        wsp.enemyAttackAction = EnemyAttackResult;                  // 227

        wsp.questClearAction = QuestClearAction;                    // 242

        wsp.mapAction = MovingMap;                                  // 252
        wsp.moveingMapExitOtherAction = MoveingMapExitOtherAction;

        wsp.logoutAction = Logout;                                  // 707

        wsp.findResultsAction = ReceivingFindResults;               // 712
    }
}