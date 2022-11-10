using UnityEditor;
using UnityEngine;

namespace InspectorProgramming.editor
{
    [CustomEditor(typeof(Executor))]
    [CanEditMultipleObjects]
    public class ExecutorEditor : Editor
    {
        SerializedProperty type;
        SerializedProperty action;
        SerializedProperty components;
        SerializedProperty moment;
        SerializedProperty timeValue;
        SerializedProperty colliderTag;
        SerializedProperty colliderMoment;
        SerializedProperty destroyOnFinished;

        void OnEnable()
        {
            type = serializedObject.FindProperty("type");
            action = serializedObject.FindProperty("action");
            components = serializedObject.FindProperty("components");
            moment = serializedObject.FindProperty("moment");
            timeValue = serializedObject.FindProperty("timeValue");
            colliderTag = serializedObject.FindProperty("colliderTag");
            colliderMoment = serializedObject.FindProperty("colliderMoment");
            destroyOnFinished = serializedObject.FindProperty("destroyOnFinished");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //ShowHead("Executions");

            Executor executor = (Executor)target;
            EditorGUILayout.PropertyField(type);

            if (executor.type == ExecuteType.Both || executor.type == ExecuteType.Components)
                 EditorGUILayout.PropertyField(components);
            if (executor.type == ExecuteType.Both || executor.type == ExecuteType.Events)
                EditorGUILayout.PropertyField(action);

            //GUILayout.Space(30);
            //ShowHead("Settings");
            EditorGUILayout.PropertyField(moment);
            if (executor.moment == ExecuteMoment.AfterTime)
                EditorGUILayout.PropertyField(timeValue);
            else
            if (executor.moment == ExecuteMoment.Trigger || executor.moment == ExecuteMoment.Collision)
            {
                EditorGUILayout.PropertyField(colliderTag);
                EditorGUILayout.PropertyField(colliderMoment);
            }


            //GUILayout.Space(30);
            //ShowHead("Others");
            EditorGUILayout.PropertyField(destroyOnFinished);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowHead(string headName)
        {
            Color defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            GUIStyle headInfo = new GUIStyle(GUI.skin.label);
            headInfo.fontStyle = FontStyle.Bold;
            headInfo.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(headName, headInfo);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = defaultColor;
            headInfo.fontStyle = FontStyle.Normal;
        }

    }

}