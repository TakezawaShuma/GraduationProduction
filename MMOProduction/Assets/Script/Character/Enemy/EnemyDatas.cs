using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager
{
    //EnemyData List
    List<EnemyData> el = new List<EnemyData>();

    //Enemyの登録 id =ID modelName = Prefab名
    public void AddEnemy(int id,string modelName)
    {
        EnemyData ed = new EnemyData();
        ed.id_ = id;
        Debug.Log("AddEnemy ID:" + id);
        ed.prefab_ = (GameObject)Resources.Load("Prefabs/"+modelName);
        el.Add(ed);
    }

    //指定されたIDのEnemyのPrefabを返す
    public GameObject GetEnemyPrefab(int id)
    {
        foreach(var v in el)
        {
            if (v.id_ != id)
                continue;
            Debug.Log("ID:" + id + "のPrefabを取得");
            return v.prefab_;
        }
        Debug.Log("存在しないID");
        return null;
    }
}

public class EnemyData
{
    //Enemy ID
    public int id_;
    //Enemy Prefab
    public GameObject prefab_;
}
