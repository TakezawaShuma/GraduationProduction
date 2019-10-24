﻿////////////////////////////////////
// ゲーム中を通して使用するデータ //
////////////////////////////////////

/// <summary>
/// ゲーム中保持する自分のデータ
/// </summary>
public static class Retention
{
    private static int id;
    private static string name;
    private static string ip = "";
    public static int ID { get { return id; } set { if (id == 0) id = value; } }
    public static string Name { get { return name; } set { if (name == "") name = value; } }
    public static string IP { get { return ip; } set { ip = value; } }
}