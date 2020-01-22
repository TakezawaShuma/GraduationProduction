using System.Collections.Generic;
using UnityEngine;

// TODO: 処理速度に心配あり
public static class AccessoryDatas
{
    private static List<Packes.AccessoryMasterData> datas_ = new List<Packes.AccessoryMasterData>();
    
    public static Packes.AccessoryMasterData FindOne(int _id) {
        foreach(Packes.AccessoryMasterData _data in datas_) { 
             if(_id == _data.id) {
                return _data;
             }
         }
        return default;
    }

    public static List<Packes.AccessoryMasterData> Find(List<int> _ids) {
        List<Packes.AccessoryMasterData> result = new List<Packes.AccessoryMasterData>(); 
        foreach(int _id in _ids) { 
            foreach(Packes.AccessoryMasterData _data in datas_) { 
                if(_id == _data.id) {
                    result.Add(_data);
                }
            }
        }
        return result;
    }    
    
    public static List<Packes.AccessoryMasterData> Find(int[] _ids) {
        List<Packes.AccessoryMasterData> result = new List<Packes.AccessoryMasterData>(); 
        foreach(int _id in _ids) { 
            foreach(Packes.AccessoryMasterData _data in datas_) { 
                if(_id == _data.id) {
                    result.Add(_data);
                }
            }
        }
        return result;
    }

    public static void SaveingData(List<Packes.AccessoryMasterData> _data) {
        datas_ = _data;
    }

    public static void Dump()
    {
        foreach (Packes.AccessoryMasterData _data in datas_) {
            Debug.Log(_data.name);
        }
    }
}