using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game_table/character", fileName = "models_data")]
public class character_table : ScriptableObject
{
    [SerializeField]
    public List<model_data> tables;

    [System.SerializableAttribute]
    public class model_data
    {
        // モデルのID
        public int modelId;

        // モデルのプレファブ
        public GameObject model;
        // 消してください
    }

    public GameObject FindModel(int _id)
    {
        GameObject ret = null;
        foreach (var tmp in tables) { if (tmp.modelId == _id) { ret = tmp.model; } }
        return ret;
    }
}