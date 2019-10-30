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

    public void AddChild()
    {
        Debug.Log("スキルの子スキルリスト作成");
        foreach (var v in tables)
        {
            foreach (var vv in tables)
            {
                if (v.pearentID == vv.id)
                {
                    if(v.pearentID!=0)
                    {
                        vv.childsList.Add(v);
                    }
                }
            }

        }

    }

    [System.SerializableAttribute]
    public class skill_data
    {
        //スキルID
        public int id;

        //アイコンID
        public int Icon;

        //エフェクトID
        public int effectId;

        //名前
        public string name;

        //最大レベル
        public int maxLv;

        //現在レベル
        public int currentLv;

        //説明文
        public string detail;

        //リキャスト時間
        public int cast;

        //現在のリキャスト時間
        public int currentCast;

        //消費コスト
        public int cost;

        //時間
        public int time;

        //ヘイト値
        public int hate;

        //親のID
        public int pearentID;

        //子のIDリスト
        public List<skill_data> childsList;

        //必要ポイント
        public int pearentPoint;

        //武器
        public int weapon;

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
}