using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected int maxHp;
    protected int maxMp;
    protected int hp = 0;
    protected int mp = 0;
    protected int status = 0;
    protected string characterName = "";
    public string Name { get { return characterName; } set { characterName = value; } }

    protected int id = 0;   // キャラクターごとのID

    protected Animator animator_;


    /// <summary>
    /// キャラクターの初期化設定
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    //public void Init(float _x, float _y, float _z, float _dir)
    //{
    //    lastPos = transform.position = new Vector3(_x, _y, _z);
    //    nextPos = new Vector3(_x, _y, _z);
    //    lastDir = transform.rotation = Quaternion.Euler(new Vector3(0, _dir, 0));
    //    nextDir = Quaternion.Euler(new Vector3(0, _dir, 0));
    //    animator_ = GetComponent<Animator>();
    //}


    /// <summary>
    /// キャラクターごとのID
    /// </summary>
    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }
}
