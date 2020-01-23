////////////////////////////////////////
// タイトルシーンのマネージャークラス //
////////////////////////////////////////


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TitleSceneManager : SceneManagerBase
{
#pragma warning disable 0649
    //ID,PWの最大文字数
    private const int MAX_WORD = 16;
    // wsソケット
    WS.WsLogin wsl = null;
    
    //ボタンの種類
    public enum CANVAS_STATE
    {
        SELECT,    //選択
        SIGN_IN,    //ログイン
        SIGN_UP,    //新規登録
    }
    CANVAS_STATE inputState = CANVAS_STATE.SELECT;


    // UIのリスト化
    public List<GameObject> selectUIList_;
    public List<GameObject> loginUIList_;
    public List<GameObject> sinupUIList_;

    [SerializeField]
    //ログインID入力用
    private InputField id_;
    [SerializeField]
    //ログインPW入力用
    private InputField pw_;
    // ユーザー名入力
    [SerializeField]
    private InputField userName_;
    [SerializeField]
    //PW確認用
    private InputField ConfirmPW_;
    [SerializeField]
    private Button loginButton_;
    [SerializeField]
    private Button sinupButton_;
    // キャンバス
    private GameObject canvas;


    //Error用Text
    // 文字数が足りない時
    [SerializeField]
    private Text Error01;
    // 使用できない文字があります
    [SerializeField]
    private Text Error02;
    // パスワードが一致しません
    [SerializeField]
    private Text Error03;
    // すでに使われているID　command 106
    [SerializeField]
    private Text Error04;
    // IDまたはPWが違う　command 104
    [SerializeField]
    private Text Error05;
    // 重複ログイン　command 901
    [SerializeField]
    private Text Error06;

    // ロード用の円
    [SerializeField]
    private GameObject loadingCirclePrefab_;
    private GameObject loadingCircle;

    private SystemSound sound_;
    // Start is called before the first frame update
    void Start()
    {
        //初期のGameObjectの表示設定
        ChangeActiveUI(selectUIList_);
        ChangeActiveUI(loginUIList_, false);
        ChangeActiveUI(sinupUIList_, false);

        ErrorMessageHide();

        Error01.GetComponent<RectTransform>().localPosition =
        Error02.GetComponent<RectTransform>().localPosition =
        new Vector3(75, -370, 0);

        //Input Field の入力文字数制限
        id_.characterLimit = MAX_WORD;
        pw_.characterLimit = MAX_WORD;
        if (connectFlag)
        {
            // 接続開始
            wsl = WS.WsLogin.Instance;
            wsl.errerAction = ErrorAction;
            wsl.createAction = CreateAction;
            wsl.loginAction = LoginAction;
            wsl.loadingAccessoryMasterAction = LoadingAccessoryMaster;
            wsl.loadingMapMasterAction = LoadingMapMaster;
            wsl.loadingQuestMasterAction = LoadingQuestMaster;

            wsl.Send(new Packes.LoadingAccessoryMasterSend(UserRecord.ID).ToJson());
            wsl.Send(new Packes.LoadingMapMasterSend(UserRecord.ID).ToJson());
            wsl.Send(new Packes.LoadingQuestMasterSend(UserRecord.ID).ToJson());
        }

        sound_ = GetComponent<SystemSound>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (Input.GetKeyDown(KeyCode.Tab)) InputChange();
        if (Input.GetKeyDown(KeyCode.Return)) EnterCheck();
    }


    private void OnDestroy()
    {
        if (connectFlag) wsl.Destroy();
    }
    //public関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// ログインのボタン
    /// </summary>
    public void LogInClick()
    {
        LogInActive();
    }

    /// <summary>
    /// 新規作成のボタン
    /// </summary>
    public void RegisterClick()
    {
        RegisterActive();
    }

    public void BackClick()
    {
        ChangeActiveUI(selectUIList_);
        ChangeActiveUI(loginUIList_, false);
        ChangeActiveUI(sinupUIList_, false);

        id_.text = "";
        pw_.text = "";
        ConfirmPW_.text = "";
        userName_.text = "";

        ErrorMessageHide();
        ButtonState(true);
        LoadingUIDelete();

        inputState = CANVAS_STATE.SELECT;
    }

    /// <summary>
    /// ログイン送信
    /// </summary>
    public void loginToGame_Click()
    {
        ButtonState(false);
        LoadingUIInstantiate(loginButton_.transform);
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;

        LoginCheck cheack = new LoginCheck();
        var result = cheack.CheckIdAndPassword(id, pw);
        if (result == LoginCheck.CHECKRESULT.OK)
        {
            ErrorMessageHide();
            if (connectFlag)
            {
                // ログイン処理
                wsl.Send(new Packes.LoginUser(id, pw).ToJson());
            }
        }
        else ErrorOn(result);
    }

    private void ErrorOn(LoginCheck.CHECKRESULT _type)
    {
        switch (_type)
        {
            case LoginCheck.CHECKRESULT.MINSTRING: Error01.gameObject.SetActive(true); break;
            case LoginCheck.CHECKRESULT.INVALID: Error02.gameObject.SetActive(true); break;
        }
        pw_.text = "";
        LoadingUIDelete();
        ButtonState(true);
    }

    /// <summary>
    /// 新規登録送信
    /// </summary>
    public void RegisterClick02()
    {
        ButtonState(false);
        LoadingUIInstantiate(sinupButton_.transform);
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;

        LoginCheck cheack = new LoginCheck();
        var result = cheack.CheckIdAndPassword(id, pw);
        if (result == LoginCheck.CHECKRESULT.OK)
        {
            // 新規登録
            if (pw_.text == ConfirmPW_.text)
            {
                if (connectFlag)
                {
                    // 送信処理
                    wsl.Send(new Packes.CreateUser(id, pw, userName_.text).ToJson());
                }
            }
            else
            {
                pw_.text = "";
                ConfirmPW_.text = "";
                Error03.gameObject.SetActive(true);
            }

        }
        else
        {
            ErrorOn(result);
            ConfirmPW_.text = "";
        }

    }

    //private関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    /// <summary>
    /// 最初の選択ボタンの状態変化
    /// </summary>
    private void ChangeActiveUI(List<GameObject> _uis, bool _active = true)
    {
        foreach (var _ui in _uis)
        {
            _ui.SetActive(_active);
        }
    }

    /// <summary>
    /// 入力の反転
    /// </summary>
    /// <returns>成功/失敗</returns>
    private bool InputChange()
    {
        if (!id_.isFocused && !pw_.isFocused && !ConfirmPW_.isFocused) return false;
        switch (inputState)
        {
            case CANVAS_STATE.SELECT: break;
            case CANVAS_STATE.SIGN_IN:
                if (id_.isFocused)
                {
                    id_.DeactivateInputField();
                    pw_.ActivateInputField();
                }
                else
                {
                    id_.ActivateInputField();
                    pw_.DeactivateInputField();
                }
                break;

            case CANVAS_STATE.SIGN_UP:
                if (id_.isFocused)
                {
                    id_.DeactivateInputField();
                    pw_.ActivateInputField();
                }
                else if (pw_.isFocused)
                {
                    ConfirmPW_.ActivateInputField();
                    pw_.DeactivateInputField();
                }
                else if (ConfirmPW_.isFocused)
                {
                    userName_.ActivateInputField();
                    ConfirmPW_.DeactivateInputField();
                }
                else if (userName_.isFocused)
                {
                    id_.ActivateInputField();
                    userName_.DeactivateInputField();
                }
                break;

            default: break;
        }


        return true;
    }

    /// <summary>
    /// 入力完了
    /// </summary>
    /// <returns></returns>
    private void EnterCheck()
    {
        if (inputState == CANVAS_STATE.SIGN_IN) loginToGame_Click();
        else if (inputState == CANVAS_STATE.SIGN_UP) RegisterClick02();
    }


    // 新規登録選択時表示させる
    private void RegisterActive()
    {
        // 状態の変化
        ChangeActiveUI(selectUIList_, false);
        ChangeActiveUI(sinupUIList_);
        inputState = CANVAS_STATE.SIGN_UP;

        id_.ActivateInputField();
    }

    private void LogInActive()
    {
        // 状態の変化
        ChangeActiveUI(selectUIList_, false);
        ChangeActiveUI(loginUIList_);
        inputState = CANVAS_STATE.SIGN_IN;

        id_.ActivateInputField();
    }


    ///
    /// エラーメッセージを全て非表示にする。
    ///
    public void ErrorMessageHide()
    {
        Error01.gameObject.SetActive(false);
        Error02.gameObject.SetActive(false);
        Error03.gameObject.SetActive(false);
        Error04.gameObject.SetActive(false);
        Error05.gameObject.SetActive(false);
        Error06.gameObject.SetActive(false);
    }


    /// <summary>
    /// キャンバスにオブジェクトを出す
    /// </summary>
    /// <param name="_prefubs"></param>
    private void UIInstantiate<T>(List<GameObject> _prefabs, List<T> _tmpList)
    {
        foreach (var prefab in _prefabs)
        {
            _tmpList.Add(Instantiate<GameObject>(prefab, canvas.transform).GetComponent<T>());
        }
    }

    /// <summary>
    /// UIの削除
    /// </summary>
    /// <param name="_ui"></param>
    private void UIDelete(List<GameObject> _ui)
    {
        foreach (var ui in _ui)
        {
            Destroy(ui);
        }
        _ui.Clear();
    }

    /// <summary>
    /// ボタンの入力状態の変更
    /// </summary>
    private void ButtonState(bool _state) => loginButton_.interactable = sinupButton_.interactable = _state;


    /// <summary>
    /// 読み込みUIの実体化
    /// </summary>
    private void LoadingUIInstantiate(Transform _parent) => loadingCircle = Instantiate<GameObject>(loadingCirclePrefab_, _parent);

    /// <summary>
    /// 読み込みUIの削除
    /// </summary>
    private void LoadingUIDelete()
    {
        Destroy(loadingCircle);
        loadingCircle = null;
    }



    // 受信時のメゾット -----------------------------------
    /// <summary>
    /// 新規作成完了
    /// </summary>
    /// <param name="_packet"></param>
    private void CreateAction(Packes.CreateOK _packet)
    {
        wsl.Send(new Packes.LoginUser(id_.text, pw_.text).ToJson());
    }

    /// <summary>
    /// ログイン完了
    /// </summary>
    /// <param name="_packet"></param>
    private void LoginAction(Packes.LoginOK _packet)
    {
        UserRecord.ID = _packet.user_id;
        UserRecord.Name = _packet.name;

        if (inputState == CANVAS_STATE.SIGN_IN) { UserRecord.MapID = MapID.Base; ChangeScene("LoadingScene"); }
        else if (inputState == CANVAS_STATE.SIGN_UP) { UserRecord.MapID = MapID.Base; ChangeScene("CharacterSelectScene"); }
    }

    int errorCount = 0;
    /// <summary>
    /// 確認エラー
    /// </summary>
    /// <param name="_packet"></param>
    void ErrorAction(int _packet)
    {
        if (inputState != CANVAS_STATE.SIGN_IN)
        {
            ButtonState(true);
            LoadingUIDelete();
        }
        else if (errorCount < 10) { StartCoroutine(ReSend(wsl, new Packes.LoginUser(id_.text, pw_.text).ToJson())); errorCount++; }
        else { errorCount = 0; }
    }

    // 選択の音
    public void EnterSoundPlay() => sound_.SystemPlay(SYSTEM_SOUND_TYPE.ENTER);


    /// <summary>
    /// マップのマスター取得
    /// </summary>
    /// <param name="_data"></param>
    private void LoadingMapMaster(Packes.LoadingMapMaster _data)
    {
        List<MapDatas.MapData> maps = new List<MapDatas.MapData>();
        foreach (var map in _data.maps)
        {
            maps.Add(new MapDatas.MapData(map.id, map.x, map.y, map.z, map.dir, null));
        }
        InputFile.WriterJson(MasterFileNameList.map, JsonUtility.ToJson(_data), FILETYPE.JSON);
        MapDatas.SaveingData(maps);
    }

    /// <summary>
    /// アクセサリのマスター保存 → loadingAccessoryMasterAction
    /// </summary>
    /// <param name="_data"></param>
    private void LoadingAccessoryMaster(Packes.LoadingAccessoryMaster _data)
    {
        InputFile.WriterJson(MasterFileNameList.accessory, JsonUtility.ToJson(_data), FILETYPE.JSON);
        AccessoryDatas.SaveingData(_data.accessorys);
    }


    private void LoadingQuestMaster(Packes.LoadingQuestMaster _data) {
        InputFile.WriterJson(MasterFileNameList.quest, JsonUtility.ToJson(_data), FILETYPE.JSON);
        QuestDatas.SaveingData(_data.quests);
    }
}
