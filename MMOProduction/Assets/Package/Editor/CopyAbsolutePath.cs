using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class CopyAbsolutePath : MonoBehaviour
{

    [MenuItem("Assets/絶対パスをクリップボードにコピー", false)]
    static void Execute()
    {
        // get select GO full path
        int instanceID = Selection.activeInstanceID;
        string path = AssetDatabase.GetAssetPath(instanceID);
        //string fullPath = System.IO.Path.GetFullPath(path);

        // copy clipboard
        GUIUtility.systemCopyBuffer = path;
        Debug.Log("Copy clipboard : \n" + path);
    }
}