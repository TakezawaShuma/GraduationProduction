using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackState : BaseState
{
    private static BaseState instance;

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
        Debug.Log("攻撃しました");

        playerController.ChangeState(IdleState.Instance);
    }
}
