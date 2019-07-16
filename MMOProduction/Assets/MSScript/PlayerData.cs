using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int id;
    public float X;
    public float Y;
    public float Z;
    public int HP;
    public int MP;
    public int Direction;

    public PlayerData(int _id,float _x,float _y,float _z,int _hp,int _mp,int _dir) {
        id = _id; X = _x; Y = _y; Z = _z; HP = _hp; MP = _mp; Direction = _dir;
    }

    public PlayerData(Packes.RecvPosSync _data) {
        id = _data.user_id; X = _data.x; Y = _data.y; Z = _data.z; HP = _data.hp; MP = _data.mp; Direction = _data.dir;
    }
}
