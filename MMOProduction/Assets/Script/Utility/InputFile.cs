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

public class InputFile : MonoBehaviour
{
    static private string[] fileType = { ".json", ".txt" };

    /// <summary>
    /// ファイルの書き出し
    /// </summary>
    /// <returns></returns>
    static public bool WriterJson(string _fileName, string _txt, FILETYPE _type)
    {
        StreamWriter sw = new StreamWriter("./Assets/SettingText/" + _fileName + fileType[(int)_type], false);
        sw.WriteLine(_txt);
        sw.Flush(); sw.Close();
        //        AssetDatabase.Refresh();
        return true;
    }


    static public string ReadFile(string _fileName) {
        StreamReader sr = new StreamReader("./ Assets / SettingText /" + _fileName, false);
        string data = sr.ReadToEnd();
        sr.Close();

        return data;
    }
}