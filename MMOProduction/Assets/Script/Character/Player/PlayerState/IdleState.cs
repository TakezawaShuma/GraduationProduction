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

    public override void Start()
    {
    }

    // 実行関数
    public override void Execute()
    {
        if (playerSetting.IA)
        {
            animatorManager.AnimChange((int)PlayerAnim.PARAMETER_ID.IDLE);
            player.animationType = PlayerAnim.PARAMETER_ID.IDLE;
        }

        if (InputManager.InputKeyCheckDown(playerSetting.FKey) || 
            InputManager.InputKeyCheckDown(playerSetting.BKey) || 
            InputManager.InputKeyCheckDown(playerSetting.LKey) || 
            InputManager.InputKeyCheckDown(playerSetting.RKey))
        {
            playerController.ChangeState(KeyMoveState.Instance);
        }
        else if (InputManager.InputKeyCheckDown(playerSetting.AKey))
        {
            playerController.ChangeState(AutoRunState.Instance);
        }
        else if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
        {
            if (playerController.MODE == PlayerController.Mode.Battle)
            {
                playerController.ChangeState(NormalAttackState.Instance);

            }
        }
        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
        {
            playerController.LockOn();
        }

        playerController.NoMove();

    }

    public override void End()
    {
    }
}
