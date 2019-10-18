using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキルの種類
/// </summary>
public enum skill_type
{
    ATTACK,
    BUFF,
    NUME
}

/// <summary>
/// バフの種類
/// </summary>
public enum buff_type
{
    DEFFENSE,
    ATTACK,
    HEEL,
    NOME
}

/// <summary>
/// ターゲットの種類
/// </summary>
public enum target_type
{
    PLAYER,
    PARTY,
    TARGET,
    RANGE,
}


[CreateAssetMenu(menuName = "game_table/skill", fileName = "skill_data")]
public class skill_table : ScriptableObject
{
    // データ一覧
    [SerializeField]
    public List<skill_data> tables;

    [System.SerializableAttribute]
    public class skill_data
    {
        // ID
        [SerializeField]
        public int id;

        // 名前
        [SerializeField]
        public string name;

        // 最大Lv
        [SerializeField]
        public int max_point;

        // 効果
        [SerializeField]
        public int effect;

        // 説明文
        [SerializeField]
        public string description;

        // スキルの種類
        [SerializeField]
        public skill_type skillType;

        // バフの種類
        [SerializeField]
        public buff_type buffType;

        // ターゲットの種類
        [SerializeField]
        public target_type targetType;
    }
}