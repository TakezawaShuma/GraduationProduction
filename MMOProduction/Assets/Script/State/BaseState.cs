using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerControllerV2 playerController;

    protected PlayerSetting playerSetting;

    public void Initialized(PlayerControllerV2 playerController, PlayerSetting playerSetting)
    {
        this.playerController = playerController;
        this.playerSetting = playerSetting;
    }

    // 実行関数
    public abstract void Execute();
}
