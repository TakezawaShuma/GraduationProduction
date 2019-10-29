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

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }


    private Quaternion lastDir = new Quaternion();
    private Quaternion nextDir = new Quaternion();

    private float nowFlame = 0;

    Vector3 last = new Vector3();
    Vector3 next = new Vector3();

    private const float UPDATE_SPEED = 1.0f / 3.0f;
    private const int MAX_FLAME = 3;

    public int id = 0;
    public OtherPlayers() { }
    public void Init(int _i) { id = _i; }

    // Start is called before the first frame update
    void Start()
    {
        last = transform.position;
        lastDir = nextDir = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        LerpMove();
    }

    public void UpdataData(int _hp, int _mp, float _x, float _y, float _z, float _dir)
    {
        HP = _hp; MP = _mp;

        // 向きを決める
        lastDir = transform.rotation;
        nextDir = Quaternion.Euler(0, _dir, 0);

        // 位置を決める
        last = transform.position;
        next = new Vector3(_x, _y, _z);
        
        // カウントを初期化
        nowFlame = 0;
    }

    /// <summary>
    /// 他プレイヤーの移動を補間する
    /// </summary>
    private void LerpMove()
    {
        nowFlame += UPDATE_SPEED;
        transform.localRotation = Quaternion.Lerp(lastDir, nextDir, nowFlame);
        transform.position = Vector3.Lerp(last, next, nowFlame);
    }
}
