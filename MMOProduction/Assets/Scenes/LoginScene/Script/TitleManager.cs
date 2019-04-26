using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Text.RegularExpressions;


public class TitleManager : MonoBehaviour
{
#pragma warning disable 0649
    //ID,PWの最大文字数
    private const int MAX_WORD = 16;

    public const int MIN_ID_WORD = 4;
    public const int MIN_PW_WORD = 4;

    // wsソケット
    Connect.ConnectLogin ws = new Connect.ConnectLogin();

    //ボタンの種類
    public enum CANVAS_STATE
    {
        SELECT,    //選択
        SIGN_IN,    //ログイン
        SIGN_UP,    //新規登録
    }
    //ログインを選択
    [SerializeField]
    private Button login01_;
    //ログインする
    [SerializeField]
    private Button login02_;
    //登録
    [SerializeField]
    private Button register_;
    //戻る
    [SerializeField]
    private Button back_;
    //アカウントを作る
    [SerializeField]
    private Button createAccount_;


    //Error用Text
    [SerializeField]
    private Text Error01;
    [SerializeField]
    private Text Error02;
    [SerializeField]
    private Text Error03;
    
    // すでに使われているID　エラー
    [SerializeField]
    private Text Error04;
    
    // IDまたはPWが違う　エラー
    [SerializeField]
    private Text Error05;

    
    // 重複ログイン　エラー
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
        login01_.gameObject.SetActive(true);
        register_.gameObject.SetActive(true);
        back_.gameObject.SetActive(false);
        id_.gameObject.SetActive(false);
        pw_.gameObject.SetActive(false);
        ConfirmPW_.gameObject.SetActive(false);
        login02_.gameObject.SetActive(false);
        createAccount_.gameObject.SetActive(false);


        ErrorMessageHide();

        //Input Field の入力文字数制限
        id_.characterLimit = MAX_WORD;
        pw_.characterLimit = MAX_WORD;

        // 接続開始
        ws.ConnectionStart(Receive);
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
        // シーンの変更
        ws.ChangeScene();


        //エラーコードチェック用
       // if(Input.GetKeyDown(KeyCode.Space))
       // {
       //     ErrorCheck(104);
       // }

    }
    //public関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //ボタンが押されたとき
    public void Click()
    {
        Debug.Log("クリックされた");
    }

    public void LogInClick()
    {
        LogInActive();
        Debug.Log("SignIn");
    }

    public void RegisterClick()
    {
        RegisterActive();
        Debug.Log("SignUp");
    }

    public void BackClick()
    {
        login01_.gameObject.SetActive(true);
        register_.gameObject.SetActive(true);
        back_.gameObject.SetActive(false);
        id_.gameObject.SetActive(false);
        id_.text = "";
        pw_.gameObject.SetActive(false);
        pw_.text = "";
        login02_.gameObject.SetActive(false);
        createAccount_.gameObject.SetActive(false);

        ErrorMessageHide();
        ConfirmPW_.gameObject.SetActive(false);
        ConfirmPW_.text = "";

        Debug.Log("Back");
    }

    public void LogInToGameClick()
    {
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;

        if (CheckString(id,true) == true && CheckString(pw,false) == true)
        {
            Debug.Log("ログイン ID:" + id + "  PW:" + pw);
            ErrorMessageHide();

            // ログイン処理
            ws.SendLogin(id, pw);
        }
        else
        {
            pw_.text = "";
        }
    }

    public void RegisterClick02()
    {
        ErrorMessageHide();

        string id = id_.text;
        string pw = pw_.text;
        if(CheckString(id,true)==true&&CheckString(pw,false)==true)
        {
            if(pw_.text==ConfirmPW_.text)
            {
                Debug.Log("新規登録 ID:" + id + "  PW:" + pw);
                //Error01.gameObject.SetActive(false);
                //Error02.gameObject.SetActive(false);
                //Error03.gameObject.SetActive(false);
                //Error04.gameObject.SetActive(false);
                // 新規登録
                ws.SendRegistration(id, pw);
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
        LogInToGameClick();
    }

    //private関数--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void LogInActive()
    {

        login01_.gameObject.SetActive(false);
        register_.gameObject.SetActive(false);
        back_.gameObject.SetActive(true);
        id_.gameObject.SetActive(true);
        pw_.gameObject.SetActive(true);
        login02_.gameObject.SetActive(true);
        
        RectTransform er01= Error01.GetComponent<RectTransform>();
        er01.localPosition = new Vector3(75, -280, 0);
        RectTransform er02 = Error02.GetComponent<RectTransform>();
        er02.localPosition = new Vector3(75, -280, 0);
    }

    private void RegisterActive()
    {
        login01_.gameObject.SetActive(false);
        register_.gameObject.SetActive(false);
        back_.gameObject.SetActive(true);
        id_.gameObject.SetActive(true);
        pw_.gameObject.SetActive(true);
        createAccount_.gameObject.SetActive(true);
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
            case (int)CommandData.CmdMissingConfirmation:
                //IDかPWが間違っている
                TypingError();
                break;
            case (int)CommandData.CmdExisting:
                //すでに使われているID
                AlreadyInUse();
                break;
            case (int)CommandData.CmdDuplicate:
                //すでにログインしているID
                MultipleLoginError();
                break;
            default:
                return true;
        }
        return false;
    }

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
