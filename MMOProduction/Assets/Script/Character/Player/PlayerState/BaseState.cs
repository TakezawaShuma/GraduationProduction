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

    protected Player player;
    public void Initialized(PlayerController playerController, PlayerSetting playerSetting, AnimatorManager animatorManager, Player _player)
    {
        this.playerController = playerController;
        this.playerSetting = playerSetting;
        this.animatorManager = animatorManager;
        this.player = _player;
    }

    public abstract void Start();

    // 実行関数
    public abstract void Execute();

    public abstract void End();
}
