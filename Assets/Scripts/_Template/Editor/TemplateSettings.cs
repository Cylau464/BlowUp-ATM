using UnityEditor;
using UnityEngine;

namespace editor
{
    public class TemplateSettings : ScriptableObject
    {
        public const string k_SettingsPath = "Assets/Scripts/_Template/Editor";
        public const string k_SettingsName = "TemplateSettings.asset";

        public static TemplateSettings GetOrCreateSettings()
        {
            return AssetUtility.GetOrCreateAsset<TemplateSettings>(k_SettingsPath, k_SettingsName);
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}