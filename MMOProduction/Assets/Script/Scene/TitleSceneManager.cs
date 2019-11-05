////////////////////////////////////////
// タイトルシーンのマネージャークラス //
////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Text.RegularExpressions;


public class TitleSceneManager : MonoBehaviour
{
#pragma warning disable 0649
    //ID,PWの最大文字数
    private const int MAX_WORD = 16;

    public const int MIN_ID_WORD = 4;
    public const int MIN_PW_WORD = 4;

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
    //ログインを選択
    [SerializeField]
    private Button selectLogin_;
    //ログインする
    [SerializeField]
    private Button loginToGame_;
    //新規登録
    [SerializeField]
    private Button selectSignUp_;
    //戻る
    [SerializeField]
    private Button back_;
    //アカウントを作る
    [SerializeField]
    private Button signUp_;


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


    //ログインID入力用
    [SerializeField]
    private InputField id_;
    //ログインPW入力用
    [SerializeField]
    private InputField pw_;
    //PW確認用
    [SerializeField]
    private InputField ConfirmPW_;



    // Start is called before the first frame update
    void Start()
    {
        //初期のGameObjectの表示設定
        selectLogin_.gameObject.SetActive(true);
        selectSignUp_.gameObject.SetActive(true);
        back_.gameObject.SetActive(false);
        id_.gameObject.SetActive(false);
        pw_.gameObject.SetActive(false);
        ConfirmPW_.gameObject.SetActive(false);
        loginToGame_.gameObject.SetActive(false);
        signUp_.gameObject.SetActive(false);


        ErrorMessageHide();

        //Input Field の入力文字数制限
        id_.characterLimit = MAX_WORD;
        pw_.characterLimit = MAX_WORD;
        if (connectFlag)
        {
            // 接続開始
            wsl = WS.WsLogin.Instance;
        }

        int[] enemyyyy =
        {
           100,
           200,
           300,
           400,
        };
    }

    private int m_command = 0;

    /// <summary>
    /// 受信したデータを扱う関数
    /// </summary>
    void Receive(int _comand)
    {
        m_command = _comand;

        ErrorCheck(_comand);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            wsl.Send(new Packes.LoginUser("tsit", "trident").ToJson());
        }
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

    private void OnDestroy()
    {
        if(connectFlag) wsl.Destroy();
    }
    //public関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //ボタンが押されたとき
    public void Click()
    {
    }

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
        selectLogin_.gameObject.SetActive(true);
        selectSignUp_.gameObject.SetActive(true);
        back_.gameObject.SetActive(false);
        id_.gameObject.SetActive(false);
        id_.text = "";
        pw_.gameObject.SetActive(false);
        pw_.text = "";
        loginToGame_.gameObject.SetActive(false);
        signUp_.gameObject.SetActive(false);

        ErrorMessageHide();
        ConfirmPW_.gameObject.SetActive(false);
        ConfirmPW_.text = "";

    }

    /// <summary>
    /// ログイン送信
    /// </summary>
    public void loginToGame_Click()
    {
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;

        if (CheckString(id, true) == true && CheckString(pw, false) == true)
        {
            //Debug.Log("ログイン ID:" + id + "  PW:" + pw);
            ErrorMessageHide();
            if (connectFlag)
            {
                // ログイン処理
                wsl.Send(new Packes.LoginUser(id, pw).ToJson());
            }
        }
        else
        {
            pw_.text = "";
        }
    }

    /// <summary>
    /// 新規登録送信
    /// </summary>
    public void RegisterClick02()
    {
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;
        if(CheckString(id,true)==true&&CheckString(pw,false)==true)
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
            pw_.text = "";

            ConfirmPW_.text = "";
        }

    }

    //id入力用input fieldでEnterが押された時の処理
    public void idEndEdit()
    {
        pw_.ActivateInputField();
    }

    //PW入力用input fieldでEnterが押された時の処理
    public void pwEndEdit()
    {
        loginToGame_Click();
    }

    //private関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void LogInActive()
    {

        selectLogin_.gameObject.SetActive(false);
        selectSignUp_.gameObject.SetActive(false);
        back_.gameObject.SetActive(true);
        id_.gameObject.SetActive(true);
        pw_.gameObject.SetActive(true);
        loginToGame_.gameObject.SetActive(true);
        
        RectTransform er01= Error01.GetComponent<RectTransform>();
        er01.localPosition = new Vector3(75, -280, 0);
        RectTransform er02 = Error02.GetComponent<RectTransform>();
        er02.localPosition = new Vector3(75, -280, 0);
    }

    // 新規登録選択時表示させる
    private void RegisterActive()
    {
        selectLogin_.gameObject.SetActive(false);
        selectSignUp_.gameObject.SetActive(false);
        back_.gameObject.SetActive(true);
        id_.gameObject.SetActive(true);
        pw_.gameObject.SetActive(true);
        signUp_.gameObject.SetActive(true);
        ConfirmPW_.gameObject.SetActive(true);
        RectTransform er01 = Error01.GetComponent<RectTransform>();
        er01.localPosition = new Vector3(75, -370, 0);
        RectTransform er02 = Error02.GetComponent<RectTransform>();
        er02.localPosition = new Vector3(75, -370, 0);
    }

    //string内にスペースや全角文字がないかをしらべる
    private bool CheckString(string str,bool id)
    {
        //文字が入っていない場合
        if (str == "")
        {
            Error01.gameObject.SetActive(true);
            return false;
        }

        //チェックするのがIDかどうか
        if(id)
        {
            // 4文字以下なら弾く
            if (str.Length < MIN_ID_WORD)
            {
                Error01.gameObject.SetActive(true);
                return false;
            }
            //英数字と_のみか判定
            if(!Regex.IsMatch(str, @"^[0-9a-zA-Z-_]+$"))
            {
                Error02.gameObject.SetActive(true);
                return false;
            }
            return true;
        }
        // パスワードなら
        else
        {
            if (str.Length < MIN_PW_WORD)
            {
                Error01.gameObject.SetActive(true);
                return false;
            }
            //英数字のみか判定
            if (!Regex.IsMatch(str, @"^[0-9a-zA-Z]+$"))
            {
                Error02.gameObject.SetActive(true);
                return false;
            }
            return true;

        }
    }


    //IDがすでに使われていたらエラーメッセージを出す
    void AlreadyInUse()
    {
        ErrorMessageHide();
        pw_.text = "";
        ConfirmPW_.text = "";

        ErrorMessageHide();
        Error04.gameObject.SetActive(true);
    }

    //IDかPWを間違えていたらエラーメッセージを出す
    void TypingError()
    {
        ErrorMessageHide();
        Error05.gameObject.SetActive(true);
    }

    //すでにログインされているIDの場合エラーメッセージを出す
    void  MultipleLoginError()
    {
        ErrorMessageHide();
        Error06.gameObject.SetActive(true);
    }


    //エラー確認
    public bool ErrorCheck(int _comand)
    {
        switch(_comand)
        {
            case (int)CommandData.LoginError:  // command 104
                //IDかPWが間違っている
                TypingError();
                break;
            //case (int)CommandData.Existing: // command 106
            //    //すでに使われているID
            //    AlreadyInUse();
            //    break;
            case (int)CommandData.Duplicate:    // command 901
                //すでにログインしているID
                MultipleLoginError();
                break;
            default:
                return true;
        }
        return false;
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
