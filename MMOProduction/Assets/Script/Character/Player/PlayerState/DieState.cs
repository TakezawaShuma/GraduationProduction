using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private static BaseState instance;

    // 実体を取得
    public static BaseState Instance
    {
        get
        {
            return (instance == null) ? instance = new DieState() : instance;

            //if (instance == null) { instance = new DieState(); }

            //return instance;
        }
    }
    public override void Start()
    {
        animatorManager.AnimChange((int)PlayerAnim.PARAMETER_ID.DIE);
        player.animationType = PlayerAnim.PARAMETER_ID.DIE;
    }

    // 実行関数
    public override void Execute()
    {


    }
    public override void End()
    {
    }
}
