using UnityEditor;

namespace editor.pin
{
    [InitializeOnLoad]
    sealed class InitializePin : Editor
    {
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            PinListInfo.OnInitializeProject();
            PinMaker.OnInitializeProject();
            PinSceneView.OnInitializeProject();
        }
    }
}