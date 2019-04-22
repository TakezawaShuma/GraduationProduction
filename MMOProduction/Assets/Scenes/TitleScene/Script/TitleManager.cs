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

        Error01.gameObject.SetActive(false);
        Error02.gameObject.SetActive(false);
        Error03.gameObject.SetActive(false);
        //Input Field の入力文字数制限
        id_.characterLimit = MAX_WORD;
        pw_.characterLimit = MAX_WORD;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Error01.gameObject.SetActive(false);
        Error02.gameObject.SetActive(false);
        Error03.gameObject.SetActive(false);
        ConfirmPW_.gameObject.SetActive(false);
        Debug.Log("Back");
    }

    public void LogInToGameClick()
    {
        Error01.gameObject.SetActive(false);
        Error02.gameObject.SetActive(false);
        Error03.gameObject.SetActive(false);
        string id = id_.text;
        string pw = pw_.text;

        if (CheckString(id,true) == true && CheckString(pw,false) == true)
        {
            Debug.Log("ログイン ID:" + id + "  PW:" + pw);
            Error01.gameObject.SetActive(false);
            Error02.gameObject.SetActive(false);
            Error03.gameObject.SetActive(false);

            gameObject.GetComponent<ConnectLogin>().Login(id, pw);
            // ------------------------------------------------------------------------
            // 
            //
            //                              ここにログイン処理
            //
            //
            //
            //  ------------------------------------------------------------------------
        }
        else
        {
            pw_.text = "";
        }



    }

    public void RegisterClick02()
    {
        Error01.gameObject.SetActive(false);
        Error02.gameObject.SetActive(false);
        Error03.gameObject.SetActive(false);
        string id = id_.text;
        string pw = pw_.text;
        if(CheckString(id,true)==true&&CheckString(pw,false)==true)
        {
            if(pw_.text==ConfirmPW_.text)
            {
                Debug.Log("新規登録 ID:" + id + "  PW:" + pw);
                Error01.gameObject.SetActive(false);
                Error02.gameObject.SetActive(false);
                Error03.gameObject.SetActive(false);


                // -------------------------------------------------------------------------
                //
                //                      新規登録処理
                // 
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
            //return Regex.IsMatch(str, @"^[0-9a-zA-Z]+$");
        }
    }
}
