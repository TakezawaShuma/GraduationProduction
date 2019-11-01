//
// KillState.cs
//
// Author : Tama
//
// 敵を斬りつける
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillState : BaseState
{
    private static BaseState _instance;
    public static BaseState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new KillState();
            return _instance;
        }
    }

    private bool _flag = false;

    // 実行関数
    public override void Execute()
    {

        var currentState = playerController.Animator.GetCurrentAnimatorStateInfo(0);
        PlayerWepon.WeponData weponData = playerController.WeponData.FindWeponFromName("sword");


        // オブジェクト表示状態の判定
        weponData._wepon.gameObject.SetActive(true);

        if (currentState.IsName("Kill") != false)
        {
            _flag = true;
        }

        if (_flag)
        {
            if (currentState.IsName("Kill") != true)
            {
                weponData._wepon.gameObject.SetActive(false);
                playerController.ChangeState(IdleState.Instance);
                _flag = false;
                return;
            }
        }

    }
}
