using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneManagerBase : MonoBehaviour
{
    
    public bool connectFlag = false;

    /// <summary>
    /// .exeの終了関数
    /// </summary>
    protected void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    /// <summary>
    /// 再度要請する
    /// </summary>
    /// <param name="_json"></param>
    /// <returns></returns>
    public IEnumerator ReSend(WS.WsBase _ws,　string _json)
    {
        yield return new WaitForSeconds(0.5f);
        _ws.Send(_json);
    }
}
