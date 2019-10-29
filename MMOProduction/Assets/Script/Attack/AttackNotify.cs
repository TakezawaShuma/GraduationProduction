using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNotify
{

    public struct NotifyData
    {
        public int user_id;
        public int skill_id;
        public Vector3 spawn_pos;

        public NotifyData(int user_id, int skill_id, Vector3 spawn_pos)
        {
            this.user_id = user_id;
            this.skill_id = skill_id;
            this.spawn_pos = spawn_pos;
        }
    }


    private AttackNotify()
    {
        _notifyData = new List<NotifyData>();
    }

    public void SendAttack(int user_id, int skill_id, Vector3 spawnPos = default)
    {
        NotifyData data = new NotifyData(user_id, skill_id, spawnPos);
        _notifyData.Add(data);
    }

    public void SendAttack(NotifyData data)
    {
        _notifyData.Add(data);
    }

    private List<NotifyData> _notifyData;
    public List<NotifyData> NotifyDataList
    {
        get { return _notifyData; }
    }

    private static AttackNotify _instance;
    public static AttackNotify Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AttackNotify();
            return _instance;
        }
    }
}
