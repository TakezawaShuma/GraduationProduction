using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザーの設定用
/// </summary>
public class GameUserSetting : MonoBehaviour
{
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
    }

    public void LogoutClick()
    {
        WS.WsPlay.Instance.Logout();
    }
}
