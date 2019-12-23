using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "game_table/player_sound", fileName = "player_sound")]
public class SoundTable: ScriptableObject
{
    // -------- 足音 --------
    [Header("Grassland")]
    public WalkSoundSetting grasslandSetting_;
    [Header("SandRoad")]
    public WalkSoundSetting sandRoadSetting_;
    [Header("RockyPlace")]
    public WalkSoundSetting rockyPlaceSetting_;
    [Header("SandyBeach")]
    public WalkSoundSetting sandyBeachSetting_;

   

    [System.Serializable]
    public struct WalkSoundSetting
    {
        [Header("歩く"), SerializeField]
        public WalkPitch walkPitch;
        [Header("走る"), SerializeField]
        public RunPitch runPitch;
        [Header("音源"), SerializeField]
        public AudioClip clip;
    }

    [System.Serializable]
    public struct WalkPitch
    {
        [SerializeField]
        public float tempo;
        [SerializeField]
        public float pitch;
    }

    [System.Serializable]
    public struct RunPitch
    {
        [SerializeField]
        public float tempo;
        [SerializeField]
        public float pitch;
    }

}
