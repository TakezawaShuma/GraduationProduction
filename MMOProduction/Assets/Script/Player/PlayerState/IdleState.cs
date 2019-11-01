//////////////////////////////////////
// 待機状態のステートパターンクラス //
//////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private static BaseState instance;

    public static BaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IdleState();
            }

            return instance;
        }
    }

    // 実行関数
    public override void Execute()
    {
        //playerController.GetAnim().Move(false);

        if (playerSetting.IA)
        {
            animatorManager.Idle();
        }

        if(Input.GetMouseButtonDown(0))
        {
            playerController.LockOn();
        }

        playerController.NoMove();

        if (Input.GetKeyDown(playerSetting.FKey) || Input.GetKeyDown(playerSetting.BKey) || Input.GetKeyDown(playerSetting.LKey) || Input.GetKeyDown(playerSetting.RKey))
        {
            playerController.ChangeState(KeyMoveState.Instance);
        }
        else if (Input.GetKeyDown(playerSetting.AKey))
        {
            playerController.ChangeState(AutoRunState.Instance);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            playerController.ChangeState(TestAttackState.Instance);
        }

        // 攻撃（デバッグ）
        // 斬る！
        if (playerController.Player.AttackCommand.Kill())
        {
            animatorManager.Kill();
            playerController.ChangeState(KillState.Instance);
        }
    }
}
