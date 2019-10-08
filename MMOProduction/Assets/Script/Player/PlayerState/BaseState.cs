//////////////////////////////////////////////
// プレイヤーのステートパターン用基底クラス //
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerController playerController;

    protected PlayerSetting playerSetting;

    protected AnimatorManager animatorManager;

    public void Initialized(PlayerController playerController, PlayerSetting playerSetting, AnimatorManager animatorManager)
    {
        this.playerController = playerController;
        this.playerSetting = playerSetting;
        this.animatorManager = animatorManager;
    }

    // 実行関数
    public abstract void Execute();
}
