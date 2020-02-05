﻿//////////////////////////////////
// 自分以外のプレイヤーのクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MAXHP　　　　　　変
   MP　　　　　　変
nowHP　常(変)
   MP　常(変)
Sutas　常(変)
ロール(ジョブ)　変
パーティ番号　　変
レベル　　　　　変
名前　　　　　　変
ヘイト順　常(変)

 */

public class OtherPlayers: NonPlayer
{

    private PlayerAnim.PARAMETER_ID lastAnimation;
    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    
    private GameObject weapon;
    public GameObject Weapon { get { return weapon; }set { weapon = value; } }


    private bool isCombat;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastDir = nextDir = transform.rotation;
        am = new AnimatorManager();
        am.ANIMATOR = animator_;
        lastAnimation = animationType;

        weapon = GetComponent<WeaponList>().GetWeapon(0);
    }


    // Update is called once per frame
    void Update()
    {
        LerpMove();
        Animation();
    }

    /// <summary>
    /// 位置情報を更新する
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    public override void UpdatePostionData(float _x, float _y, float _z, float _dir)
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
    /// アニメーションの再生
    /// </summary>
    private void Animation()
    {
        if (lastAnimation != animationType)
        {
            am.AnimChange((int)animationType);
            lastAnimation = animationType;
        }
    }


    public void HoldingWeapon()
    {
        weapon.SetActive(true);
    }

    /// <summary>
    /// 武器を隠す
    /// </summary>
    public void HideWeapon()
    {
        weapon.SetActive(false);
    }

    public void NextAnim(int _anim)
    {
        switch ((PlayerAnim.PARAMETER_ID)_anim)
        {
            case PlayerAnim.PARAMETER_ID.IDLE:
                animationType = PlayerAnim.PARAMETER_ID.IDLE;
                break;
            case PlayerAnim.PARAMETER_ID.WALK:
                animationType = PlayerAnim.PARAMETER_ID.WALK;
                break;
            case PlayerAnim.PARAMETER_ID.RUN:
                animationType = PlayerAnim.PARAMETER_ID.RUN;
                break;
            case PlayerAnim.PARAMETER_ID.CONBAT:
                animationType = PlayerAnim.PARAMETER_ID.CONBAT;
                break;
            case PlayerAnim.PARAMETER_ID.ATTACK:
                animationType = PlayerAnim.PARAMETER_ID.ATTACK;
                break;
            case PlayerAnim.PARAMETER_ID.DIE:
                animationType = PlayerAnim.PARAMETER_ID.CONBAT;
                break;
        }
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
