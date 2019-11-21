using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// システム音の種類
/// </summary>
public enum SYSTEM_SOUND_TYPE{
    OPEN_MENU,
    CLOSE_MENU,
    ENTER,
    LOGIN,

    NONE
}

/// <summary>
/// システム音
/// </summary>
public class SystemSound : MonoBehaviour
{
    [Header("メニューを開く"), SerializeField]
    private AudioClip menuOpen_ = null;
    [Header("メニューを閉じる"), SerializeField]
    private AudioClip meunClose_ = null;
    [Header("決定"), SerializeField]
    private AudioClip enter_ = null;

    [Header("ログイン"), SerializeField]
    private AudioClip login_ = null;

    // オーディオ
    private AudioSource source_ = null;
    // サウンド
    private Dictionary<SYSTEM_SOUND_TYPE, AudioClip> sounds_ = new Dictionary<SYSTEM_SOUND_TYPE, AudioClip>();

    /// <summary>
    /// 音の初期化
    /// </summary>
    private void Init() {
        sounds_[SYSTEM_SOUND_TYPE.OPEN_MENU] = menuOpen_;
        sounds_[SYSTEM_SOUND_TYPE.CLOSE_MENU] = meunClose_;
        sounds_[SYSTEM_SOUND_TYPE.ENTER] = enter_;
        sounds_[SYSTEM_SOUND_TYPE.LOGIN] = login_;
    }

    private void Start() {
        source_ = GetComponent<AudioSource>();

        Init();
    }

    /// <summary>
    /// システム音
    /// </summary>
    /// <param name="_type"></param>
    public void SystemPlay(SYSTEM_SOUND_TYPE _type) {
        if (!sounds_.ContainsKey(_type)) return;
        if(!source_.isPlaying) source_.PlayOneShot(sounds_[_type]);
    }
}
