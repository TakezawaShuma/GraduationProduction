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
    public GameObject playerPre = null;

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

    // コールバック関数をリスト化
    private List<Action<string>> callbackList = new List<Action<string>>();

    private GameObject player_;

    //EnemyManager
    EnemyManager enm = new EnemyManager();

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // ユーザーID
        var user_id = UserRecord.ID;
        if (connectFlag)
        {
            // プレイサーバに接続
            wsp = new WS.WsPlay(8001);
            wsp.moveingAction = UpdatePlayers;  // 202
            wsp.enemysAction = RegisterEnemies; // 204
            wsp.statusAction = UpdateStatus;    // 206
            wsp.loadSaveAction = RecvSaveData;  // 210
            wsp.loadFinAction = LoadFinish;     // 212
            wsp.enemyAliveAction = AliveEnemy;  // 221
            wsp.enemyDeadAction = DeadEnemy;    // 222
            wsp.logoutAction = Logout;          // 
            wsp.Send(new Packes.DataLoading(UserRecord.ID).ToJson());

        }
        Debug.Log("プレイスタート");
        MakePlayer(new Vector3(5, 1, 15));
        UpdatePlayers(new Packes.TranslationStoC(100, 0, 0, 10,0));
        //テスト用のEnemy情報を追加
        enm.AddEnemy(0, "Forest Bunny Black");
        var newEnemy = Instantiate<GameObject>(enm.GetEnemyPrefab(0), new Vector3(1,0,5.8f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateFlag)
        {
            if (player!=null)
            {
                var playerData = player.GetPosition();
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
    private void MakePlayer(Vector3 _save,string _name= "player0")
    {
        _name = "player" + UserRecord.ID;
        if (player == null)
        {
            var tmp = Instantiate<GameObject>(playerPre);
            tmp.transform.position = new Vector3(_save.x, _save.y, _save.z);
            tmp.name = (UserRecord.Name != "") ? UserRecord.Name : _name;
            tmp.AddComponent<Player>();
            tmp.AddComponent<PlayerController>();
            tmp.AddComponent<PlayerSetting>();
            tmp.GetComponent<PlayerController>().Init(tmp.GetComponent<Player>(), FollowingCamera, tmp.GetComponent<PlayerSetting>(), chat);
            player = tmp.GetComponent<Player>();
            FollowingCamera.SetTarget(tmp);
        }
    }


    // コールバック系統↓

    /// <summary>
    /// 自分以外のユーザーの更新　→　moveingAction
    /// </summary>
    private void UpdatePlayers(Packes.TranslationStoC _packet)
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
                        others[data.user_id].UpdataData(0, 0, data.x, data.y, data.z, data.dir);
                    }
                }
                // todo 他プレイヤーの更新と作成を関数分けする
                // 他のユーザーの作成
                else
                {
                    var otherPlayer = Instantiate<GameObject>(playerPre, new Vector3(data.x, data.y, data.z), Quaternion.Euler(0, data.dir, 0));
                    otherPlayer.name = "otherPlayer" + data.user_id;
                    otherPlayer.AddComponent<OtherPlayers>();
                    otherPlayer.GetComponent<OtherPlayers>().Init(data.user_id);
                    others.Add(data.user_id, otherPlayer.GetComponent<OtherPlayers>());
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
        foreach(var ene in list)
        {
            if(ene.unique_id!=UserRecord.ID)
            {
                // 敵の更新
                if (enemies.ContainsKey(ene.unique_id))
                {
                    if (enemies[ene.unique_id] != null)
                    {
                        enemies[ene.unique_id].UpdataData(ene.hp, 0, ene.x, ene.y, ene.z, ene.dir);
                    }
                }
                // 敵の作成
                else
                {
                    var newEnemy = Instantiate<GameObject>(testEnemyPre, new Vector3(ene.x, ene.y, ene.z), Quaternion.Euler(0, ene.dir, 0));
                    newEnemy.name = "Enemy:" + ene.master_id + "->" + ene.unique_id;
                    Enemy enemy = newEnemy.AddComponent<Enemy>();
                    enemies.Add(ene.unique_id, enemy);
                }
            }
        }
    }

    /// <summary>
    /// ステータスの更新　→ statusAction
    /// </summary>
    private void UpdateStatus(Packes.StatusStoC _packet)
    {
        // todo
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

        Debug.Log("敵は死んだ！！！");
    }

    /// <summary>
    /// 他プレイヤーがログアウトした時 → logoutAction
    /// </summary>
    /// <param name="_packet"></param>
    private void Logout(Packes.LogoutStoC _packet)
    {
        Destroy(others[_packet.user_id].gameObject);
        others.Remove(_packet.user_id);
    }


    // --------------------送信関係--------------------

    /// <summary>
    /// 位置情報の送信
    /// </summary>
    private void SendPosition(Vector4 _pos)
    {
        wsp.Send(new Packes.TranslationCtoS(UserRecord.ID, _pos.x, _pos.y, _pos.z, _pos.w).ToJson());
    }
    
    private void SendStatus(int _maxHp,int _hp,int _maxMp,int _mp ,int _status)
    {
        wsp.Send(new Packes.StatusCtoS(UserRecord.ID, _maxHp, _hp, _maxMp, _mp, _status).ToJson());
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
        return others[_playerId].GetComponent<OtherPlayers>();
    }

}
