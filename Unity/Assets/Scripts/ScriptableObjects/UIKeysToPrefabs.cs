using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "UIKeyToPrefabMap")]
public class UIKeysToPrefabs : ScriptableObject {
    [System.Serializable]
    public class UIKeyToPrefabMap
    {
        public string UIKey;
        public GameObject Prefab;

        public UIKeyToPrefabMap Clone()
        {
            return new UIKeyToPrefabMap()
            {
                UIKey = UIKey,
                Prefab = Prefab
            };
        }
    }

    [SerializeField]
    private List<UIKeyToPrefabMap> _uiMaps = new List<UIKeyToPrefabMap>();

    public GameObject GetPrefabForUIKey( string uiKey )
    {
        for( int i = 0; i < _uiMaps.Count; ++i )
        {
            UIKeyToPrefabMap map = _uiMaps[i];
            if( map.UIKey.Equals(uiKey) )
            {
                return map.Prefab;
            }
        }

        return null;
    }

    public int GetNumUIPaths()
    {
        return _uiMaps.Count;
    }

    public UIKeyToPrefabMap GetMapAt( int i )
    {
        UIKeyToPrefabMap map = null;

        if( i >= 0 && i < _uiMaps.Count )
        {
            // Clone it, so we aren't leaking our data out for modification
            map = _uiMaps[i].Clone();
        }

        return map;
    }
}
