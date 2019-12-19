using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSocket : SceneManagerBase
{
    WS.WsPlay wsPlay_;
    WS.WsLogin wsLogin_;

    List<int> userIds_ = new List<int>();

    void Start() {
        wsLogin_ = WS.WsLogin.Instance;
        wsPlay_  = WS.WsPlay.Instance;

        wsLogin_.loginAction = LoginAction;

        
        wsPlay_.loadSaveAction = null;
    }

    void LoginAction(Packes.LoginOK _data) {
        userIds_.Add(_data.user_id);
        Debug.Log(_data.user_id);
    }

    public void Login() {
        StartCoroutine(LoginCoroutine());
    }
    public void SaveLoad() {
        StartCoroutine(SaveLoadCtoSCoroutine());
    }
    public void LoadingOk() {
        StartCoroutine(LoadingOKCoroutine());
    }
    public void Logout() {
        StartCoroutine(LogoutCoroutine());
    }
    public void Position() {
        StartCoroutine(PositionCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        for (int i = 1; i < 100; i++)
        {
            wsLogin_.Send(new Packes.LoginUser("test" + i.ToString(), "test").ToJson());
            yield return new WaitForSeconds(0.1f);
        }  
    }

    IEnumerator LogoutCoroutine() {
        foreach (var id in userIds_)
        {
            wsPlay_.Send(new Packes.LogoutCtoS(id).ToJson());
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator PositionCoroutine() {
        foreach (var id in userIds_)
        {
            wsPlay_.Send(new Packes.TranslationCtoS(id, -210,0,-210, 1).ToJson());
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator LoadingOKCoroutine() {
        foreach (var id in userIds_)
        {
            wsPlay_.Send(new Packes.LoadingOK(id).ToJson());
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator SaveLoadCtoSCoroutine() {
        foreach (var id in userIds_)
        {
            wsPlay_.Send(new Packes.SaveLoadCtoS(id).ToJson());
            yield return new WaitForSeconds(0.1f);
        }
    }
}
