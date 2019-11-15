﻿////////////////////////////////////////
// タイトルシーンのマネージャークラス //
////////////////////////////////////////


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleSceneManager : MonoBehaviour
{
#pragma warning disable 0649
    //ID,PWの最大文字数
    private const int MAX_WORD = 16;
    // wsソケット
    WS.WsLogin wsl = null; 

    [SerializeField]
    bool connectFlag = false;

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
    private InputField id_;[SerializeField]
    //ログインPW入力用
    private InputField pw_;[SerializeField]
    //PW確認用
    private InputField ConfirmPW_;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if(Input.GetKeyDown(KeyCode.Tab)) InputChange();
        if (Input.GetKeyDown(KeyCode.Return)) EnterCheck();
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

    private void OnDestroy() {
        if(connectFlag) wsl.Destroy();
    }
    //public関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // 
    public void LogInClick()
    {
        LogInActive();
    }

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

        ErrorMessageHide();

        inputState = CANVAS_STATE.SELECT;
    }

    /// <summary>
    /// ログイン送信
    /// </summary>
    public void loginToGame_Click()
    {
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;

        LoginCheck cheack = new LoginCheck();
        var result = cheack.CheckIdAndPassword(id, pw);
        if (result == LoginCheck.CHECKRESULT.OK)
        {
            //Debug.Log("ログイン ID:" + id + "  PW:" + pw);
            ErrorMessageHide();
            if (connectFlag)
            {
                // ログイン処理
                wsl.Send(new Packes.LoginUser(id, pw).ToJson());
            }
        }
        else ErrorOn(result);
    }

    private void ErrorOn(LoginCheck.CHECKRESULT _type) {
        switch (_type) {
            case LoginCheck.CHECKRESULT.MINSTRING: Error01.gameObject.SetActive(true); break;
            case LoginCheck.CHECKRESULT.INVALID: Error02.gameObject.SetActive(true); break;
        }
        pw_.text = "";
    }

    /// <summary>
    /// 新規登録送信
    /// </summary>
    public void RegisterClick02()
    {
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
                //Debug.Log("新規登録 ID:" + id + "  PW:" + pw);
                //Error01.gameObject.SetActive(false);
                //Error02.gameObject.SetActive(false);
                //Error03.gameObject.SetActive(false);
                //Error04.gameObject.SetActive(false);
                if (connectFlag)
                {
                    // 送信処理
                    wsl.Send(new Packes.CreateUser(id, pw).ToJson());
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
    private void ChangeActiveUI(List<GameObject> _uis, bool _active = true) {
        foreach (var _ui in _uis) {
            _ui.SetActive(_active);
        }
    }

    /// <summary>
    /// 入力の反転
    /// </summary>
    /// <returns>成功/失敗</returns>
    private bool InputChange() {
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
                if (id_.isFocused) {
                    id_.DeactivateInputField();
                    pw_.ActivateInputField();
                } else if (pw_.isFocused) {
                    ConfirmPW_.ActivateInputField();
                    pw_.DeactivateInputField();
                } else {
                    id_.ActivateInputField();
                    ConfirmPW_.DeactivateInputField();
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
    private void EnterCheck() {
        loginToGame_Click();
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
}