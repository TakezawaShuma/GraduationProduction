//
// PossessWeponList.cs
//
// Author : Tama
//
// プレイヤーの武器情報
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWepon : MonoBehaviour
{
    [Serializable]
    public class WeponData
    {
        // 攻撃判定用コライダがアタッチされているオブジェクトが対象
        public AttackCollider _wepon;
        public string _name;
    }

    // 武器データのリスト
    [SerializeField]
    private List<WeponData> _weponList = new List<WeponData>();
    public List<WeponData> WeponList { get { return _weponList; } }


    private void Start()
    {
        
    }

    public WeponData FindWeponFromName(string name)
    {
        foreach(var tmp in _weponList)
        {
            if(tmp._name == name)
            {
                return tmp;
            }
        }
        return null;
    }
}
