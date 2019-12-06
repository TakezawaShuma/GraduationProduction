///
/// ReferenceExplorer
/// 
/// Copyright (c) 2014 Tatsuhiko Yamamura
/// Released under the MIT license
// / http://opensource.org/licenses/mit-license.php
/// 

#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReferenceExplorer
{
    /// <summary>
    /// シーン内のクラス群を特定キーワードで検索するエディタウィンドウ
    /// </summary>
    public class SearchCodeInSceneWindow : EditorWindow
    {
        /// <summary>
        /// 検索する文字列
        /// </summary>
        protected string searchText = string.Empty;

        /// <summary>
        /// 検索キーワードにヒットしたMonobehaviour一覧
        /// </summary>
        List<CodeObject> ExistCodeObjectList = new List<CodeObject>();

        /// <summary>
        /// 選択中のMonobehaviour
        /// </summary>
        int current = 0;

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem("Window/ReferenceExplorer/SearchCode")]
        static void Open()
        {
            SearchCodeInSceneWindow.GetWindow<SearchCodeInSceneWindow>("code search");
        }

        /// <summary>
        /// ContainCodeObjectを更新する
        /// </summary>
        void UpdateContainCodeObjectList()
        {
            ExistCodeObjectList.Clear();

            var uniqueMonobehaviour = SceneData.GetUniqueMonobehaviour(SceneData.SelectedObjects);

            foreach (var monobehaviour in uniqueMonobehaviour)
            {
                var monoscript = MonoScript.FromMonoBehaviour(monobehaviour);

                bool isMatch = false;

                List<string> lines = new List<string>();
                foreach (var line in monoscript.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {

                    if (!SceneData.Match(line, searchText))
                        continue;

                    lines.Add(line);
                    isMatch = true;
                }

                if (isMatch)
                {
                    ExistCodeObjectList.Add(new CodeObject()
                    {
                        componentType = monobehaviour.GetType(),
                        monoscript = monoscript,
                        lines = lines.ToArray(),
                    });
                }
            }
        }

        #region GUI

        /// <summary>
        /// スクロールビューの座標
        /// </summary>
        Vector2 componentScroll, textScroll;

        /// <summary>
        /// サーチバーの表示を行う。OnGUIで使用する想定
        /// </summary>
        void SearchBar()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            SceneData.IsSelected = GUILayout.Toggle(SceneData.IsSelected, "Selection", EditorStyles.toolbarButton, GUILayout.Width(60));
            SceneData.IsRegex = GUILayout.Toggle(SceneData.IsRegex, "Regex", EditorStyles.toolbarButton, GUILayout.Width(40));
            searchText = GUILayout.TextField(searchText, GUILayout.Width(Screen.width - 110));

            if (EditorGUI.EndChangeCheck())
            {
                UpdateContainCodeObjectList();
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// コンポーネントの一覧を表示する。OnGUIで使用する想定
        /// </summary>
        void ContentView()
        {
            var containCodeObjects = ExistCodeObjectList.ToArray();

            componentScroll = EditorGUILayout.BeginScrollView(componentScroll);

            for (int i = 0; i < containCodeObjects.Length; i++)
            {
                var containCodeObject = containCodeObjects[i];
                EditorGUILayout.BeginHorizontal();


                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Toggle(i == current, EditorStyles.radioButton, GUILayout.Width(14));
                if (EditorGUI.EndChangeCheck())
                {
                    current = i;
                }
                EditorGUILayout.ObjectField(containCodeObject.monoscript, typeof(MonoScript));
                EditorGUILayout.EndHorizontal();

                if (i == current)
                {
                    GUILayout.Space(4);
                    var components = SceneData.SelectedMonobehaviours.FindAll(item => item.GetType() == containCodeObject.componentType);

                    EditorGUI.indentLevel = 2;
                    foreach (var obj in components)
                    {
                        EditorGUILayout.ObjectField(obj.gameObject, typeof(GameObject));
                    }
                    GUILayout.Space(4);
                    EditorGUI.indentLevel = 0;
                }
            }

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 選択中のコンポーネントの検索結果を表示する。OnGUIで使用する想定
        /// </summary>
        void CodeLineView()
        {
            if (ExistCodeObjectList.Count <= current)
                return;

            var currentContainCodeObject = ExistCodeObjectList[current];
            var textHeight = (currentContainCodeObject.lines.Length + 1) * 18;
            var windowHeight = Mathf.Min(100, textHeight);

            GUILayout.BeginArea(new Rect(0, Screen.height - 120, Screen.width, 100));

            textScroll = EditorGUILayout.BeginScrollView(textScroll);
            EditorGUILayout.BeginVertical("box", GUILayout.Height(windowHeight));


            foreach (var line in currentContainCodeObject.lines)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(line);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        #endregion

        #region EditorWindowCallback

        /// <summary>
        /// Unity Callback OnGUI
        /// </summary>
        void OnGUI()
        {
            SearchBar();
            ContentView();
            CodeLineView();
        }

        /// <summary>
        /// Unity Callback OnGUI
        /// </summary>
        void OnSelectionChange()
        {
            if (!SceneData.IsSelected)
                return;

            UpdateContainCodeObjectList();
            Repaint();
        }

        #endregion

        /// <summary>
        /// 検索して見つかったコンポーネントを格納する
        /// </summary>
        class CodeObject
        {
            /// <summary> Monobehaviourのタイプ </summary>
            public Type componentType;
            /// <summary> Monobehaviourのコード </summary>
            public MonoScript monoscript;
            /// <summary> 一致したライン </summary>
            public string[] lines;
        }
    }
}