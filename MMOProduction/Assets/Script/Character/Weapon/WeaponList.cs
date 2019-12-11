using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponList:MonoBehaviour
{
    public WeaponBase[] weapons = null;

    public GameObject GetWeapons(int _i)
    {
        return weapons[_i].gameObject; 
    }
}
