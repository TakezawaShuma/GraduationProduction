//////////////////////////////////
// 自分以外のプレイヤーのクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HAYAKAWA
{
    public int command;
    public Vector3 pos;
}

public class OtherPlayers: MonoBehaviour
{
    private int hp = 0;
    private int mp = 0;
    private float x = 0;
    private float y = 0;
    private float z = 0;
    private float dir = 0;

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    public float X { get { return x; } set { x = value; } }
    public float Y { get { return y; } set { y = value; } }
    public float Z { get { return z; } set { z = value; } }
    public float Dir { get { return dir; } set { dir = value; } }

    private float lastX = 0;
    private float lastY = 0;
    private float lastZ = 0;
    private float lastDir = 0;

    private int nowFlame = 0;

    private const int MAX_FLAME = 3;


    // Start is called before the first frame update
    void Start()
    {
        HAYAKAWA oi = new HAYAKAWA();
        oi.command = 101;
        oi.pos = new Vector3(99, 999, 99);
        string str = JsonUtility.ToJson(oi);
        Debug.Log(str);
    }


    // Update is called once per frame
    void Update()
    {
        LerpMove();
        //transform.position = new Vector3(x, y, z);
        //transform.Rotate(0, dir, 0);
    }

    public void UpdataData(int hp, int mp, float x, float y, float z, float dir)
    {
        lastX = this.x;
        lastY = this.y;
        lastZ = this.z;
        lastDir = this.dir;

        HP = hp; MP = mp; X = x; Y = y; Z = z; Dir = dir;

        nowFlame = 0;
        transform.position = new Vector3(lastX, lastY, lastZ);
        transform.rotation = Quaternion.Euler(0, lastDir, 0);
    }

    private void LerpMove()
    {
        Vector3 last = new Vector3(lastX, lastY, lastZ);
        Vector3 next = new Vector3(x, y, z);

        float t = 1 / 3 * (nowFlame + 1);

        Vector3 v = Vector3.Lerp(last, next, t);
        Quaternion.Slerp(Quaternion.Euler(0,lastDir,0), Quaternion.Euler(0,dir,0), t);

        transform.position = v;

        nowFlame++;
    }
}
