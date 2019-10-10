//////////////////////////////////
// 自分以外のプレイヤーのクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MAXHP　　　　　　変
   MP　　　　　　変
nowHP　常(変)
   MP　常(変)
Sutas　常(変)
ロール(ジョブ)　変
パーティ番号　　変
レベル　　　　　変
名前　　　　　　変
ヘイト順　常(変)

 */

public class OtherPlayers: MonoBehaviour
{
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


    public void Init(float _x,float _y ,float _z,float _dir)
    {
        X = _x;
        Y = _y;
        Z = _z;
        Dir = _dir;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(X, Y, Z);
        transform.Rotate(0, dir, 0);
    }

    public void UpdataData(int hp, int mp, float x, float y, float z, float dir)
    {
        HP = hp; MP = mp; X = x; Y = y; Z = z; Dir = dir;
    }
}
