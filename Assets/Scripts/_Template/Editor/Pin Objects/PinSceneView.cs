using UnityEditor;
using UnityEngine;

namespace editor.pin
{
    public class PinSceneView : Editor
    {
        private const float k_Width = 300f;
        private const int k_Height = 20;
        private const int k_Padding = 20;

        private static bool s_IsInited = false;

        internal static void OnInitializeProject()
        {
            if (!s_IsInited)
            {
                SceneView.duringSceneGui += UpdateSceneView;
                s_IsInited = true;
            }
        }

        private static void UpdateSceneView(SceneView sceneView)
        {
            Handles.BeginGUI();

            int j = 0;
            Object[] pinObjects = PinListInfo.GetAllPinObjects();
            for (int i = 0; i < pinObjects.Length; i++)
            {
                if (pinObjects[i] == null) continue;

                if (GUI.Button(new Rect(sceneView.camera.pixelWidth - 95, 120 + (k_Padding * j), 80f, 20f), pinObjects[i].name, GetButtonStyle()))
                {
                    Debug.Log(pinObjects[i]);

                    AssetUtility.PingObject(pinObjects[i]);
                }

                if (GUI.Button(new Rect(sceneView.camera.pixelWidth - 120, 120 + (k_Padding * j), 20f, 20f), "X", GetButtonStyle()))
                {
                    PinListInfo.RemovePinObject(pinObjects[i]);
                }
                j++;
            }

            Handles.EndGUI();
        }

        private static GUIStyle GetButtonStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleLeft;
            return style;
        }
    }

}