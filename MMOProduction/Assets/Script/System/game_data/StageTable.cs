using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "game_table/map", fileName = "mapdata")]
public class StageTable : ScriptableObject
{
    [SerializeField]
    List<PrefabMapData> tables = new List<PrefabMapData>();

    [System.SerializableAttribute]
    public class PrefabMapData {
        public MapID id;
        public GameObject prefab;
    }

    public GameObject FindPrefab(MapID _id) { 
        foreach(var table in tables) {
            if (_id == table.id) return table.prefab;
        }
        return null;
    }
}
