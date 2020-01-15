using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManger : SceneManagerBase
{


    public GameObject playerPre = null;

    [SerializeField, Header("キャラクターモデルリスト")]
    private character_table characterModel = null;

    // ソケット
    private WS.WsPlay wsp = null;

    // プレイヤー情報
    private Player player = null;
    /// <summary> 自分自身の情報を渡す </summary>
    public Player Player { get { return player; } }
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

            SettingCallback();

            // セーブデータを要請する。
            wsp.Send(new Packes.SaveLoadCtoS(UserRecord.ID).ToJson());
            wsp.Send(new Packes.LoadingAccessoryMasterSend(UserRecord.ID).ToJson());

        }
        else { MakePlayer(new Vector3(-210, 5, -210), characterModel.tables[0].model); }
    }


    // Update is called once per frame
    void Update()
    {
        
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
            /*
            var tmp = Instantiate<GameObject>(_playerModel, this.transform);
            tmp.transform.position = new Vector3(_save.x, _save.y, _save.z);
            tmp.name = (UserRecord.Name != "") ? UserRecord.Name : _name;
            tmp.tag = "Player";
            tmp.transform.localScale = new Vector3(2, 2, 2);

            //cheatCommand.PLAYER = tmp;

            player = tmp.AddComponent<Player>();
            PlayerController playerCComponent = tmp.AddComponent<PlayerController>();

            playerCComponent.Init(player, FollowingCamera, playerSetting, chatController, tmp.GetComponent<Animator>());
            userSeeting.Init(tmp);
            FollowingCamera.Target = tmp;


            // ミニマップのカメラの作成
            var miniMapTmp = Instantiate<GameObject>(miniMapCameraPrefab_, this.transform);
            miniMapTmp.GetComponent<MiniMapController>().Init(tmp);

            // プレイヤーUIを設定
            playerUI.PLAYER_CMP = player;
            playerUI.PLAYER_NAME = UserRecord.Name;
            */
            ret = true;
        }
        return ret;
    }


    /// <summary>
    /// コールバックを設定
    /// </summary>
    private void SettingCallback()
    {

    }
    }
