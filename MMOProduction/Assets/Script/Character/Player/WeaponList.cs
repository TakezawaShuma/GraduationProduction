using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weapons = null;

    public GameObject GetWeapons(int _i)
    {
        return weapons[_i]; 
    }
}
