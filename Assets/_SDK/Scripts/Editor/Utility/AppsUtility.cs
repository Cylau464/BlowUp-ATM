using System.IO;
using UnityEditor;
using UnityEngine;

namespace apps.editor
{
    public static class AppsUtility
    {
        public static T CreateAsset<T>(string directoryPath, string assetsName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            AssetDatabase.CreateAsset(asset, directoryPath + assetsName);
            AssetDatabase.SaveAssets();
            return asset;
        }

        public static void SaveData(Object target)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void PingObject(Object target)
        {
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }
}