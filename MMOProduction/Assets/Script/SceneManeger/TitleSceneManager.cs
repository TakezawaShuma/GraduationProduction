////////////////////////////////////////
// タイトルシーンのマネージャークラス //
////////////////////////////////////////


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;


public class TitleSceneManager : SceneManagerBase
{
#pragma warning disable 0649
    //ID,PWの最大文字数
    private const int MAX_WORD = 16;
    private const int UPDATE_MAX_COUNT = 30;
    // wsソケット
    WS.WsLogin wsl = null;
    
    //ボタンの種類
    public enum CANVAS_STATE
    {
        SELECT,    //選択
        SIGN_IN,    //ログイン
        SIGN_UP,    //新規登録
    }

    private enum ERROR_PATTERN
    {
        LACK,                   //文字数不足
        UNAVAILABLE,            //使用不可の文字がある
        DISAGREEMENT,           //パスワードが一致せず
        ALREADY_USED,           //すでに使われている
        DIFFERENT_IDorPASS,     //IDかPASSが異なる
        ALREADY_IN,             //すでにログインしている
        NON,
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
    private InputField confirmPW_;
    [SerializeField]
    private Button loginButton_;
    [SerializeField]
    private Button sinupButton_;
    // キャンバス
    private GameObject canvas;


    //Error用Text
    [SerializeField]
    private Text errorText;

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

        //Error01.GetComponent<RectTransform>().localPosition =
        //Error02.GetComponent<RectTransform>().localPosition =
        //new Vector3(75, -370, 0);

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
        }
        //マスターデータの要求
        SendPacket(new Packes.LoadingAccessoryMasterSend(UserRecord.ID).ToJson());
        SendPacket(new Packes.LoadingMapMasterSend(UserRecord.ID).ToJson());
        SendPacket(new Packes.LoadingQuestMasterSend(UserRecord.ID).ToJson());


        sound_ = GetComponent<SystemSound>();
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
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (Input.GetKeyDown(KeyCode.Tab)) InputChange();
        if (Input.GetKeyDown(KeyCode.Return)) EnterCheck();

