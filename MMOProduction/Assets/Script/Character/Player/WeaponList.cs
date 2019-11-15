using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weapons;

    public GameObject GetWeapons(int _i)
    {
        return weapons[_i]; 
    }
}
