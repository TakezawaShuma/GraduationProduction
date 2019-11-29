///
/// ReferenceExplorer
/// 
/// Copyright (c) 2014 Tatsuhiko Yamamura
/// Released under the MIT license
// / http://opensource.org/licenses/mit-license.php
/// 

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
    /// シーン全体の情報を格納する
    /// </summary>
    public class SceneData
    {
        /// <summary>
        /// Initializes the <see cref="ReferenceExplorer.SceneData"/> class.
        /// </summary>
        static SceneData()
        {
            EditorApplication.update += () => {
                frameCount++;
            };
            IsSelected = false;
            IsRegex = false;
        }

        /// <summary>
        /// 検索範囲を選択中のオブジェクトに限定する
        /// </summary>
        public static bool IsSelected { get; set; }

        /// <summary>
        /// 検索に正規表現を使用する
        /// </summary>
        public static bool IsRegex { get; set; }

        /// <summary>
        /// 検索に使用する全てのMonobehaviour
        /// </summary>
        static List<MonoBehaviour> allMonobehaviourList = new List<MonoBehaviour>();

        /// <summary>
        /// シーン内の全オブジェクト
        /// </summary>
        static List<GameObject> allGameObjectList = new List<GameObject>();

        /// <summary>
        /// エディタのフレームカウント
        /// </summary>
        public static long frameCount { get; private set; }

        /// <summary>
        /// 更新フレーム。frameCountと一致しなければ更新しない等の用途
        /// </summary>
        static long monobehaviourLastUpdateFrame = 0, gameobjectLastUpdateFrame = 0;

        /// <summary>
        /// シーンにある全てのGameObject。isSelectedにチェックがある場合は選択中のオブジェクトを取得する
        /// </summary>
        /// <value>GameObject一覧</value>
        public static List<GameObject> SelectedObjects
        {
            get
            {
                if (IsSelected)
                {
                    return new List<GameObject>(Selection.gameObjects);
                }
                else
                {
                    return AllObjects;
                }
            }
        }

        /// <summary>
        /// シーン内の全コンポーネントを取得する。isSelectedにチェックがある場合は選択中のオブジェクトのコンポーネントを取得する
        /// </summary>
        /// <value>Monobehaviour一覧</value>
        public static List<MonoBehaviour> SelectedMonobehaviours
        {
            get
            {
                var monobehaviourList = new List<MonoBehaviour>();

                if (IsSelected)
                {
                    foreach (var obj in Selection.gameObjects)
                    {
                        monobehaviourList.AddRange(AllComponents.FindAll(item => item.gameObject == obj));
                    }
                    return monobehaviourList;
                }
                else
                {
                    return AllComponents;
                }
            }
        }

        /// <summary>
        /// Gets all objects.
        /// </summary>
        /// <value>All objects.</value>
        public static List<GameObject> AllObjects
        {
            get
            {
                if (frameCount != gameobjectLastUpdateFrame)
                {
                    UpdateAllObjectList();
                    gameobjectLastUpdateFrame = frameCount;
                }

                return new List<GameObject>(allGameObjectList);
            }
        }

        /// <summary>
        /// Gets all component.
        /// </summary>
        /// <value>All component.</value>
        public static List<MonoBehaviour> AllComponents
        {
            get
            {
                if (monobehaviourLastUpdateFrame != frameCount)
                {
                    UpdateAllMonobehaviourList();
                    monobehaviourLastUpdateFrame = frameCount;
                }
                return new List<MonoBehaviour>(allMonobehaviourList);
            }
        }

        /// <summary>
        ///Component一覧を更新する
        /// </summary>
        static void UpdateAllMonobehaviourList()
        {
            allMonobehaviourList.Clear();
            allMonobehaviourList.AddRange(GetComponentsInList<MonoBehaviour>(AllObjects));
        }

        /// <summary>
        /// GameObject一覧（deactive含む）を更新する
        /// </summary>
        static void UpdateAllObjectList()
        {
            allGameObjectList.Clear();
            foreach (GameObject obj in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {

                if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Application.isEditor)
                {
                    string sAssetPath = AssetDatabase.GetAssetPath(obj.transform.root.gameObject);
                    if (!string.IsNullOrEmpty(sAssetPath))
                        continue;
                }

                allGameObjectList.Add(obj);
            }
        }

        public static IEnumerable<T> GetComponentsInList<T>(IEnumerable<GameObject> gameObjects) where T : Component
        {
            var componentList = new List<T>();

            foreach (var obj in gameObjects)
            {
                var component = obj.GetComponent<T>();
                if (component != null)
                {
                    componentList.Add(component);
                }
            }
            return componentList;
        }

        /// <summary>
        /// searchの内容がInputに含まれることを確認する。isRegexにチェックを入れると正規表現で検索する
        /// </summary>
        /// <param name="input">インプット</param>
        /// <param name="search">検索キーワード</param>
        public static bool Match(string input, string search)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (IsRegex)
            {
                var match = Regex.Match(input, search);
                return match.Success;
            }
            else
            {
                var result = input.IndexOf(search);
                return (result != 0 && result != -1);
            }
        }

        /// <summary>
        /// ユニークなMonobehaviourを継承したコンポーネント一覧を取得する
        /// </summary>
        /// <returns>検索するオブジェクト一覧</returns>
        /// <param name="objects">Monobehaviour一覧</param>
        public static List<MonoBehaviour> GetUniqueMonobehaviour(IEnumerable<GameObject> objects)
        {
            var uniqueMonobehaviourList = new List<MonoBehaviour>();
            foreach (var obj in objects)
            {
                foreach (var monobehaviour in obj.GetComponents<MonoBehaviour>())
                {
                    if (!uniqueMonobehaviourList.Exists(item => item.GetType() == monobehaviour.GetType()))
                    {
                        uniqueMonobehaviourList.Add(monobehaviour);
                    }
                }
            }

            return uniqueMonobehaviourList;
        }
    }
}