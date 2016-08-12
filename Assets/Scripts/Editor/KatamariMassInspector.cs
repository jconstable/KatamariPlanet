using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KatamariMass))]
public class KatamariMassInspector : Editor
{
    void OnInpsectorGUI()
    {
        DrawDefaultInspector();

        KatamariMass m = target as KatamariMass;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("KatamariMass: " + m.Mass.ToString() );
        EditorGUILayout.EndHorizontal();
    }
}