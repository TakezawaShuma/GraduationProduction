using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogin : MonoBehaviour
{


    Connect.ConnectLogin connect_login = new Connect.ConnectLogin();
    // Start is called before the first frame update
    void Start()
    {
        // 通信関係の初期処理
        connect_login.ConnectionStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("s"))
        {
            connect_login.SendLogin("tsit", "trident");
        }
        if (Input.GetKeyDown("n"))
        {
            connect_login.SendRegistration("haroharo", "trident");
        }
        connect_login.ChangeScene();

        if (Input.GetKeyDown("r")) { OnDestroy(); Start(); }
    }

    private void OnDestroy()
    {
        connect_login.Destroy();
    }
}

