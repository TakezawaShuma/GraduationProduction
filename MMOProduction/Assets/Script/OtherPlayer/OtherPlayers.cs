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




    // Start is called before the first frame update
    void Start() {
        HAYAKAWA oi = new HAYAKAWA();
        oi.command = 101;
        oi.pos = new Vector3(99, 999, 99);
        string str = JsonUtility.ToJson(oi);
        Debug.Log(str);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(x, y, z);
        transform.Rotate(0, dir, 0);
    }

    public void UpdataData(int hp, int mp, float x, float y, float z, float dir)
    { HP = hp; MP = mp; X = x; Y = y; Z = z; Dir = dir; }
}
