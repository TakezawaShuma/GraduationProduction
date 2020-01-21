using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestToggle : MonoBehaviour
{
    [SerializeField, Header("マップID")]
    private MapID mapID = MapID.Non;

    public MapID GetMapID()
    {
        return mapID;
    }
}
