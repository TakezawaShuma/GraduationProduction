using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキルの種類
/// </summary>
public enum skill_type {
    ATTACK,
    BUFF,
    NUME
}

/// <summary>
/// バフの種類
/// </summary>
public enum buff_type {
    DEFFENSE,
    ATTACK,
    HEEL,
    NOME
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
        [SerializeField]
        // ID
        public int id;
        [SerializeField]
        // 最大Lv
        public int max_point;
        [SerializeField]
        // 効果
        public int effect;
        [SerializeField]
        // 説明文
        public string description;
        // スキルの種類
        [SerializeField]
        public skill_type skillType;
        [SerializeField]
        public buff_type buffType;
    }
}