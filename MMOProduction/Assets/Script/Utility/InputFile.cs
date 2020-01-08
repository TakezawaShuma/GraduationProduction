using System.IO;
using UnityEngine;

/// <summary>
/// ファイルの拡張子
/// </summary>
public enum FILETYPE
{
    JSON,
    TXT,

    NONE,
}

public static class MasterFileNameList {
    public static string accessory = "accessoryMaster";
    public static string map = "mapMaster";
}

public class InputFile : MonoBehaviour
{
    static private string[] fileType = { ".json", ".txt" };

    /// <summary>
    /// ファイルの書き出し
    /// </summary>
    /// <returns></returns>
    static public bool WriterJson(string _fileName, string _txt, FILETYPE _type = FILETYPE.JSON)
    {
        string path = Application.dataPath + _fileName + fileType[(int)_type];
        StreamWriter sw = new StreamWriter(path, false);
        sw.WriteLine(_txt);
        sw.Flush(); sw.Close();
        return true;
    }


    static public string ReadFile(string _fileName, FILETYPE _type = FILETYPE.JSON) {
        StreamReader sr = new StreamReader(Application.dataPath + _fileName + _type, false);
        string data = sr.ReadToEnd();
        sr.Close();

        return data;
    }
}