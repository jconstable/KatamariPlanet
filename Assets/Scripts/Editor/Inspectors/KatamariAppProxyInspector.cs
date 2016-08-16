using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(KatamariAppProxy))]
public class KatamariAppProxyInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        KatamariApp app = KatamariAppProxy.instance;
        if( app != null )
        {
            EventManager events = app.GetEventManager();

            if( events != null )
            {
                Dictionary<string, int> stats = events.GetListenerStats();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Event Manager: ");
                EditorGUILayout.EndHorizontal();

                foreach( KeyValuePair<string,int> pair in stats )
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(pair.Key);
                    EditorGUILayout.LabelField(pair.Value.ToString());
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}
