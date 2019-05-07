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
    private static void Open(){
        tablelist = Resources.Load<skill_table>("skill_data");
        // 生成
        window = GetWindow<skill_Editor>("skill_setting");
        window.maxSize = window.minSize = new Vector2 (440, 500);
        foreach(var table in tablelist.tables) {
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
            EditorGUILayout.LabelField("ID : ", GUILayout.Width(20));
            table.id = EditorGUILayout.IntField(table.id, GUILayout.Width(40));
            EditorGUILayout.LabelField("最大Lv : ", GUILayout.Width(40));
            table.max_point = EditorGUILayout.IntField(table.max_point, GUILayout.Width(40));
            EditorGUILayout.LabelField("スキル種類 : ", GUILayout.Width(60));
            table.skillType = (skill_type)EditorGUILayout.EnumPopup(table.skillType, GUILayout.Width(100));

            if (!delete_flag[i]) {
                if(GUILayout.Button("削除",GUILayout.Width(80))) {
                    delete_flag[i] = true;
                }
            }
            else {
                if (GUILayout.Button("OK",GUILayout.Width(40))) {
                    tablelist.tables.Remove(table);
                    delete_flag.Remove(delete_flag[i]);
                    break;
                }
                if (GUILayout.Button("NO", GUILayout.Width(40))) delete_flag[i] = false;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            switch (table.skillType)
            {
                case skill_type.ATTACK:
                    EditorGUILayout.LabelField("威力 : ", GUILayout.Width(40));
                    table.effect = EditorGUILayout.IntField(table.effect, GUILayout.Width(80));
                    break;


                case skill_type.BUFF:
                    EditorGUILayout.LabelField("バフの種類", GUILayout.Width(80));
                    table.buffType = (buff_type)EditorGUILayout.EnumPopup(table.buffType, GUILayout.Width(100));
                    switch (table.buffType)
                    {
                        case buff_type.ATTACK:
                            EditorGUILayout.LabelField("上昇量 : ", GUILayout.Width(40));
                            table.effect = EditorGUILayout.IntField(table.effect, GUILayout.Width(80));
                            break;
                        case buff_type.DEFFENSE:
                            EditorGUILayout.LabelField("上昇量 : ", GUILayout.Width(40));
                            table.effect = EditorGUILayout.IntField(table.effect, GUILayout.Width(80));
                            break;
                        case buff_type.HEEL:
                            EditorGUILayout.LabelField("回復量 : ", GUILayout.Width(40));
                            table.effect = EditorGUILayout.IntField(table.effect, GUILayout.Width(80));
                            break;
                        case buff_type.NOME:
                            EditorGUILayout.LabelField("何を設定すれば・・・・？", GUILayout.Width(160));
                            break;
                        default: break;
                    }
                    break;

                case skill_type.NUME:
                    EditorGUILayout.LabelField("何を設定すれば・・・・？", GUILayout.Width(160));
                    break;

                default: break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("説明文");
            table.description = EditorGUILayout.TextArea(table.description);
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
