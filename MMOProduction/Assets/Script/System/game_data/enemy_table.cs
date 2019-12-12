using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game_table/enemy", fileName = "enemy_data")]
public class enemy_table : ScriptableObject
{
    // データ一覧
    [SerializeField]
    public List<enemy_data> tables;

    [System.SerializableAttribute]
    public class enemy_data
    {
        //エネミーID
        public int id;

        //モデルID
        public int modelId;

        // 最大HP
        public int maxHp;
    }
}