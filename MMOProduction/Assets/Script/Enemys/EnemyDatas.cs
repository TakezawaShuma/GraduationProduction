using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager
{
    List<EnemyData> el = new List<EnemyData>();
    public void AddEnemy(int id,string modelName)
    {
        
        EnemyData ed = new EnemyData();
        ed.id_ = id;
        Debug.Log("AddEnemy ID:" + id);
        //switch (id)
        //{
        //    case 0:
        //        ed.prefab_ = (GameObject)Resources.Load("Prefabs/Forest Bunny Black");
        //        break;
        //    default:
        //        ed.prefab_ = (GameObject)Resources.Load("Prefabs/Forest Bunny Black");
        //        break;
        //}
        ed.prefab_ = (GameObject)Resources.Load("Prefabs/"+modelName);
        el.Add(ed);
    }

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



//    public GameObject EnemyModel(int id_)
//    {
//        
//    }

    //int master;
    //int model;
    //int  maxHp;
}

public class EnemyData
{
    public int id_;
    public GameObject prefab_;
}
