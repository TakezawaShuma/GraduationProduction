using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class skill_Editor : EditorWindow
{
    static skill_table tablelist;
    Vector2 rightScrollPos = Vector2.zero;
    static List<bool> delete_flag = new List<bool>();
    static skill_Editor window = null;
    /// <summary>
    /// window作成
    /// </summary>
    [MenuItem("GameSetting/Skill")]
    private static void Open()
    {
        tablelist = Resources.Load<skill_table>("GameData\\skill_data");
        // 生成
        window = GetWindow<skill_Editor>("skill_setting");
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
            EditorGUILayout.LabelField("名前", GUILayout.Width(35));
            table.name = EditorGUILayout.TextArea(table.name, GUILayout.Width(100));
            EditorGUILayout.LabelField("", GUILayout.Width(50));
            EditorGUILayout.LabelField("エフェクトID", GUILayout.Width(80));
            table.effectId = EditorGUILayout.IntField(table.effectId, GUILayout.Width(40));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("最大Lv", GUILayout.Width(50));
            table.maxLv = EditorGUILayout.IntField(table.maxLv, GUILayout.Width(40));

            EditorGUILayout.LabelField("親ID", GUILayout.Width(40));
            table.pearentID = EditorGUILayout.IntField(table.pearentID, GUILayout.Width(40));

            EditorGUILayout.LabelField("取得必要ポイント", GUILayout.Width(100));
            table.pearentPoint = EditorGUILayout.IntField(table.pearentPoint, GUILayout.Width(40));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("効果時間", GUILayout.Width(50));
            table.time = EditorGUILayout.IntField(table.time, GUILayout.Width(40));

            EditorGUILayout.LabelField("ヘイト値", GUILayout.Width(50));
            table.hate = EditorGUILayout.IntField(table.hate, GUILayout.Width(40));

            EditorGUILayout.LabelField("武器", GUILayout.Width(50));
            table.weapon = EditorGUILayout.IntField(table.weapon, GUILayout.Width(40));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ターゲット種類 : ", GUILayout.Width(60));
            table.targetType = (target_type)EditorGUILayout.EnumPopup(table.targetType, GUILayout.Width(100));

            EditorGUILayout.LabelField("範囲種類 : ", GUILayout.Width(60));
            table.rangeType = (range_type)EditorGUILayout.EnumPopup(table.rangeType, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.LabelField("影響ステータス", GUILayout.Width(80));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            string[] nameTable = { "HP", "MP", "STR", "VIT", "INT", "MND", "DEX", "AGI", "状態異常", "威力" };
            int[] statusTable = { 0,0,0,0,0,0,0,0,0,0 };
            for (int j = 0; j < 5; j++)
            {
                EditorGUILayout.LabelField(nameTable[j].ToString(), GUILayout.Width(50));
                statusTable[j] = EditorGUILayout.IntField(statusTable[j], GUILayout.Width(30));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            for (int j = 5; j < 10; j++)
            {
                EditorGUILayout.LabelField(nameTable[j].ToString(), GUILayout.Width(50));
                statusTable[j] = EditorGUILayout.IntField(statusTable[j], GUILayout.Width(30));
            }

            table.hp = statusTable[0];
            table.mp = statusTable[1];
            table.str = statusTable[2];
            table.vit = statusTable[3];
            table.inte = statusTable[4];
            table.mnd = statusTable[5];
            table.dex = statusTable[6];
            table.agi = statusTable[7];
            table.effect = statusTable[8];
            table.condition = statusTable[9];



            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("説明文");
            table.detail = EditorGUILayout.TextArea(table.detail, GUILayout.Width(430));

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
            tablelist.tables.Add(new skill_table.skill_data());
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
        StreamWriter sw = new StreamWriter("../skilldata.txt", false);
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();

        Debug.Log(txt);
    }
}
