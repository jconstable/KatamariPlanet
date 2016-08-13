using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KatamariMass))]
public class KatamariMassInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KatamariMass m = target as KatamariMass;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("KatamariMass: " + m.Mass.ToString() );
        EditorGUILayout.LabelField("Mass Exceeded: " + m.MassExceeded.ToString());
        EditorGUILayout.EndHorizontal();
    }
}