        if (Timer())
        {
            //if (wsl.WsStatus() != WebSocketSharp.WebSocketState.Open)
            {
                // 接続不良が起きているのでエラーメッセージを表示

            }
        }
    }


    private void OnDestroy()
    {
        if (!connectFlag) return;
        if (wsl!=null) wsl.Destroy();
        
    }
    //public関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// ログインのボタン
    /// </summary>
    public void LoginClick()
    {
        LoginActive();
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
        confirmPW_.text = "";
        userName_.text = "";

        ErrorMessageHide();
        ButtonState(true);
        LoadingUIDelete();

        inputState = CANVAS_STATE.SELECT;
    }

    /// <summary>
    /// ログイン送信
    /// </summary>
    public void LoginButton()
    {
        // 入力を規制
        ButtonState(false);
        LoadingUIInstantiate(loginButton_.transform);
        ErrorMessageHide();
        

        if (SetErrorText(CheckErrorType(id_.text, pw_.text)))
        {
            if(!SendPacket(new Packes.LoginUser(id_.text, pw_.text).ToJson()))
            {
                ErrorOn();
            }
        }
        else ErrorOn();
    }

    /// <summary>
    /// 新規登録送信
    /// </summary>
    public void CreateButton()
    {
        ButtonState(false);
        LoadingUIInstantiate(sinupButton_.transform);
        ErrorMessageHide();


        if (SetErrorText(CheckErrorType(id_.text, pw_.text,confirmPW_.text)))
        {
            SendPacket(new Packes.LoginUser(id_.text, pw_.text).ToJson());
        }
        else ErrorOn();
    }

    /// <summary>
    /// クライアントで判断できるエラー処理
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_pass"></param>
    /// <param name="_confirmPass"></param>
    /// <returns></returns>
    private ERROR_PATTERN CheckErrorType(string _id, string _pass, string _confirmPass = "")
    {
        // 指定文字数以上か
        if (_id.Length < 4 || _pass.Length < 4)
        {
            return ERROR_PATTERN.LACK;
        }
        // 使用不可能文字がある
        else if (!CheackWord(_id) || !CheackWord(_pass))
        {
            return ERROR_PATTERN.UNAVAILABLE;
        }
        // パスワードと確認用パスワードが違います。
        else if (inputState == CANVAS_STATE.SIGN_UP && _pass != _confirmPass)
        {
            return ERROR_PATTERN.DISAGREEMENT;
        }
        return ERROR_PATTERN.NON;
    }

    /// <summary>
    /// サーバーからのエラー処理
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private ERROR_PATTERN CheckErrorType(int _type)
    {
        if (_type == (int)ERROR_PATTERN.ALREADY_USED) return ERROR_PATTERN.ALREADY_USED;
        else if (_type == (int)ERROR_PATTERN.DIFFERENT_IDorPASS) return ERROR_PATTERN.DIFFERENT_IDorPASS;
        else if (_type == (int)ERROR_PATTERN.ALREADY_IN) return ERROR_PATTERN.ALREADY_IN;
       
        return ERROR_PATTERN.NON;
    }

    /// <summary>
    /// 使用可能な文字か確認
    /// </summary>
    /// <param name="_str"></param>
    /// <returns>true＝問題無し/false＝使用不可能な文字がある</returns>
    private bool CheackWord(string _str) { return Regex.IsMatch(_str, @"^[0-9a-zA-Z-_]+$"); }

    /// <summary>
    /// エラーテキストを設定表示する
    /// </summary>
    /// <param name="_pttern"></param>
    private bool SetErrorText(ERROR_PATTERN _pttern)
    {
        bool ret = true;
        switch (_pttern)
        {
            case ERROR_PATTERN.LACK:
                errorText.text = "文字数が足りません。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            case ERROR_PATTERN.UNAVAILABLE:
                errorText.text = "使用できない文字が含まれています。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            case ERROR_PATTERN.DISAGREEMENT:
                errorText.text = "PWが一致しません。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            case ERROR_PATTERN.ALREADY_USED:
                errorText.text = "既に使われているIDです。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            case ERROR_PATTERN.DIFFERENT_IDorPASS:
                errorText.text = "IDまたPasswordが違います。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            case ERROR_PATTERN.ALREADY_IN:
                errorText.text = "そのIDはすでにログインしています。";
                errorText.gameObject.SetActive(true);
                ret = false;
                break;
            default:
                errorText.gameObject.SetActive(false);
                break;
        }
        return ret;
    }


    private void ErrorOn()
    {
        pw_.text = "";
        confirmPW_.text = "";
        LoadingUIDelete();
        ButtonState(true);
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
    /// 入力の欄の変更(フォーカス切り替え)
    /// </summary>
    /// <returns>成功/失敗</returns>
    private bool InputChange()
    {
        if (!id_.isFocused && !pw_.isFocused && !confirmPW_.isFocused) return false;
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
                    pw_.DeactivateInputField();
                    confirmPW_.ActivateInputField();
                }
                else if (confirmPW_.isFocused)
                {
                    confirmPW_.DeactivateInputField();
                    userName_.ActivateInputField();
                }
                else if (userName_.isFocused)
                {
                    userName_.DeactivateInputField();
                    id_.ActivateInputField();
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
        if (inputState == CANVAS_STATE.SIGN_IN) LoginButton();
        else if (inputState == CANVAS_STATE.SIGN_UP) CreateButton();
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

    private void LoginActive()
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
        errorText.gameObject.SetActive(false);
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


    private bool SendPacket(string _data)
    {
        if (!connectFlag) return false;
        wsl.Send(_data);
        return true;
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
    void ErrorAction(Packes.LoginError _packet)
    {
        if (inputState != CANVAS_STATE.SIGN_IN)
        {
            ButtonState(true);
            LoadingUIDelete();
        }
        else
        {
            // 再送信
            if (errorCount < 5)
            {
                StartCoroutine(ReSend(wsl, new Packes.LoginUser(id_.text, pw_.text).ToJson()));
                errorCount++;
            }
            else
            {
                
                errorCount = 0;
                SetErrorText(CheckErrorType(_packet.errorType));
                ButtonState(true);
                LoadingUIDelete();
            }
        }
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


