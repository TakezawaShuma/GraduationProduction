using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Status
{
    public int hp;
    public int mp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int dexterity;
    public int agility;

    public Status(int hp = 0, int mp = 0, int strength = 0, int vitality = 0, int intelligence = 0, int mind = 0, int dexterity = 0, int agility = 0)
    {
        this.hp = hp;
        this.mp = mp;
        this.strength = strength;
        this.vitality = vitality;
        this.intelligence = intelligence;
        this.mind = mind;
        this.dexterity = dexterity;
        this.agility = agility;
    }
}
