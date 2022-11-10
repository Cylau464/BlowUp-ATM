using engine.utility;
using System.Collections.Generic;
using UnityEditor;

namespace editor.pin
{
    public static class PinListInfo
    {
        [System.Serializable]
        public class PinInfo
        {
            public string path;

            public PinInfo(UnityEngine.Object obj)
            {
                path = AssetDatabase.GetAssetPath(obj);
            }
        }

        [System.Serializable]
        public class ListPin
        {
            public List<PinInfo> list;

            public ListPin()
            {
                list = new List<PinInfo>();
            }
        }


        private static readonly string s_PinsItem = "TEMP_ITEM_PINS";

        [UnityEngine.SerializeField] private static ListPin s_Pins;
        private static List<UnityEngine.Object> m_PinObjects;

        internal static void OnInitializeProject()
        {
            s_Pins = new ListPin();

            LoadPaths();

            Refresh();
        }

        private static void LoadPaths()
        {
            if (EditorPrefs.HasKey(s_PinsItem))
            {
                JsonConverter.JsonToObject(s_Pins, EditorPrefs.GetString(s_PinsItem));
            }
        }

        private static void Save()
        {
            EditorPrefs.SetString(s_PinsItem, JsonConverter.ObjectToJson(s_Pins));
            Refresh();
        }


        private static void Refresh()
        {
            m_PinObjects = new List<UnityEngine.Object>();

            for (int i = 0; i < s_Pins.list.Count; i++)
            {
                if (s_Pins.list[i] == null)
                {
                    s_Pins.list.RemoveAt(i);
                    i--;
                }

                m_PinObjects.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s_Pins.list[i].path));
            }
        }

        public static bool AddPinObject(UnityEngine.Object obj)
        {
            if (s_Pins.list.Exists((info) =>
                {
                    if (info.path == AssetDatabase.GetAssetPath(obj))
                        return true;

                    return false;
                }))
                return false;

            s_Pins.list.Add(new PinInfo(obj));

            Save();
            return true;
        }

        public static void RemovePinObject(UnityEngine.Object obj)
        {
            s_Pins.list.RemoveAll((info) =>
            {
                if (info.path == AssetDatabase.GetAssetPath(obj))
                    return true;

                return false;
            });

            Save();
        }

        public static void RemoveAllPinObjects()
        {
            s_Pins.list.Clear();

            Save();
        }

        public static UnityEngine.Object[] GetAllPinObjects()
        {
            return m_PinObjects.ToArray();
        }

        public static void DebugAllPins()
        {
            foreach (PinInfo info in s_Pins.list)
            {
                UnityEngine.Debug.Log(info.path);
            }
        }
    }
}