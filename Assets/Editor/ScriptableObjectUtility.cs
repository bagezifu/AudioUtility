using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using KLFrame;

public static class  ScriptableObjectUtility  {

    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }


        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/创建声音资源文件")]
    public static void CreateAudioAsset()
    {
        CreateAsset<AudioAsset>();
    }
    [MenuItem("Assets/Create/创建声音配置文件")] 
    public static void CreateAudioConfigAsset()
    {
        CreateAsset<AudioConfig>();
    }
}
