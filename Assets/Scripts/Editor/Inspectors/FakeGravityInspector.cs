using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FakeGravity))]
public class FakeGravityInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if( GUILayout.Button("Align") )
        {
            FakeGravity f = target as FakeGravity;
            FixDown(f.gameObject);
        }

        EditorGUILayout.EndHorizontal();
    }

    void FixDown( GameObject o )
    {
        o.transform.up = o.transform.position.normalized;
    }
}
