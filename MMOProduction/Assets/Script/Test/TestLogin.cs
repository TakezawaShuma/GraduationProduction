using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogin : MonoBehaviour
{


    WS.WsLogin ws_login = new WS.WsLogin();
    // Start is called before the first frame update
    void Start()
    {
        // 通信関係の初期処理
        ws_login.ConnectionStart(Receive);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("s"))
        {
            ws_login.SendLogin("haroharo", "trident");
        }
        if (Input.GetKeyDown("n"))
        {
            ws_login.SendRegistration("haroharo", "trident");
        }
        //ws_login.ChangeScene();

        if (Input.GetKeyDown("r")) { Start(); }
    }

    void Receive(int cmd)
    {
        switch(cmd)
        {
            case 103:
                Debug.Log("受信コマンド：" + cmd + "　ログイン成功");
                break;
            case 104:
                Debug.Log("受信コマンド：" + cmd + "　ログイン失敗");
                break;
            case 105:
                Debug.Log("受信コマンド：" + cmd + "　新規作成成功");
                break;
            case 106:
                Debug.Log("受信コマンド：" + cmd + "　新規作成失敗");
                break;
            case 901:
                Debug.Log("受信コマンド：" + cmd + "　重複ログイン");
                break;
            default:
                break;
        }
    }
}

