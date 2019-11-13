using System.Collections;
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
        animatorManager.NormalAttack();

        Collider weapon = playerController.AttackCollider.GetComponent<Collider>();

        weapon.enabled = true;

        changeAnimeState = animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Attack"));
        playerController.SKIL = 0;
    }

    public override void Execute()
    {
        changeAnimeState = animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("NormalAttack"));

        // Attackアニメに切り替わったら
        if (changeAnimeState)
        {
            // Attackアニメが終了したら
            if (animatorManager.ANIMATOR.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                playerController.ChangeState(IdleState.Instance);
            }
        }
    }

    public override void End()
    {
        playerController.AttackCollider.GetComponent<Collider>().enabled = false;

    }



}
