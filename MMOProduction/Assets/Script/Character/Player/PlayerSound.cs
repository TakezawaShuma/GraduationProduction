using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    private SoundTable walkSoundTable_;


    // -------- 攻撃 --------
    [Header("斬撃"), SerializeField]
    private AudioClip slashing_ = null;
    [Header("打撃"), SerializeField]
    private AudioClip blow_ = null;
    [Header("炎魔法"), SerializeField]
    private AudioClip fireBall_ = null;
    [Header("雷魔法"), SerializeField]
    private AudioClip thunder_ = null;
    [Header("水魔法"), SerializeField]
    private AudioClip water_ = null;

    // -------- その他 --------
    [Header("被弾"), SerializeField]
    private AudioClip damage_ = null;
    [Header("死亡"), SerializeField]
    private AudioClip die_ = null;

    // オーディオソース
    private AudioSource audioSource_ = null;
    //オーディオミキサー
    public AudioMixer audioMixer_ = null;
    // 歩く音判定用
    private Dictionary<string, SoundTable.WalkSoundSetting> walkSounds_ = new Dictionary<string, SoundTable.WalkSoundSetting>();
    // スキルの音
    private Dictionary<int, AudioClip> skillSounds_ = new Dictionary<int, AudioClip>();

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        audioSource_ = GetComponent<AudioSource>();
        //audioSource_ = new AudioSource();
        // 歩く初期化
        InitWalk();
        // スキルの初期化
        InitSkill();
    }

    /// <summary>
    /// スキルの初期化
    /// </summary>
    private void InitSkill()
    {
        skillSounds_[1] = slashing_;
        skillSounds_[2] = blow_;
        skillSounds_[3] = fireBall_;
        skillSounds_[4] = thunder_;
        skillSounds_[5] = water_;
    }

    /// <summary>
    /// 歩く初期化
    /// </summary>
    private void InitWalk()
    {
        walkSounds_["Grassland"] = walkSoundTable_.grasslandSetting_;
        walkSounds_["SandRoad"] = walkSoundTable_.sandRoadSetting_;
        walkSounds_["RockyPlace"] = walkSoundTable_.rockyPlaceSetting_;
        walkSounds_["SandyBeach"] = walkSoundTable_.sandyBeachSetting_;
    }


    public void RunPlay(string _tag)
    {
        if (!walkSounds_.ContainsKey(_tag)) return;
        if (!audioSource_.isPlaying)
        {
            audioSource_.pitch = walkSounds_[_tag].runPitch.tempo;
            audioMixer_.SetFloat(_tag, walkSounds_[_tag].runPitch.pitch);
            audioSource_.PlayOneShot(walkSounds_[_tag].clip);
        }
    }


    /// <summary>
    /// 歩く音再生
    /// </summary>
    public void WalkPlay(string _tag)
    {
        if (!walkSounds_.ContainsKey(_tag)) return;
        if (!audioSource_.isPlaying)
        {

            audioSource_.pitch = walkSounds_[_tag].walkPitch.tempo;
            audioMixer_.SetFloat(_tag, walkSounds_[_tag].walkPitch.pitch);
            audioSource_.PlayOneShot(walkSounds_[_tag].clip);
        }
    }

    /// <summary>
    /// スキルの音再生
    /// </summary>
    public void SkillPlay(int _id)
    {
        if (!skillSounds_.ContainsKey(_id)) return;
        if (!audioSource_.isPlaying && skillSounds_[_id] != null) audioSource_.PlayOneShot(skillSounds_[_id]);
    }

    /// <summary>
    /// 被弾時
    /// </summary>
    public void Damage()
    {
        if (audioSource_.isPlaying) audioSource_.PlayOneShot(damage_);
    }

    /// <summary>
    /// 死亡時
    /// </summary>
    public void Die()
    {
        if (audioSource_.isPlaying) audioSource_.PlayOneShot(die_);
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    public void SettingVolume(float _val)
    {
        audioSource_.volume = _val;
    }

    /// <summary>
    /// ミュート
    /// </summary>
    public void Mute(bool _state)
    {
        audioSource_.mute = _state;
    }
}