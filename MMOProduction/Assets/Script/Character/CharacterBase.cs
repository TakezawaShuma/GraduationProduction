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

    protected float nowFlame = 0;

    protected int id;   // キャラクターごとのID

    protected Vector3 lastPos = new Vector3();
    protected Vector3 nextPos = new Vector3();
    protected Quaternion lastDir = new Quaternion();
    protected Quaternion nextDir = new Quaternion();

    public const float UPDATE_SPEED = 1.0f / 3.0f;

    /// <summary>
    /// キャラクターの初期化設定
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    public void Init(float _x, float _y, float _z, float _dir)
    {
        lastPos = transform.position = new Vector3(_x, _y, _z);
        nextPos = new Vector3(_x, _y, _z);
        lastDir = transform.rotation = Quaternion.Euler(new Vector3(0, _dir, 0));
        nextDir = Quaternion.Euler(new Vector3(0, _dir, 0));
    }


    public void UpdatePostionData(float _x, float _y, float _z, float _dir)
    {
        // 向きを決める
        lastDir = transform.rotation;
        nextDir = Quaternion.Euler(0, _dir, 0);

        // 位置を決める
        lastPos = transform.position;
        nextPos = new Vector3(_x, _y, _z);

        // カウントを初期化
        nowFlame = 0;
    }

    public void UpdateStatusData(int _hp,  int _mp, int _status)
    {
        hp = _hp;
        mp = _mp;
        status = _status;
    }

    /// <summary>
    /// キャラクターの移動を補間する
    /// </summary>
    protected void LerpMove()
    {
        nowFlame += UPDATE_SPEED;
        transform.rotation = Quaternion.Lerp(lastDir, nextDir, nowFlame);
        transform.position = Vector3.Lerp(lastPos, nextPos, nowFlame);
    }

    /// <summary>
    /// キャラクターごとのID
    /// </summary>
    public virtual int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }
}
