using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayer :CharacterBase
{

    protected Vector3 lastPos = new Vector3();
    protected Vector3 nextPos = new Vector3();
    protected Quaternion lastDir = new Quaternion();
    protected Quaternion nextDir = new Quaternion();


    protected float nowFlame = 0;
    public const float UPDATE_SPEED = 1.0f / 3.0f;

    /// <summary>
    /// キャラクターの初期化設定
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    public void Init(float _x, float _y, float _z, float _dir,int _id)
    {
        ID = _id;
        lastPos = transform.position = new Vector3(_x, _y, _z);
        nextPos = new Vector3(_x, _y, _z);
        lastDir = transform.rotation = Quaternion.Euler(new Vector3(0, _dir, 0));
        nextDir = Quaternion.Euler(new Vector3(0, _dir, 0));
        animator_ = GetComponent<Animator>();
    }

    /// <summary>
    /// 位置情報を更新する
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
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

    /// <summary>
    /// ステータス情報を更新する
    /// </summary>
    /// <param name="_hp"></param>
    /// <param name="_mp"></param>
    /// <param name="_status"></param>
    public void UpdateStatusData(int _hp, int _mp, int _status)
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

}
