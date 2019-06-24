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

        dir = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(0, dir, 0);
    }

    public void UpdatePosition(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public void UpdateDirection(float _dir)
    {
        dir = _dir;
    }
}
