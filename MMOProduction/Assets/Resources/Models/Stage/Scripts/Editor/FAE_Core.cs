// Fantasy Adventure Environment
// Staggart Creations
// http://staggart.xyz

using UnityEngine;
using System.IO;

//Make this entire class is editor-only without requiring it to be in an "Editor" folder
#if UNITY_EDITOR
using UnityEditor;

namespace FAE
{
    public class FAE_Core : Editor
    {
        public const string ASSET_NAME = "Fantasy Adventure Environment";
        public const string ASSET_ABRV = "FAE";
        public const string ASSET_ID = "70354";

        public const string PACKAGE_VERSION = "20171";
        public static string INSTALLED_VERSION = "1.4.1";
        public const string MIN_UNITY_VERSION = "2017.1";

        public static string DOC_URL = "http://staggart.xyz/unity/fantasy-adventure-environment/fae-documentation/";
        public static string FORUM_URL = "https://forum.unity3d.com/threads/486102";

        public static void OpenStorePage()
        {
            Application.OpenURL("com.unity3d.kharma:content/" + ASSET_ID);
        }

        public static string PACKAGE_ROOT_FOLDER
        {
            get { return SessionState.GetString(ASSET_ABRV + "_BASE_FOLDER", string.Empty); }
            set { SessionState.SetString(ASSET_ABRV + "_BASE_FOLDER", value); }
        }

        public static string GetRootFolder()
        {
            //Get script path
            string[] scriptGUID = AssetDatabase.FindAssets("FAE_Core t:script");
            string scriptFilePath = AssetDatabase.GUIDToAssetPath(scriptGUID[0]);

            //Truncate to get relative path
            PACKAGE_ROOT_FOLDER = scriptFilePath.Replace("Scripts/Editor/FAE_Core.cs", string.Empty);

#if WB_DEV
            Debug.Log("<b>Package root</b> " + PACKAGE_ROOT_FOLDER);
#endif

            return PACKAGE_ROOT_FOLDER;
        }
    }

    public class FAE_Window : EditorWindow
    {
        //Window properties
        private static int width = 440;
        private static int height = 300;

        private bool isTabGettingStarted = true;
        private bool isTabSupport = false;


        [MenuItem("Help/Fantasy Adventure Environment", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = EditorWindow.GetWindow<FAE_Window>(false, "About", true);
            editorWindow.titleContent = new GUIContent("Help " + FAE_Core.INSTALLED_VERSION);
            editorWindow.autoRepaintOnSceneChange = true;

            //Open somewhat in the center of the screen
            editorWindow.position = new Rect((Screen.width) / 2f, (Screen.height) / 2f, width, height);

            //Fixed size
            editorWindow.maxSize = new Vector2(width, height);
            editorWindow.minSize = new Vector2(width, 200);

            Init();

            editorWindow.Show();

        }

        private void SetWindowHeight(float height)
        {
            this.maxSize = new Vector2(width, height);
            this.minSize = new Vector2(width, height);
        }

        //Store values in the volatile SessionState
        static void Init()
        {
            GetRootFolder();
        }

        public static void GetRootFolder()
        {
            //Get script path
            string[] scriptGUID = AssetDatabase.FindAssets("FAE_CORE t:script");
            string scriptFilePath = AssetDatabase.GUIDToAssetPath(scriptGUID[0]);

            //Truncate to get relative path
            string PACKAGE_ROOT_FOLDER = scriptFilePath.Replace("Scripts/Editor/FAE_Core.cs", string.Empty);

            SessionState.SetString("PATH", PACKAGE_ROOT_FOLDER);
        }

        void OnGUI()
        {

            DrawHeader();

            GUILayout.Space(5);
            DrawTabs();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (isTabGettingStarted) DrawGettingStarted();

            if (isTabSupport) DrawSupport();

            //DrawActionButtons();

            EditorGUILayout.EndVertical();

            DrawFooter();

        }

        void DrawHeader()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b><size=24>Fantasy Adventure Environment</size></b>", Header);

            GUILayout.Label("Version: " + FAE_Core.INSTALLED_VERSION, Footer);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal();


            if (GUILayout.Toggle(isTabGettingStarted, "Getting started", Tab))
            {
                isTabGettingStarted = true;
                isTabSupport = false;
            }

            if (GUILayout.Toggle(isTabSupport, "Support", Tab))
            {
                isTabGettingStarted = false;
                isTabSupport = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        void DrawGettingStarted()
        {
            SetWindowHeight(335);

            EditorGUILayout.HelpBox("Please view the documentation for further details about this package and its workings.", MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("<b><size=16>Online documentation</size></b>\n<i>Set up, best practices and troubleshooting</i>", Button))
            {
                Application.OpenURL(FAE_Core.DOC_URL + "#getting-started-3");
            }

        }

        void DrawSupport()
        {
            SetWindowHeight(350f);

            EditorGUILayout.BeginVertical(); //Support box

            EditorGUILayout.HelpBox("If you have any questions, or ran into issues, please get in touch!", MessageType.Info);

            EditorGUILayout.Space();

            //Buttons box
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<b><size=12>Email</size></b>\n<i>Contact</i>", Button))
            {
                Application.OpenURL("mailto:contact@staggart.xyz");
            }
            if (GUILayout.Button("<b><size=12>Twitter</size></b>\n<i>Follow developments</i>", Button))
            {
                Application.OpenURL("https://twitter.com/search?q=staggart%20creations");
            }
            if (GUILayout.Button("<b><size=12>Forum</size></b>\n<i>Join the discussion</i>", Button))
            {
                Application.OpenURL(FAE_Core.FORUM_URL);
            }
            EditorGUILayout.EndHorizontal();//Buttons box

            EditorGUILayout.EndVertical(); //Support box
        }

        private void DrawActionButtons()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();


            if (GUILayout.Button("<size=12>Rate</size>", Button))
                Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/account/downloads/search=");

            if (GUILayout.Button("<size=12>Review</size>", Button))
                Application.OpenURL("");


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawFooter()
        {
            //EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            GUILayout.Label("- Staggart Creations -", Footer);
        }

        #region Styles

        private static GUIStyle _Footer;
        public static GUIStyle Footer
        {
            get
            {
                if (_Footer == null)
                {
                    _Footer = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        wordWrap = true,
                        fontSize = 12
                    };
                }

                return _Footer;
            }
        }

        private static GUIStyle _Button;
        public static GUIStyle Button
        {
            get
            {
                if (_Button == null)
                {
                    _Button = new GUIStyle(GUI.skin.button)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Button;
            }
        }

        private static GUIStyle _Header;
        public static GUIStyle Header
        {
            get
            {
                if (_Header == null)
                {
                    _Header = new GUIStyle(GUI.skin.label)
                    {
                        richText = true,
                        alignment = TextAnchor.MiddleCenter,
                        wordWrap = true,
                        fontSize = 18,
                        fontStyle = FontStyle.Bold
                    };
                }

                return _Header;
            }
        }

        private static Texture _HelpIcon;
        public static Texture HelpIcon
        {
            get
            {
                if (_HelpIcon == null)
                {
                    _HelpIcon = EditorGUIUtility.FindTexture("d_UnityEditor.InspectorWindow");
                }
                return _HelpIcon;
            }
        }


        private static GUIStyle _Tab;
        public static GUIStyle Tab
        {
            get
            {
                if (_Tab == null)
                {
                    _Tab = new GUIStyle(EditorStyles.miniButtonMid)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Tab;
            }
        }

        #endregion //Stylies
    }//Window Class
}//namespace
#endif //If Unity Editor