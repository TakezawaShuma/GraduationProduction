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

    public void Initialized(PlayerController playerController, PlayerSetting playerSetting)
    {
        this.playerController = playerController;
        this.playerSetting = playerSetting;
    }

    // 実行関数
    public abstract void Execute();
}
