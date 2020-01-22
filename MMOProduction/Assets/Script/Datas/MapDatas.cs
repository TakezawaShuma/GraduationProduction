using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapDatas
{
    public struct MapData
    {
        public int id;
        public int x;
        public int y;
        public int z;
        public int dir;
        public GameObject pre;

        public MapData(
            int _id,
            int _x,
            int _y,
            int _z,
            int _dir,
            GameObject _pre
                )
        {
            id = _id;
            x = _x;
            y = _y;
            z = _z;
            dir = _dir;
            pre = _pre;
        }
    }

    private static List<MapData> datas_ = new List<MapData>();


    public static MapData FindOne(int _id)
    {
        foreach (MapData _data in datas_)
        {
            if (_id == _data.id)
            {
                return _data;
            }
        }
        return default;
    }
    public static MapData FindOne(MapID _id)
    {
        foreach (MapData _data in datas_)
        {
            if ((int)_id == _data.id)
            {
                return _data;
            }
        }
        return default;
    }

    public static List<MapData> Find(List<int> _ids)
    {
        List<MapData> result = new List<MapData>();
        foreach (int _id in _ids)
        {
            foreach (MapData _data in datas_)
            {
                if (_id == _data.id)
                {
                    result.Add(_data);
                }
            }
        }
        return result;
    }

    public static List<MapData> Find(int[] _ids)
    {
        List<MapData> result = new List<MapData>();
        foreach (int _id in _ids)
        {
            foreach (MapData _data in datas_)
            {
                if (_id == _data.id)
                {
                    result.Add(_data);
                }
            }
        }
        return result;
    }

    public static void SaveingData(List<MapData> _data)
    {
        datas_ = _data;
    }

    public static void Dump()
    {
        foreach (MapData _data in datas_)
        {
            Debug.Log(_data.id);
        }
    }
}