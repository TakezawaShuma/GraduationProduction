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
        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
        {
            playerController.LockOn();
        }
        playerController.NoMove();
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
}
