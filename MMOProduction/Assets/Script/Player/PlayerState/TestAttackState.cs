using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackState : BaseState
{
    private static BaseState instance;

    int count = 0;

    int maxCount = 30;

    public static BaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TestAttackState();
            }

            return instance;
        }
    }

    // 実行関数
    public override void Execute()
    {
        if (count < maxCount)
        {
            Debug.Log("攻撃してるYO");
            playerController.AttackCollider.enabled = true;
        }
        else
        {
            Debug.Log("攻撃やめたYO");
            playerController.AttackCollider.enabled = false;
            playerController.ChangeState(IdleState.Instance);
            count = 0;
        }

        count++;
    }
}
