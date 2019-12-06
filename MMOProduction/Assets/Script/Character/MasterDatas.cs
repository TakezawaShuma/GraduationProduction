using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDatas : MonoBehaviour
{
    // モデルデータ
    [SerializeField]
    private ModelTable modelTables_ = null;
    // スキルデータ
    // 

    /// <summary>
    /// ID検索
    /// </summary>
    public GameObject FindById(int _id) {
        foreach(var table in modelTables_.tables_) {
            if (table.id_ == _id) return table.modelPre_;
        }
        return null;
    }
}
