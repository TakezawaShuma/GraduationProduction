using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Status
{
    public int hitPoint;
    public int magicPoint;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int dexterity;
    public int agility;

    public Status(int hp = 0, int mp = 0, int str = 0, int vit = 0, int inte = 0, int min = 0, int dex = 0, int agi = 0)
    {
        hitPoint = hp;
        magicPoint = mp;
        strength = str;
        vitality = vit;
        intelligence = inte;
        mind = min;
        dexterity = dex;
        agility = agi;
    }
}
