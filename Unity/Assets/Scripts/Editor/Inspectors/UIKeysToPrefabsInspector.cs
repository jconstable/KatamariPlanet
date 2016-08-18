using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIKeysToPrefabs))]
public class UIKeysToPrefabsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIKeysToPrefabs data = target as UIKeysToPrefabs;

        UIManager uiManager = null;
        if( Application.isPlaying )
        {
            KatamariApp app = KatamariAppProxy.instance;
            if( app != null )
            {
                uiManager = app.GetUIManager();
            }
        }

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
                    uiManager.LoadUI(map.UIKey, null, (int)UILayers.Layers.DefaultUI );
                }
            } else
            {
                GUILayout.Button("Load (when playing)");
            }

            EditorGUILayout.EndHorizontal();
        }
        
    }
}