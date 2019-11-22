//////////////////////
// プレイヤークラス //
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    //[SerializeField, Header("プレイヤーの設定")]
    //private PlayerSetting playerSetting;

    private Vector4 position = new Vector4();

    private float x;
    private float y;
    private float z;

    //private Status status;
    public int HP { get; set; }
    public int MP { get; set; }
    public int MaxHp { get; set; }
    public int MaxMp { get; set; }
    public int Status { get; set; }
    public Vector4 Position { get { return position; } set { position = value; } }
    private int strength;
    private int vitality;
    private int Intelligence;
    private int mind;
    private int dexterity;
    private int agility;

    private float dir;

    private Rigidbody rb;


    private void Start()
    {
        HP = 30;
        MP = 80;

        MaxHp = 100;
        MaxMp = 100;

        x = 0;
        y = 0;
        z = 0;

        dir = transform.rotation.eulerAngles.y;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //rb.AddForce(new Vector3(x, y, z) * 100);
        //transform.rotation = Quaternion.Euler(0, dir, 0);
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

    public Vector4 GetPosition()
    {
        return position;
    }
    public void UpdateStatus( int _hp,  int _mp, int _status)
    {
        HP = _hp;
        MP = _mp;
        Status = _status;
    }

}