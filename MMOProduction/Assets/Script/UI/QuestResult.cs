﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestResult : MonoBehaviour
{

    [SerializeField]
    Sprite questCrear = null;
    [SerializeField]
    Sprite questFailed = null;

    [SerializeField]
    Image resultImage = null;

    [SerializeField]
    Image crearText = null;
    [SerializeField]
    Slider timerSlider = null;


    // 計測開始時間
    float stertTime = 0;
    // 計測開始からの経過時間
    float elapsedTime = 0;

    // 失敗：-1 / 何もなし：0 / 成功：1
    int isResult = 0;

    PlaySceneManager scene = null;

    
    // Start is called before the first frame update
    void Start()
    {
        resultImage.gameObject.SetActive(false);
        crearText.gameObject.SetActive(false);
        timerSlider.gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isResult != 0)
        {
            // 現在時刻との差から経過時間を算出
            elapsedTime = Time.time - this.stertTime;
        }

        if (isResult == 1)
        {
            if (resultDisplayFlag)
            {
                if (scene != null) SceneSwitch();
            }
            else
            {
                UpdateSlider();
            }

        }
        else if (isResult == -1)
        {
            if (scene != null) SceneSwitch();
        }

    }


    public void SetQuestCrear(float _time)
    {
        isResult = 1;
        stertTime = _time;
        // 有効化
        timerSlider.gameObject.SetActive(true);
        crearText.gameObject.SetActive(true);
        // その他設定
        resultImage.sprite = questCrear;
        timerSlider.value = 0;
    }

    public void SetQuestFailed(float _time)
    {
        isResult = -1;
        stertTime = _time;
        resultImage.sprite = questFailed;
    }

    bool resultDisplayFlag = false;
    /// <summary>
    /// スライダーを更新する
    /// </summary>
    /// <returns></returns>
    private bool UpdateSlider()
    {
        if (timerSlider.maxValue < elapsedTime)
        {
            timerSlider.gameObject.SetActive(false);
            crearText.gameObject.SetActive(false);
            resultImage.gameObject.SetActive(true);

            stertTime = Time.time;
            resultDisplayFlag = true;
            return true;
        }
        timerSlider.value = elapsedTime;
        return false;
    }

    public void SetScenes(PlaySceneManager _base)
    {
        scene = _base;
    }

    private void SceneSwitch()
    {
        if (elapsedTime >= 3.0f)
        {
            UserRecord.MapID = MapID.Base;
            scene.SendMoveMap(UserRecord.MapID);
            Debug.Log("ベースに移動");
        }
    }
}