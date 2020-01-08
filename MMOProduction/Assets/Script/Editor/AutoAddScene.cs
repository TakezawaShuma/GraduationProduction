using System;
using System.IO;
using UnityEditor;

public class AutoAddScene
{
    [MenuItem("Assets/Create/Empty Scene")]
    private static void CreateEmptyScene()
    {
        CreateScene(
            "New Empty Sene",
            EditorApplication.NewEmptyScene
        );
    }

    [MenuItem("Assets/Create/Scene")]
    private static void CreateScene()
    {
        CreateScene(
            "New Sene",
            EditorApplication.NewScene
        );
    }

    private static void CreateScene(
        string filenameWithoutExtension,
        Action newSceneCallback
    )
    {
        var filename = filenameWithoutExtension + ".unity";
        var path = GetPath() + "/" + filename;
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        newSceneCallback();

        EditorApplication.SaveScene(path);

        var scenes = EditorBuildSettings.scenes;
        ArrayUtility.Add(
            ref scenes,
            new EditorBuildSettingsScene(path, true)
        );
        EditorBuildSettings.scenes = scenes;
    }

    private static string GetPath()
    {
        var instanceId = Selection.activeInstanceID;
        var path = AssetDatabase.GetAssetPath(instanceId);
        path = string.IsNullOrEmpty(path) ? "Assets" : path;

        if (Directory.Exists(path))
        {
            return path;
        }
        if (File.Exists(path))
        {
            var parent = Directory.GetParent(path);
            var fullName = parent.FullName;
            var unixFileName = fullName.Replace("\\", "/");
            return FileUtil.GetProjectRelativePath(unixFileName);
        }
        return string.Empty;
    }
}
