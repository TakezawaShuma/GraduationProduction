using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponList:MonoBehaviour
{
    public WeaponBase[] weapons = null;

    public GameObject GetWeapon(int _i)
    {
        if (!weapons[_i]) return null;
        return weapons[_i].gameObject; 
    }
    public GameObject FindWeapon(int _id)
    {
        foreach(var tmp in weapons)
        {
            if (_id == tmp.weapon_id)
            {
                return tmp.gameObject;
            }
        }
        return null;
    }
}
