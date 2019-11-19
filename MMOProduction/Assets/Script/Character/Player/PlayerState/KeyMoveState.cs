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
        if (Input.GetMouseButtonDown(0))
        {
            playerController.LockOn();
        }

        velocity = Vector3.zero;

        VelocityDecision();

        if (velocity.magnitude > 0)
        {
            playerController.Move(velocity);
        }

        if(!Input.GetKey(playerSetting.FKey) && !Input.GetKey(playerSetting.BKey) && !Input.GetKey(playerSetting.LKey) && !Input.GetKey(playerSetting.RKey))
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
        if (Input.GetKey(playerSetting.FKey))
        {
            velocity.x -= 1;
        }
        else if (Input.GetKey(playerSetting.BKey))
        {
            velocity.x += 1;
        }

        if (Input.GetKey(playerSetting.LKey))
        {
            velocity.z -= 1;
        }
        else if (Input.GetKey(playerSetting.RKey))
        {
            velocity.z += 1;
        }

        if (playerController.TARGET)
        {
            if (playerController.TARGET.GetComponent<Marker>().LOCK_OBSERVE)
            {
                if (velocity.x == 0)
                {
                    velocity.x = -1;
                }
            }
        }

        // 正規化
        velocity = velocity.normalized;

        // 押していたらダッシュ
        if (Input.GetKey(playerSetting.DKey))
        {
            if (playerSetting.IA)
            {
                animatorManager.Run();
            }
            velocity *= playerSetting.DS;
        }
        else
        {
            if (playerSetting.IA)
            {
                animatorManager.Walk();
            }
            velocity *= playerSetting.NS;
        }
    }
}
