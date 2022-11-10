#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace editor
{
    public static class AssetUtility
    {
        private static string FilePath(string directoryPath, string assetsName) => directoryPath + assetsName;

        public static T[] FindScribtableObjectsOfType<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            if (guids == null || guids.Length == 0)
                return null;

            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static T FindScribtableObjectOfType<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            if (guids == null || guids.Length == 0)
                return null;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        /// Search for the asset type of T if we find we will return it if no we will create a new one.
        /// 
        public static T GetOrCreateAsset<T>(string directoryPath, string assetsName) where T : ScriptableObject
        {
            var settings = AssetDatabase.LoadAssetAtPath<T>(FilePath(directoryPath, assetsName));
            if (settings == null)
            {
                settings = CreateAsset<T>().SaveAsset(directoryPath, assetsName);
            }
            return settings;
        }

        public static T CreateAsset<T>() where T : ScriptableObject
        {
            return ScriptableObject.CreateInstance<T>();
        }

        public static T SaveAsset<T>(this T t, string directoryPath, string assetsName) where T : ScriptableObject
        {

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            AssetDatabase.CreateAsset(t, FilePath(directoryPath, assetsName));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return t;
        }

        public static void PingObject<T>(this T target) where T : Object
        {
            if (target == null) throw new System.ArgumentNullException("The Object has null variable.");

            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }
}
#endif