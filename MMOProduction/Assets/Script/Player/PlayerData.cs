////////////////////////////
// プレイヤーの簡易データ //
////////////////////////////

[System.Serializable]
public class PlayerData
{
    public int id;
    public float x;
    public float y;
    public float z;
    public int dir;

    public PlayerData(int _id,float _x,float _y,float _z,int _dir) {
        id = _id; x = _x; y = _y; z = _z; dir = _dir;
    }

    public PlayerData(Packes.RecvPosSync _data) {
        id = _data.user_id; x = _data.x; y = _data.y; z = _data.z; dir = _data.dir;
    }
}

/// <summary>
/// プレイヤーの状態
/// </summary>
public class PlayerStatus{
    public int id;
    public int hp;
    public int mp;
}


/// <summary>
/// セーブデータ
/// </summary>
public class SaveData{
    public SaveData(Weapon _weapon, UnityEngine.Vector3 _pos, int _lv, int _exp) { weapon = _weapon; position = _pos; lv = _lv; exp = _exp; }
    public Weapon weapon;
    public UnityEngine.Vector3 position;
    public int lv;
    public int exp;
}

/// <summary>
/// 装備クラス
/// </summary>
public class Weapon
{
    int weapon;
    int head;
    int body;
    int leg;
    int accessoryL;
    int accessoryR;
}
