//
// SkillBase.cs
//
// Author : Tama
//
// 全スキルの基底クラス
// 

using UnityEngine;

public class SkillBase : MonoBehaviour
{

    protected GameObject _owner;
    public GameObject Owner { get { return _owner; } set { _owner = value; } }

    private string _tag;
    public string Tag { get { return _tag; } set { _tag = value; } }

    private SkillID _id;
    public SkillID ID { get { return _id; } set { _id = value; } }
}
