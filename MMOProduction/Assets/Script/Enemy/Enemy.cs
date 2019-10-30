using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float UPDATE_SPEED = 1.0f / 3.0f;

    private int maxHp;
    private int maxMp;
    private int hp = 0;
    private int mp = 0;
    private Vector3 position;
    private float dir = 0;

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    public float X { get { return position.x; } set { position.x = value; } }
    public float Y { get { return position.y; } set { position.y = value; } }
    public float Z { get { return position.z; } set { position.z = value; } }
    public float Dir { get { return dir; } set { dir = value; } }

    private float lastX = 0;
    private float lastY = 0;
    private float lastZ = 0;
    private float lastDir = 0;


    private float nowFlame = 0;

    Vector3 last;
    Vector3 next;

    private const int MAX_FLAME = 3;

    // Start is called before the first frame update
    void Start()
    {
        X = lastX = last.x = transform.position.x;
        Y = lastY = last.y = transform.position.y;
        Z = lastZ = last.z = transform.position.z;
        Dir = lastDir = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        LerpMove();
    }


    public void UpdataData(int _hp, int _mp, float _x, float _y, float _z, float _dir)
    {
        lastX = X;
        lastY = Y;
        lastZ = Z;
        lastDir = Dir;

        HP = _hp; MP = _mp; X = _x; Y = _y; Z = _z; Dir = _dir;

        last = transform.position;
        next = new Vector3(X, Y, Z);

        nowFlame = 0;

        transform.position = new Vector3(lastX, lastY, lastZ);
        transform.rotation = Quaternion.Euler(0, lastDir, 0);
    }


    private void LerpMove()
    {
        nowFlame += UPDATE_SPEED;

        Vector3 v = Vector3.Lerp(last, next, nowFlame);
        transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, lastDir, 0), Quaternion.Euler(0, dir, 0), nowFlame);
   
        transform.position = v;
    }
}
