using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


public class TitleManager : MonoBehaviour
{
    static Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

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

    [SerializeField]
    private Text Error;


    //ログインID入力用
    [SerializeField]
    private InputField id_;
    //ログインPW入力用
    [SerializeField]
    private InputField pw_;
    // Start is called before the first frame update
    void Start()
    {
        login01_.gameObject.SetActive(true);
        register_.gameObject.SetActive(true);
        back_.gameObject.SetActive(false);
        id_.gameObject.SetActive(false);
        pw_.gameObject.SetActive(false);
        login02_.gameObject.SetActive(false);
        createAccount_.gameObject.SetActive(false);

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
        Error.gameObject.SetActive(false);
        Debug.Log("Back");
    }

    public void LogInToGameClick()
    {
        string id = id_.text;
        string pw = pw_.text;

        if (CheckString(id) == true && CheckString(pw) == true)
        {
            Debug.Log("ログイン ID:" + id + "  PW:" + pw);
            Error.gameObject.SetActive(false);
        }
        else
        {
            Error.gameObject.SetActive(true);
            pw_.text = "";
        }

//        bool result01 = id.IndexOf(" ") >= 0;
//        bool result02 = pw.IndexOf(" ") >= 0;
//
//        if(result01==false&&result02==false)
//        {
//            if(id==""||pw=="")
//            {
//                Error.gameObject.SetActive(true);
//            }
//            else
//            {
//                Debug.Log("ログイン ID:" + id + "  PW:" + pw);
//                Error.gameObject.SetActive(false);
//            }
//        }
//        else
//        {
//            Error.gameObject.SetActive(true);
//        }
    }

    public void RegisterClick02()
    {
        
        string id = id_.text;
        string pw = pw_.text;
        if(CheckString(id)==true&&CheckString(pw)==true)
        {
             Debug.Log("新規登録 ID:" + id + "  PW:" + pw);
             Error.gameObject.SetActive(false);
        }
        else
        {
            Error.gameObject.SetActive(true);
            pw_.text = "";
        }
        //        bool result01 = id.IndexOf(" ") >= 0;
        //        bool result02 = pw.IndexOf(" ") >= 0;
        //        if (result01 == false && result02 == false)
        //        {
        //            if (id == "" || pw == "")
        //            {
        //                Error.gameObject.SetActive(true);
        //            }
        //            else
        //            {

        //            }
        //
        //        }
        //        else
        //        {
        //            Error.gameObject.SetActive(true);
        //        }

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
    }

    private void RegisterActive()
    {
        login01_.gameObject.SetActive(false);
        register_.gameObject.SetActive(false);
        back_.gameObject.SetActive(true);
        id_.gameObject.SetActive(true);
        pw_.gameObject.SetActive(true);
        createAccount_.gameObject.SetActive(true);
    }

    //string内にスペースや全角文字がないかをしらべる
    private bool CheckString(string str)
    {
        bool result01 = str.IndexOf(" ") >= 0;
        bool result02 = isZenkaku(str); // 出力：False

        if (result01 != false)
            return false;
        if (str == "")
            return false;
        //全角文字がないかのチェック
        if (isZenkaku(str) == false && isHankaku(str) == true)
            return true;

        return false;
    }

    public static bool isZenkaku(string str)
    {
        int num = sjisEnc.GetByteCount(str);
        return num == str.Length * 2;
    }

    public static bool isHankaku(string str)
    {
        int num = sjisEnc.GetByteCount(str);
        return num == str.Length;
    }
}
