using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveState : BaseState
{
    // 速度
    private Vector3 velocity;
        
    private RaycastHit hit;
    
    private Ray ray;

    private static BaseState instance;

    // 実体を取得
    public static BaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MouseMoveState();
            }

            return instance;
        }
    }

    // 実行関数
    public override void Execute()
    {
        VelocityDecision(GetMousePos());

        if (velocity.magnitude > 0)
        {
            playerController.Move(velocity);
        }

        if(Input.GetMouseButtonUp(0))
        {
            playerController.ChangeState(IdleState.Instance);
        }

        Debug.Log("mouse");
    }

    private Vector3 GetMousePos()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void VelocityDecision(Vector3 clickPosition)
    {
        Vector3 v = clickPosition - playerController.transform.position;

        float rad = Mathf.Atan2(v.x, v.z) + (270.0f * Mathf.Deg2Rad);

        velocity.x = Mathf.Sin(rad);
        velocity.z = Mathf.Cos(rad);

        velocity = velocity.normalized;

        velocity *= playerSetting.DS * Time.deltaTime;
    }
}
