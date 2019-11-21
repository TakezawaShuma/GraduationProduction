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
    private Toggle mute;
    [Header("BGM設定項目"), SerializeField]
    private Slider bgmVolume_;
    [Header("SE設定項目"), SerializeField]
    private Slider seVolume_;

    [Header("プレイヤー音"), SerializeField]
    private PlayerSound playerSound_;
    [Header("システム音"), SerializeField]
    private SystemSound systemSound_;

    [Header("BGM音"), SerializeField]
    private AudioSource backMusic;

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
}
