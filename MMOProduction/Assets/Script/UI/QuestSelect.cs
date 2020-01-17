using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelect : MonoBehaviour
{
    public void Close() {
        gameObject.SetActive(false);
    }

    public void MoveMap(int _id) {
        gameObject.SetActive(false);
        UserRecord.NextMap = (MapID)_id;
    }
}
