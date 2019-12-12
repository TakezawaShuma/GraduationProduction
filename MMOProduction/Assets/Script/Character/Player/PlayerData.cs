////////////////////////////
// プレイヤーの簡易データ //
////////////////////////////


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
    public Equipment equipment;
    public int lv;
    public int exp;
}

/// <summary>
/// 装備クラス
/// </summary>
public class Equipment
{
    // 武器ID
    int weapon;
    // 兜ID
    int head;
    // 鎧ID
    int body;
    // 具足ID
    int leg;
    // アクセサリー1ID
    int accessoryL;
    // アクセサリー2ID
    int accessoryR;
}
