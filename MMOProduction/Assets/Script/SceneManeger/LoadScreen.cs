//
// LoadScreen.cs
// 
// Author: Tama
//

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// ---------------------------------------------
// ロード画面で使用
// ---------------------------------------------
public class LoadScreen : SceneManagerBase
{

    [SerializeField]
    private GameObject _loadingUI = null;       // ロード中に表示するUI画面

    [SerializeField]
    private Slider _loadingSlider = null;       // 読み込み率を表示するスライダ

    [SerializeField]
    private Text _progressUI = null;            // 読み込み具合を表示するUI

    [SerializeField]
    private Image _loadingInfoWindow = null;    // ロード中に表示するインフォ画面

    //[SerializeField]
    //private Image _loadingCharacter = null;     // ロード中のキャラクター画像  

    private float _progress;    // 読み込みの進み具合     
       
    private ColumnWindow _columnWindow;    // ロード中に表示するコラム画面

    private AsyncOperation async;

    private Ready ready;


    private void Start()
    {
        ready = Ready.Instance;

        //_loadingUI.SetActive(false);

        _progress = 0f;

        _columnWindow = new ColumnWindow("Images/ColumnWindow");
        // 標準ウィンドウ幅 width:500 height:300

        StartCoroutine("LoadScene");
    }

    private void Update()
    {
        // 画面クリックでコラムを変更
        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY ||
            InputManager.InputMouseCheckDown(0) == INPUT_MODE.NONE ||
            InputManager.InputMouseCheckDown(0) == INPUT_MODE.UI)
        {
            ChangeColumn();
        }

        _progress = Mathf.Clamp(_progress, 0.0f, 1.0f);
        _loadingSlider.value = _progress;
        _progressUI.text = Mathf.Floor(_progress * 100.0f).ToString() + "%";

#if DEBUG 
        if (InputManager.InputKeyCheck(KeyCode.Space))
        {
            _progress += 0.01f;
        }
#endif
    }

    // --------------------------------------
    // ロード画面を表示させる
    // --------------------------------------
    public void OnLoad()
    {
        _loadingInfoWindow.sprite = _columnWindow.GetRandomImage();
        
        _loadingUI.SetActive(true);
    }

    // --------------------------------------
    // ロード画面を閉じる
    // --------------------------------------
    public void ExitLoad()
    {
        _loadingUI.SetActive(false);
    }

    // --------------------------------------
    // 読み込みの進歩状況を設定する
    // progress: 数値
    // --------------------------------------
    public void SetProgress(float progress)
    {
        _progress = progress;
    }

    // --------------------------------------
    // コラム画像を変更する
    // --------------------------------------
    private void ChangeColumn()
    {
        _loadingInfoWindow.sprite = _columnWindow.GetRandomImage();
    }

    private IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Additive);
        while (!async.isDone)
        {
            SetProgress(Mathf.Clamp01(async.progress / 0.9f));

            yield return null;
        }

        while(!ready.CheckReady())
        {
            yield return null;
        }

        Debug.Log("このシーンを消す");

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
