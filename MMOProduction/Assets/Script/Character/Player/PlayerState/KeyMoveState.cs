////////////////////////////////////////
// キー操作時のステートパターンクラス //
////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMoveState : BaseState
{
    // 速度
    private Vector3 velocity;

    private static BaseState instance;

    // 実体を取得
    public static BaseState Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new KeyMoveState();
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
        if (InputManager.InputMouseCheckDown(0) == INPUT_MODE.PLAY)
        {
            playerController.LockOn();
        }

        velocity = Vector3.zero;

        VelocityDecision();

        if (velocity.magnitude > 0)
        {
            playerController.Move(velocity);
        }

        if(!InputManager.InputKeyCheck(playerSetting.FKey) && 
            !InputManager.InputKeyCheck(playerSetting.BKey) && 
            !InputManager.InputKeyCheck(playerSetting.LKey) && 
            !InputManager.InputKeyCheck(playerSetting.RKey))
        {
            playerController.ChangeState(IdleState.Instance);
        }
        
    }

    public override void End()
    {
    }

    private void VelocityDecision()
    {
        // キー判定
        if (InputManager.InputKeyCheck(playerSetting.FKey))
        {
            velocity.x -= 1;
        }
        else if (InputManager.InputKeyCheck(playerSetting.BKey))
        {
            velocity.x += 1;
        }

        if (InputManager.InputKeyCheck(playerSetting.LKey))
        {
            velocity.z -= 1;
        }
        else if (InputManager.InputKeyCheck(playerSetting.RKey))
        {
            velocity.z += 1;
        }

        if (playerController.Target)
        {
            if (playerController.Target.GetComponent<Marker>().LOCK_OBSERVE)
            {
                if (velocity.x == 0)
                {
                    velocity.x = -0.25f;
                }
            }
        }

        // 正規化
        velocity = velocity.normalized;

        // 押していたらダッシュ
        if (InputManager.InputKeyCheck(playerSetting.DKey))
        {
            if (playerSetting.IA)
            {
                animatorManager.AnimChange((int)PlayerAnim.PARAMETER_ID.RUN);
                playerController.RunFlag = true;
            }
            velocity *= playerSetting.DS;
        }
        else
        {
            if (playerSetting.IA)
            {
                animatorManager.AnimChange((int)PlayerAnim.PARAMETER_ID.WALK);
                playerController.RunFlag = false;
            }
            velocity *= playerSetting.NS;
        }
    }
}
