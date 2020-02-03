using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayer : CharacterBase
{

    protected Vector3 lastPos = new Vector3();
    protected Vector3 nextPos = new Vector3();
    protected Quaternion lastDir = new Quaternion();
    protected Quaternion nextDir = new Quaternion();


    // スキル一覧
    protected skill_table skillTable;

    protected float nowFlame = 0;
    public const float UPDATE_SPEED = 1.0f / 9.0f;

    /// <summary>
    /// キャラクターの初期化設定
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    public void Init(float _x, float _y, float _z, float _dir, int _id, skill_table _skills)
    {
        ID = _id;
        lastPos = transform.position = new Vector3(_x, _y, _z);
        nextPos = new Vector3(_x, _y, _z);
        lastDir = transform.rotation = Quaternion.Euler(new Vector3(0, _dir, 0));
        nextDir = Quaternion.Euler(new Vector3(0, _dir, 0));
        animator_ = GetComponent<Animator>();
        skillTable = _skills;
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
        nextPos = new Vector3(_x, CheckSurface(_y), _z);

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

    /// <summary>
    /// 地表の位置を獲得
    /// </summary>
    /// <param name="_y"></param>
    /// <returns></returns>
    private float CheckSurface(float _y)
    {
        // 下に調べる
        Ray ray = new Ray(transform.position, -transform.up);
        Vector3 point = DistanceMeasured(ray, 10, "Ground");
        // 下になかったら
        if (point == Vector3.zero)
        {
            // 上を調べる
            ray = new Ray(transform.position, transform.up);
            point = DistanceMeasured(ray, 10, "Ground");
        }
        // 地面が見つかれば
        if (point != Vector3.zero)
        {
            Vector3 dist = point - ray.origin;

            float dis = Mathf.Sqrt(dist.sqrMagnitude);
            if (dis > 0.0f)
            {
                // 地表のy軸の位置を返す
                return point.y + 0.1f;
            }
        }
        // 見つからなかったらそのままに
        return _y;
    }

    /// <summary>
    /// Rayで当たり判定を取る
    /// </summary>
    /// <param name="_ray"></param>
    /// <param name="_distance"></param>
    /// <param name="_tags"></param>
    /// <returns>Hit = Hitした場所のVector3 / NonHit = Vector3.zero</returns>
    Vector3 DistanceMeasured(Ray _ray,int _distance, string _tags="")
    {
        RaycastHit hitObj;
        if (Physics.Raycast(_ray, out hitObj, _distance))
        {
            if (_tags != "" && _tags != hitObj.collider.tag)
            {
                return Vector3.zero;

            }

            return hitObj.point;
        }
        return Vector3.zero;
    }
    
}
