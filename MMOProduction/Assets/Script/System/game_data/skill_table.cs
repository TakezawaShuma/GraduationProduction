using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum range_type
{
    PLAYER,
    TARGET,
    PARTY,
    RANGE,
}

public enum target_type
{
    PLAYER,
    ENEMY,
    NONE,
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
        //スキルID
        public int id;

        //アイコンID
        public int Icon;

        //エフェクトID
        public int effectId;

        //スキル名
        public string name;

        //最大レベル
        public int maxLv;

        //現在レベル
        public int currentLv;

        //説明文
        public string detail;

        // 使用者
        public string user;

        // アニメーション
        public string animation;

        //リキャスト時間
        public int cast;

        //現在のリキャスト時間
        public int currentCast;

        //消費コスト
        public int cost;

        //効果時間
        public int time;

        //ヘイト値
        public int hate;

        //親のID
        public int pearentID;

        //必要ポイント
        public int pearentPoint;

        //武器
        public int weapon;

        //効果範囲(武器攻撃ならプレイヤーから/魔法系なら着弾位置から)
        public int range;

        // ステータスに及ぼす効果
        public int hp;
        public int mp;
        public int str;
        public int vit;
        public int inte;
        public int mnd;
        public int dex;
        public int agi;
        public int effect;
        public int condition;

        // 識別用
        public range_type rangeType;

        public target_type targetType;
    }

    public skill_data FindSkill(int _skill_id)
    {
        skill_data ret = null;
        foreach(var skill in tables)
        {
            if (skill.id == _skill_id) { ret = skill; break; }
        }
        return ret;
    }
}