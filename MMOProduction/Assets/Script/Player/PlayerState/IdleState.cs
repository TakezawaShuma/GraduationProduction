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
        //Debug.Log("idle");

        animatorManager.Idle();

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
    }
}
