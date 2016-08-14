using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    private static readonly int MetersPerLayer = 10;

    public interface IUIScreen
    {
        void Setup();
        void Teardown();
    }

    // Singleton, gross
    private static UIManager _instance;
    private static UIManager instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject o = new GameObject("UIManager");
                    _instance = o.AddComponent<UIManager>();
                }
                if (_instance != null)
                {
                    _instance.Setup();
                }
            }
            return _instance;
        }
    }

    private Canvas _canvas;
    private UIKeysToPrefabs _prefabMap;
    private Dictionary<int, GameObject> _loadedScreens;
    private int _keyIndex = -1;

    private void Setup()
    {
        _prefabMap = Resources.Load(Files.PrefabMapResourcePath) as UIKeysToPrefabs;
        Debug.Assert(_prefabMap != null, "UIManager: Unable to load prefab map from " + Files.PrefabMapResourcePath);

        GameObject canvasPrefab = Resources.Load(Files.PrefabDefaultCanvasPath) as GameObject;
        Debug.Assert(canvasPrefab != null, "UIManager: Unable to load default canvas prefab from " + Files.PrefabDefaultCanvasPath);

        if (canvasPrefab != null)
        {
            GameObject canvasInstance = GameObject.Instantiate(canvasPrefab);
            _canvas = canvasInstance.GetComponent<Canvas>();
            _canvas.transform.position = Vector3.zero;
        }

        _loadedScreens = new Dictionary<int, GameObject>();
    }

    public static int LoadUI( string UIKey, int layer )
    {
        int id = -1;

        if( instance != null )
        {
            id = instance._LoadUI(UIKey, layer);
        }

        return id;
    }

    public int _LoadUI( string UIKey, int layer )
    {
        GameObject prefab = _prefabMap.GetPrefabForUIKey(UIKey);
        Debug.Assert(prefab != null, "UIManager: Unable to load UI by key " + UIKey);

        int id = -1;
        
        if( prefab != null )
        {
            GameObject screenGO = GameObject.Instantiate(prefab);
            IUIScreen screen = screenGO.GetComponent<IUIScreen>();
            Debug.Assert(screen != null, "UIManager: Loaded UI prefab does not contain a IUIScreen implementor");

            RectTransform topRect = screenGO.GetComponent<RectTransform>();
            Debug.Assert(topRect != null, "UIManager: Loaded UI prefab does contain a RectTransform. Is it really a UI screen?");

            if ( screen != null && topRect != null )
            {
                id = ++_keyIndex;
                screen.Setup();
                _loadedScreens[id] = screenGO;

                screenGO.name += " (" + id.ToString() + ")";
                
                // Parent the new screen to the canvas
                screenGO.transform.parent = _canvas.transform;
                screenGO.transform.localScale = Vector3.one;

                // Fix the transform and assign a z-depth for layering
                topRect.pivot = Vector2.one * 0.5f;
                topRect.position = Vector3.back * (float)layer * (float)MetersPerLayer;                
                topRect.sizeDelta = Vector2.zero;
            } else
            {
                GameObject.Destroy(screenGO);
            }
        }

        return id;
    }
}
