using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pushable))]
public class PushableInspector : Editor
{
    private int _dirLineLength = 10;
    void OnInpsectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Facing Dir Line Length");
        EditorGUILayout.IntField(_dirLineLength);
        EditorGUILayout.EndHorizontal();
    }

    void OnSceneGUI()
    {
        Pushable p = target as Pushable;

        if (p == null)
            return;

        Vector3 center = p.transform.position;

        Handles.DrawLine(center, center+ (p.CurrentPushDir * _dirLineLength));
    }
}
