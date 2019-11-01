//
// PlayerCommand.cs
//
// Author : Tama
//
// プレイヤーが使用すコマンドをまとめたクラス
//

using UnityEngine;


public abstract class PlayerCommand
{

    protected Player _owner;
    public Player Owner
    {
        get { return _owner; }
    }

    public PlayerCommand(Player owner)
    {
        _owner = owner;
    }
}
