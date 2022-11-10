using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace apps.editor
{
    public static class HeadAppsWindow
    {
        [MenuItem("DSOneGames/Ping AppsSettings", false, 101)]
        public static void AppSettingsPing()
        {
            AppsSettingsEditor.PingAppsSettings(AppsSettingsEditor.GetAppSettings());
        }

        //[MenuItem("DSOneGames/New DSOneGames GO", false, 110)]
        //public static void CreateDSOneGamesGO()
        //{
        //    if (Object.FindObjectOfType<AppsIntegration>() != null)
        //    {
        //        Debug.LogError("AppsManager object already exist on the scene...");
        //        return;
        //    }

        //    GameObject dsOneGamesGO = Resources.Load("DSOneGames GO") as GameObject;
        //    if (dsOneGamesGO != null)
        //    {
        //        (PrefabUtility.InstantiatePrefab(dsOneGamesGO) as GameObject).transform.SetAsLastSibling();
        //    }
        //    else
        //    {
        //        Debug.LogError("DSOneGames GO not found in the path: " + AppsSettings.globalDirectoryPath);
        //    }

        //    SaveAllActiveScenes();
        //}

        public static void SaveAllActiveScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                EditorSceneManager.SaveScene(SceneManager.GetSceneAt(i));
            }
        }
    }
}