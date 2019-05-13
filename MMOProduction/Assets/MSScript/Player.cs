using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    private float x;
    private float y;
    private float z;
    private int hp;
    private int mp;
    private float dir;

    public float X
    {
        set { x = value; UpdatePosition(); }
    }

    public float Y
    {
        set { y = value; UpdatePosition(); }
    }

    public float Z
    {
        set { z = value; UpdatePosition(); }
    }

    public float Dir
    {
        set { dir = value; UpdateDirection(); }
    }

    public int HP
    {
        set { hp = value; }
    }

    public int MP
    {
        set { mp = value; }
    }

    private void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        dir = transform.rotation.y;
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(x, y, z);
    }

    private void UpdateDirection()
    {
        transform.rotation = Quaternion.Euler(0, dir, 0);
    }
}
