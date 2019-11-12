using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider[] weapons;

    public CapsuleCollider[] WEAPONS
    {
        get { return weapons; }
    }
}
