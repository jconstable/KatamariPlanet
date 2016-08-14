using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIKeysToPrefabs))]
public class UIKeysToPrefabsInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIKeysToPrefabs data = target as UIKeysToPrefabs;

        int count = data.GetNumUIPaths();
        for(int i = 0; i < count; ++i )
        {
            UIKeysToPrefabs.UIKeyToPrefabMap map = data.GetMapAt(i);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("UIKey: " + map.UIKey);
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Load"))
                {
                    UIManager.LoadUI(map.UIKey, 0);
                }
            } else
            {
                GUILayout.Button("Load (when playing)");
            }

            EditorGUILayout.EndHorizontal();
        }
        
    }
}