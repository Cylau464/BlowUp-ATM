using editor.pin;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace editor
{
    public static class ProjectSettingsTemplate
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Template", SettingsScope.Project)
            {
                label = "Template",

                guiHandler = (searchContext) =>
                {
                    var settings = TemplateSettings.GetSerializedSettings();

                    GUIStyle styleButton = GUIButtonStyle();

                    if (GUILayout.Button("Remove All Pins", styleButton))
                    {
                        PinListInfo.RemoveAllPinObjects();
                    }

                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                keywords = new HashSet<string>(new[] { "Pin", "Template, Settings" })
            };

            return provider;
        }

        private static GUIStyle GUIButtonStyle()
        {
            GUIStyle headButton = new GUIStyle(GUI.skin.button);
            headButton.fontStyle = FontStyle.Bold;

            return headButton;
        }
    }
}