//
// CommandAttack.cs
//
// Author : Tama
//
// スキルの発動に使用するコマンド
//

using System;
using UnityEngine;

public class CommandAttack : PlayerCommand
{

    public Func<bool> Kill { get; set; }
    public Func<bool> Punch { get; set; }

    public CommandAttack(Player owner):
        base(owner)
    {
    }
}
