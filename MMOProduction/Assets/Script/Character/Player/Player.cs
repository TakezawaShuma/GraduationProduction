//////////////////////
// プレイヤークラス //
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : CharacterBase
{
    //[SerializeField, Header("プレイヤーの設定")]
    //private PlayerSetting playerSetting;

    // 位置
    private Vector4 position = default(Vector4);
    public Vector3 PositionV3 { get { return new Vector3(position.x, position.y, position.z); } }
    public Vector4 PositionV4 { get { return position; } set { position = value; } }

    // 回転
    private Quaternion rotation = default(Quaternion);
    public Quaternion Rotation { get { return rotation; } set { rotation = value; } }

    // リジッドボディ
    private Rigidbody playerRigid = null;

    //private Status status;

    public int Status { get; set; }
    public int Lv { get; set; }
    public int EXP { get; set; }

    // ステータス
    public int STR { get; set; }        // 筋力 攻撃力 所持アイテム数等
    public int VIT { get; set; }        // 体力 防御力 状態異常耐性
    public int INT { get; set; }        // 知力 魔法攻撃力 MP最大値
    public int MND { get; set; }        // 精神力 魔法防御力 
    public int DEX { get; set; }        // 器用 
    public int AGI { get; set; }        // 機動力 移動速度 攻撃範囲に影響？

    public Rigidbody Rigid { get { return playerRigid; } set { playerRigid = value; } }


    private void Start()
    {
        hp = mp = maxHp = maxMp = 1;

        position = transform.position;
        rotation = transform.rotation;

        playerRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        position = Rigid.position;
        playerRigid.rotation = rotation;
    }


    public void UpdateStatus(int _maxHp, int _hp,int _maxMp,int _mp, int _status)
    {
        hp = _hp;
        mp = _mp;
        Status = _status;
        maxHp = _maxHp;
        maxMp = _maxMp;
    }

}