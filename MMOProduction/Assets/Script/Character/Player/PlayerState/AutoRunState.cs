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

    public override void Start()
    {
    }

    public override void Execute()
    {
        if(playerSetting.IA)
        {
            //animatorManager.AnimChange((int) PlayerAnim.PARAMETER_ID.RUN);
            //player.animationType = PlayerAnim.PARAMETER_ID.RUN;
            Debug.Log("RUN");
        }

        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
        {
            playerController.LockOn();
        }

        // 前に進む
        velocity = new Vector3(-1, 0, 0) * playerSetting.DS;

        playerController.Move(velocity);

        if (InputManager.InputKeyCheckDown(playerSetting.FKey) || 
            InputManager.InputKeyCheckDown(playerSetting.BKey) || 
            InputManager.InputKeyCheckDown(playerSetting.LKey) || 
            InputManager.InputKeyCheckDown(playerSetting.RKey))
        {
            playerController.ChangeState(KeyMoveState.Instance);
        }
    }

    public override void End()
    {
    }
}
