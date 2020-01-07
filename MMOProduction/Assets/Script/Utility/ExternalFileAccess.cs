using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;


public static class ExternalFileAccess 
{
    // ファイルに追加
    public static void Additional(string _path,string _data)
    {
        StreamWriter sw = new StreamWriter(_path, true);
        sw.Write(_data);
        sw.Flush();
        sw.Close();
    }

    // ファイルを上書き
    public static void Overwrite(string _path,string _data)
    {
        StreamWriter sw = new StreamWriter(_path, false);
        sw.Write(_data);
        sw.Flush();
        sw.Close();
    }

    // ファイルの読み込み
    public static string ReadFile(string _path)
    {
        string ret="";
        FileInfo fi = new FileInfo(_path);
        try
        {
            using(StreamReader sr=new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                ret=sr.ReadToEnd();
            }
        }catch(Exception e)
        {
            ret += SetDefaultText();
        }
        return ret;
    }

    private static string SetDefaultText()
    {
        return "C#あ\n";
    }
}
