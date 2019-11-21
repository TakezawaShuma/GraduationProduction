////////////////////////////////////////////
// スタートが全部終了したか確認するクラス //
////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready
{
    private static Ready instance;

    private bool isReady;

    public static Ready Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Ready();
            }

            return instance;
        }
    }

    public void ReadyGO()
    {
        isReady = true;
    }

    public bool CheckReady()
    {
        return isReady;
    }
}
