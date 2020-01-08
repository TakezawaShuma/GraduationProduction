﻿///////////////////////////////////////////
// 何処でも使うクラスや関数をまとめる.cs //
///////////////////////////////////////////

using UnityEngine;

/// <summary>
/// パケットデータのJSON化かけるクラス
/// </summary>
public static class Json 
{
    /// <summary>
    /// パケットをJSON形式に変換する
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ConvertToJson(Packes.IPacketDatas packet)
    {
        return JsonUtility.ToJson(packet);
    }

    /// <summary>
    /// JSONをパケットに変換する
    /// </summary>
    /// <typeparam name="Type">変換後のパケット</typeparam>
    /// <param name="json">変換するJSON</param>
    /// <returns></returns>
    public static Type ConvertToPackets<Type>(string json)
    {
        return JsonUtility.FromJson<Type>(json);
    }

    /// <summary>
    /// パケットをJSON形式に変換する
    /// </summary>
    public static string ToJson(this Packes.IPacketDatas _data) {
        return JsonUtility.ToJson(_data);
    }

    /// <summary>
    /// アクセサリーのマスターパーサー
    /// </summary>
    public static void ReadAccessoryMasterData() {
        InputFile.ReadFile(MasterFileNameList.accessory);
    }

    /// <summary>
    /// マップのマスターパーサー
    /// </summary>
    public static void ReadMapMasterData() {
        InputFile.ReadFile(MasterFileNameList.map);
    }
}
