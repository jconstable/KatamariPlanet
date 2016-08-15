using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "GameStates Data")]
public class GameStates : ScriptableObject
{
    [System.Serializable]
    public class GameStateRef
    {
        public string RefName;
        public string RefPath;
    }

    [System.Serializable]
    public class GameStateData
    {
        public string ClassName;
        public List<GameStateRef> ObjectRefs;
    }
    
    public List< GameStateData > GameStateDataList;
}
