using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameScenes
{
    private readonly static string s_MainScene = "Main";
    private readonly static string s_LoadingScene = "Loading";

    public static AsyncOperation LoadMainScene()
    {
         return LoadScene(s_MainScene, true);
    }

    public static void ReloadScene(bool async = false)
    {
        //LoadScene(SceneManager.GetActiveScene().name, async);
        LoadScene(s_LoadingScene, async);
    }

    private static AsyncOperation LoadScene(string sceneName, bool async = false)
    {
        if (async)
            return SceneManager.LoadSceneAsync(sceneName);
        else
            SceneManager.LoadScene(sceneName);

        return null;
    }
}
