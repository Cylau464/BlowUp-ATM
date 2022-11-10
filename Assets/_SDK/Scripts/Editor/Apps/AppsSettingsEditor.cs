using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEngine;

namespace apps.editor
{
    [CustomEditor(typeof(AppsSettings))]
    public class AppsSettingsEditor : Editor
    {
        SerializedProperty autoInitialize;


        SerializedProperty integrateFacebook;
        SerializedProperty appLabels;
        SerializedProperty appFacebookID;
        SerializedProperty clientTokens;

        SerializedProperty integrateADS;
        SerializedProperty showADSType;
        SerializedProperty adsInfo;

        SerializedProperty integrateGameAnalytics;
        SerializedProperty integrateAppMetrica;
        SerializedProperty appMetricaInfo;

        SerializedProperty integrateTenjin;
        SerializedProperty tenjinApiKey;

        SerializedProperty debugMode;

        protected void OnEnable()
        {
            autoInitialize = serializedObject.FindProperty("autoInitialize");

            integrateFacebook = serializedObject.FindProperty("integrateFacebook");
            appLabels = serializedObject.FindProperty("appLabels");
            appFacebookID = serializedObject.FindProperty("appFacebookID");
            clientTokens = serializedObject.FindProperty("clientTokens");

            integrateADS = serializedObject.FindProperty("integrateADS");
            showADSType = serializedObject.FindProperty("showADSType");
            adsInfo = serializedObject.FindProperty("adsInfo");
            integrateGameAnalytics = serializedObject.FindProperty("integrateGameAnalytics");
            integrateAppMetrica = serializedObject.FindProperty("integrateAppMetrica");
            appMetricaInfo = serializedObject.FindProperty("appMetricaInfo");

            integrateTenjin = serializedObject.FindProperty("integrateTenjin");
            tenjinApiKey = serializedObject.FindProperty("tenjinApiKey");

            debugMode = serializedObject.FindProperty("debugMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AppsSettings appsSettings = (AppsSettings)target;

            GUIStyle header = new GUIStyle(GUI.skin.label);
            header.margin = new RectOffset(25, 20, 20, 5);
            header.fontStyle = FontStyle.Bold;
            GUILayout.Label("Auto Apps Initialize", header);
            EditorGUILayout.PropertyField(autoInitialize);

            GUILayout.Label("Facebook Settings", header);

            EditorGUILayout.PropertyField(integrateFacebook);
            if (appsSettings.integrateFacebook)
            {
                EditorGUILayout.PropertyField(appLabels);
                EditorGUILayout.PropertyField(appFacebookID);
                EditorGUILayout.PropertyField(clientTokens);
            }

            GUILayout.Label("Ads Settings", header);
            EditorGUILayout.PropertyField(integrateADS);
            if (appsSettings.integrateADS)
            {
                EditorGUILayout.PropertyField(showADSType);
                EditorGUILayout.PropertyField(adsInfo);
            }

            GUILayout.Label("GameAnalytics settings", header);
            EditorGUILayout.PropertyField(integrateGameAnalytics);

            GUILayout.Label("AppMetrica Settings", header);
            EditorGUILayout.PropertyField(integrateAppMetrica);
            if (appsSettings.integrateAppMetrica)
            {
                EditorGUILayout.PropertyField(appMetricaInfo);
            }

            GUILayout.Label("Tenjin Settings", header);
            EditorGUILayout.PropertyField(integrateTenjin);
            if (appsSettings.integrateTenjin)
            {
                EditorGUILayout.PropertyField(tenjinApiKey);
            }

            GUILayout.Label("Other Settings", header);
            EditorGUILayout.PropertyField(debugMode);

            GUILayout.Label("Player Information", header);
            GUIStyle headInfo = new GUIStyle(GUI.skin.label);
            headInfo.margin = new RectOffset(40, 0, 0, 0);

            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Package name:   " + PlayerSettings.applicationIdentifier, headInfo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Default orientation:   " + PlayerSettings.defaultInterfaceOrientation, headInfo);
            GUILayout.EndHorizontal();


            GUILayout.Label("Android", headInfo);

            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Min sdk version:   " + PlayerSettings.Android.minSdkVersion, headInfo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Target sdk version:   " + PlayerSettings.Android.targetSdkVersion, headInfo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Target architectures:   " + PlayerSettings.Android.targetArchitectures, headInfo);
            GUILayout.EndHorizontal();

            GUILayout.Label("IOS", headInfo);

            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Target device:   " + PlayerSettings.iOS.targetDevice, headInfo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("box");
            int num = PlayerSettings.GetArchitecture(BuildTargetGroup.iOS);
            string architecture = (num == 0) ? "Armv7" : (num == 1) ? "Arm64" : "Universal";
            GUILayout.Label("Architecture:   " + architecture, headInfo);
            GUILayout.EndHorizontal();

            GUIStyle headButton = new GUIStyle(GUI.skin.button);
            headButton.margin = new RectOffset(0, 0, 20, 0);
            headButton.fixedHeight = 50;
            headButton.fontStyle = FontStyle.Bold;

            if (GUILayout.Button("Save And Refresh", headButton))
            {
                RefreshFacebookSettings(appsSettings);
                ManifestMod.GenerateManifest();
                AppsUtility.SaveData(target);
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        public static void RefreshFacebookSettings(AppsSettings settings)
        {
            if (ScriptableObjectUtility.GetScriptableObjects<FacebookSettings>().Length == 0)
            {
                AppsUtility.CreateAsset<FacebookSettings>(
                    "Assets/" + FacebookSettings.FacebookSettingsPath + "/",
                    FacebookSettings.FacebookSettingsAssetName + FacebookSettings.FacebookSettingsAssetExtension
                    );
            }

            FacebookSettings.AppIds[0] = settings.appFacebookID;
            FacebookSettings.AppLabels[0] = settings.appLabels;
            FacebookSettings.ClientTokens[0] = settings.clientTokens;
        }

        public static AppsSettings GetAppSettings()
        {
            AppsSettings[] appsSettings = ScriptableObjectUtility.GetScriptableObjects<AppsSettings>();
            if (appsSettings == null || appsSettings.Length <= 0)
            {
                appsSettings = new AppsSettings[]
                {
                    AppsUtility.CreateAsset<AppsSettings>(AppsSettings.globalDirectoryPath, "AppsSettings.asset")
                };

                Debug.Log("The appsSettings is created...");
            }

            if (1 < appsSettings.Length)
            {
                Debug.LogWarning("You have 2 or plus AppsSettings, please leave just one for be sure that we use the right one!...");
            }

            return appsSettings[0];
        }

        public static void PingAppsSettings(AppsSettings settings)
        {
            if (settings == null)
                throw new System.ArgumentNullException("The AppsSettings has null variable.");

            AppsUtility.SaveData(settings);
            AppsUtility.PingObject(settings);
        }
    }
}