using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemyEditor : EditorWindow
{
    static enemy_table tablelist;
    Vector2 rightScrollPos = Vector2.zero;
    static List<bool> delete_flag = new List<bool>();
    static EnemyEditor window = null;

    /// <summary>
    /// window作成
    /// </summary>
    [MenuItem("GameSetting/Enemy")]
    private static void Open()
    {
        tablelist = Resources.Load<enemy_table>("GameData\\enemy_data");
        // 生成
        window = GetWindow<EnemyEditor>("enemy_setting");
        window.maxSize = window.minSize = new Vector2(450, 500);
        foreach (var table in tablelist.tables)
        {
            delete_flag.Add(false);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void OnGUI()
    {
        rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos);
        int i = 0;
        foreach (var table in tablelist.tables)
        {
            EditorGUILayout.LabelField("----------------------------------number" + (i + 1).ToString() + "----------------------------------");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID", GUILayout.Width(20));
            table.id = EditorGUILayout.IntField(table.id, GUILayout.Width(40));
            EditorGUILayout.LabelField("モデルID", GUILayout.Width(55));
            table.modelId = EditorGUILayout.IntField(table.modelId, GUILayout.Width(40));
            EditorGUILayout.LabelField("最大HP", GUILayout.Width(50));
            table.maxHp = EditorGUILayout.IntField(table.maxHp, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            if (!delete_flag[i])
            {
                if (GUILayout.Button("削除", GUILayout.Width(80)))
                {
                    delete_flag[i] = true;
                }
            }
            else
            {
                if (GUILayout.Button("OK", GUILayout.Width(40)))
                {
                    tablelist.tables.Remove(table);
                    delete_flag.Remove(delete_flag[i]);
                    break;
                }
                if (GUILayout.Button("NO", GUILayout.Width(40))) delete_flag[i] = false;
            }
            EditorGUILayout.EndHorizontal();
            i++;
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("追加"))
        {
            tablelist.tables.Add(new enemy_table.enemy_data());
            delete_flag.Add(false);
        }

        if (GUILayout.Button("最後尾の削除"))
        {
            tablelist.tables.Remove(tablelist.tables[tablelist.tables.Count - 1]);
            delete_flag.Remove(delete_flag[delete_flag.Count - 1]);
        }

        EditorGUILayout.EndHorizontal();

        // ボタンの配置
        if (GUILayout.Button("保存"))
            textSave(JsonUtility.ToJson(tablelist));
    }

    /// <summary>
    /// 書き出し
    /// </summary>
    /// <param name="txt"></param>
    public void textSave(string txt)
    {
        StreamWriter sw = new StreamWriter("../enemydata.txt", false);
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();

        Debug.Log(txt);
    }
}
