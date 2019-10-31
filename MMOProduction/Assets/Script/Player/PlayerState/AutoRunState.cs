////////////////////////////////
// オートランのステートクラス //
////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRunState : BaseState
{
    // 速度
    private Vector3 velocity;

    private static BaseState instance;

    public static BaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AutoRunState();
            }

            return instance;
        }
    }

    public override void Execute()
    {
        if(playerSetting.IA)
        {
            animatorManager.Run();
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerController.LockOn();
        }

        // 前に進む
        velocity = new Vector3(-1, 0, 0) * playerSetting.DS * Time.deltaTime;

        playerController.Move(velocity);

        if (Input.GetKeyDown(playerSetting.FKey) || Input.GetKeyDown(playerSetting.BKey) || Input.GetKeyDown(playerSetting.LKey) || Input.GetKeyDown(playerSetting.RKey))
        {
            playerController.ChangeState(KeyMoveState.Instance);
        }
    }
}
