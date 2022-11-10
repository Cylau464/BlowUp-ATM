using UnityEditor;
using UnityEngine;

namespace editor.pin
{
    public static class PinMaker
    {
        internal static void OnInitializeProject()
        {

        }

        [MenuItem("Assets/Add To Pin", false, 19)]
        public static void AddToPin()
        {
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                PinListInfo.AddPinObject(obj);
            }

            SceneView.RepaintAll();
        }
    }
}