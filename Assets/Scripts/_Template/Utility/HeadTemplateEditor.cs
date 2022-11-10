#if UNITY_EDITOR
using core;
using UnityEditor;
using UnityEngine;

namespace editor
{
    public class HeadTemplateEditor
    {
        [MenuItem("DSOneGames/Reset Data", false, 0)]
        public static void ResetAllData()
        {
            PlayerPrefs.DeleteAll();
            data.ObjectSaver.ClearAllFiles();

            IResetData[] reseters = EditorManager.FindAllAssetsOfType<IResetData>();

            foreach (IResetData reset in reseters)
            {
                reset.ResetData();
            }

            EditorManager.SaveGame();
        }

        [MenuItem("DSOneGames/Validate Settings", false, 0)]
        public static void ValidateAll()
        {
            IValidate[] validates = EditorManager.FindAllAssetsOfType<IValidate>();
            for (int i = 0; i < validates.Length; i++)
            {
                validates[i].Validate();
            }

            EditorManager.SaveGame();
        }

        [MenuItem("DSOneGames/Save Project", false, 150)]
        public static void SaveProject()
        {
            IAsset[] validates = EditorManager.FindAllAssetsOfType<IAsset>();
            for (int i = 0; i < validates.Length; i++)
            {
                if (validates[i] != null) EditorUtility.SetDirty(validates[i] as Object);
            }

            EditorManager.SaveGame();

            for (int i = 0; i < validates.Length; i++)
            {
                if (validates[i] != null) EditorUtility.ClearDirty(validates[i] as Object);
            }
        }
    }
}
#endif