using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザーの設定用
/// </summary>
public class GameUserSetting : MonoBehaviour
{
    public struct config_data {
        public bool mute;
        public float bgmVolume;
        public float seVolume;
    }

    [Header("ミュート設定項目"), SerializeField]
    private Toggle mute = null;
    [Header("BGM設定項目"), SerializeField]
    private Slider bgmVolume_ = null;
    [Header("SE設定項目"), SerializeField]
    private Slider seVolume_ = null;

    [Header("プレイヤー音"), SerializeField]
    private PlayerSound playerSound_ = null;
    [Header("システム音"), SerializeField]
    private SystemSound systemSound_ = null;

    [Header("BGM音"), SerializeField]
    private AudioSource backMusic = null;

    private void Start()
    {
        //SaveConfig();
        LoadConfig();
    }

    public void Init(GameObject _player)
    {
        playerSound_ = _player.GetComponent<PlayerSound>();
    }

    void Update() {
        if (!mute.isOn) SettingVolume();
        
        playerSound_.Mute(mute.isOn);
        systemSound_.Mute(mute.isOn);

        backMusic.mute = mute.isOn;
    }

    /// <summary>
    /// ボリュームの設定
    /// </summary>
    private void SettingVolume() {
        playerSound_.SettingVolume(seVolume_.value);
        systemSound_.SettingVolume(seVolume_.value);

        backMusic.volume = bgmVolume_.value;
        SaveConfig();
    }

    public void LogoutClick()
    {
        WS.WsPlay.Instance.Logout();
    }

    private void LoadConfig() {
        var dataJson = InputFile.ReadFile(MasterFileNameList.config, FILETYPE.JSON);
        Debug.Log(dataJson);

        if (string.IsNullOrEmpty(dataJson)) return;
        config_data data    = JsonUtility.FromJson<config_data>(dataJson);
        bgmVolume_.value    = data.bgmVolume;
        seVolume_.value     = data.seVolume;
        mute.isOn           = data.mute;
    }

    private void SaveConfig() {
        config_data data;
        data.bgmVolume  = bgmVolume_.value;
        data.seVolume   = seVolume_.value;
        data.mute       = mute.isOn;
        InputFile.WriterJson(MasterFileNameList.config, JsonUtility.ToJson(data));
    }
}
