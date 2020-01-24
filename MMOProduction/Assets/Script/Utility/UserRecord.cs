////////////////////////////////////
// ゲーム中を通して使用するデータ //
////////////////////////////////////

/// <summary>
/// ゲーム中保持する自分のデータ
/// </summary>
using System.Collections.Generic;
using UnityEngine;
public static class UserRecord
{
    private static int id = 0;
    private static string name = "";
    private static string ip = "";
    private static MapID mapId = MapID.Non;

    private static bool fastCheck = false;
    private static GameObject model = null;
    private static MapID nextMapId = MapID.Non;
    private static int questID = -1;
    private static List<int> inventory = new List<int>();
    private static List<int> accessorys = new List<int>();

    public static GameObject MODEL { get { return model; } set { model = value; } }
    public static bool FAST { get { return fastCheck; } set { fastCheck = value; } }
    public static int ID { get { return id; } set { if(id==0)id = value; } }
    public static string Name { get { return name; } set { if (name == "") name = value; } }
    public static string IP { get { return ip; } set { ip = value; } }
    public static MapID MapID { get { return mapId; } set { mapId = value; } }
    public static MapID NextMapId { get { return nextMapId; } set { nextMapId = value; } }
    public static int QuestID { get { return questID; } set { questID = value; } }
    public static List<int> Inventory { get { return inventory; } set { inventory = value; } }
    public static List<int> Accessorys { get { return accessorys; } set { accessorys = value; } }

    public static void DiscardAll()
    {
        id = 0;
        name = "";
        ip = "";
        mapId = MapID.Title;
    }
}