using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game_table/enemy", fileName = "enemy_data")]
public class enemy_table : ScriptableObject
{
    // データ一覧
    [SerializeField]
    public List<enemy_data> tables;

    [Header("敵キャラクターモデル"), SerializeField]
    private character_table models;

    [System.SerializableAttribute]
    public class enemy_data
    {
        //エネミーの種別ID
        public int id;

        //モデルID
        public int modelId;

        // 最大HP
        public int maxHp;
    }

    /// <summary>
    /// マスターIDからモデルを引っ張ってくる
    /// </summary>
    /// <param name="_masterId"></param>
    /// <returns>エネミーモデル</returns>
    public GameObject FindModel(int _masterId)
    {
        GameObject ret = null;
        int modelId = FindModelsFromMaster(_masterId);
        if (modelId != 0) { ret = models.FindModel(modelId); }
        return ret;
    }

    /// <summary>
    /// マスターIDからモデルIDを引っ張ってくる
    /// </summary>
    /// <param name="_mastar"></param>
    /// <returns>モデルID</returns>
    private int FindModelsFromMaster(int _mastar)
    {
        int ret = 0;
        foreach(var data in tables)
        {
            if (data.id == _mastar) { ret = data.modelId; break; }
        }
        return ret;
    }
}