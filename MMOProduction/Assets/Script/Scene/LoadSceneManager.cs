using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{

    public string nextScene;
    public string unloadScene;

    // Start is called before the first frame update
    void Start()
    {
        LoadNextScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadNextScene()
    {
        switch (nextScene)
        {
            case "PlayScene":
                Debug.Log("Load Play");
                StartCoroutine(this.invokeActionOnloadScene(nextScene, () => { }));
                var play = FindObjectOfType<PlaySceneManager>() as PlaySceneManager;
                break;
            case "LoginScene":
                Debug.Log("Load Title");
                StartCoroutine(this.invokeActionOnloadScene(nextScene, () => { }));
                var title = FindObjectOfType<TitleSceneManager>() as TitleSceneManager;
                break;
            default:
                Debug.Log("Non Load");
                StartCoroutine(this.invokeActionOnloadScene(unloadScene, () => { }));
                break;
        }
    }

    private IEnumerator invokeActionOnloadScene(string sceneName, System.Action onLoad)
    {
        Debug.Log(sceneName);
        var asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return asyncOp;
        onLoad.Invoke();
        SceneManager.UnloadSceneAsync(unloadScene);
        Debug.Log("Unload : " + unloadScene);
    }

}
