﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackState : BaseState
{
    private static BaseState instance;

    private bool changeAnimeState = false;

    // 実体を取得
    public static BaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NormalAttackState();
            }

            return instance;
        }
    }

    public override void Start()
    {
        animatorManager.AnimChange((int)PlayerAnim.PARAMETER_ID.ATTACK);
        player.animationType = PlayerAnim.PARAMETER_ID.ATTACK;
        changeAnimeState = animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Attack"));
        HitRange(10);
        playerController.SKIL = 0;

        this.playerController.Sword.PlayAttack();
    }

    public override void Execute()
    {
        player.GetComponent<PlayerController>().HoldingWeapon();
        //changeAnimeState = animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Attack"));

        //// Attackアニメに切り替わったら
        //if (changeAnimeState)
        //{
        //    // Attackアニメが終了したら
        //    if (animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        //    {
        //        // 剣の軌跡の表示を停止する
        //        this.playerController.Sword.StopAttack();
        //    }
        //}
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    private void Animation(PlayerAnim.PARAMETER_ID _animationType)
    {
        if (player.lastAnimation != _animationType)
        {
            animatorManager.AnimChange((int)_animationType);
            player.lastAnimation = _animationType;
        }
    }


    public override void End()
    {

    }

    private bool HitRange(float _range)
    {
        float dis = playerController.Distance(playerController.Target);
        if (dis < _range && dis > 0)
        {
            int enemyID = playerController.Target.gameObject.GetComponentInParent<Enemy>().ID;
            int myID = UserRecord.ID;

            WS.WsPlay.Instance.Send(new Packes.Attack(enemyID, UserRecord.ID, 0, 0).ToJson());
            return true;
        }
        return false;
    }

}
