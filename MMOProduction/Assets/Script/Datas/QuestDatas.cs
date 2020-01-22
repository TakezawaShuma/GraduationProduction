using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestDatas
{
    private static List<Packes.QuestMasterData> datas_ = new List<Packes.QuestMasterData>();


    public static Packes.QuestMasterData FindOne(int _id)
    {
        foreach (Packes.QuestMasterData _data in datas_)
        {
            if (_id == _data.id)
            {
                return _data;
            }
        }
        return default;
    }
    public static Packes.QuestMasterData FindOne(MapID _id)
    {
        foreach (Packes.QuestMasterData _data in datas_)
        {
            if ((int)_id == _data.id)
            {
                return _data;
            }
        }
        return default;
    }

    public static List<Packes.QuestMasterData> Find(List<int> _ids)
    {
        List<Packes.QuestMasterData> result = new List<Packes.QuestMasterData>();
        foreach (int _id in _ids)
        {
            foreach (Packes.QuestMasterData _data in datas_)
            {
                if (_id == _data.id)
                {
                    result.Add(_data);
                }
            }
        }
        return result;
    }

    public static List<Packes.QuestMasterData> Find(int[] _ids)
    {
        List<Packes.QuestMasterData> result = new List<Packes.QuestMasterData>();
        foreach (int _id in _ids)
        {
            foreach (Packes.QuestMasterData _data in datas_)
            {
                if (_id == _data.id)
                {
                    result.Add(_data);
                }
            }
        }
        return result;
    }

    public static void SaveingData(List<Packes.QuestMasterData> _data)
    {
        datas_ = _data;
    }

    public static void Dump()
    {
        foreach (Packes.QuestMasterData _data in datas_)
        {
            Debug.Log(_data.name);
        }
    }
}
