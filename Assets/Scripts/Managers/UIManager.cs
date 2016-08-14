using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager {
    private static readonly int MetersPerLayer = 10;

    public interface IUIScreen
    {
        void Setup( KatamariApp app );
        void Teardown();
    }
   
    private Canvas _canvas;
    private UIKeysToPrefabs _prefabMap;
    private Dictionary<int, GameObject> _loadedScreens;
    private int _keyIndex = -1;

    private KatamariApp _app;

    public void Setup( KatamariApp app )
    {
        _app = app;

        // Load UI data
        _prefabMap = Resources.Load(Files.PrefabMapResourcePath) as UIKeysToPrefabs;
        Debug.Assert(_prefabMap != null, "UIManager: Unable to load prefab map from " + Files.PrefabMapResourcePath);

        GameObject canvasPrefab = Resources.Load(Files.PrefabDefaultCanvasPath) as GameObject;
        Debug.Assert(canvasPrefab != null, "UIManager: Unable to load default canvas prefab from " + Files.PrefabDefaultCanvasPath);

        // Instantiate the canvas
        if (canvasPrefab != null)
        {
            GameObject canvasInstance = GameObject.Instantiate(canvasPrefab);
            _canvas = canvasInstance.GetComponent<Canvas>();
            _canvas.transform.position = Vector3.zero;
        }

        _loadedScreens = new Dictionary<int, GameObject>();
    }

    public void Teardown()
    {
        foreach( KeyValuePair<int,GameObject> pair in _loadedScreens )
        {
            GameObject.Destroy(pair.Value);
        }

        _loadedScreens.Clear();
        _loadedScreens = null;

        _app = null;
    }

    public int LoadUI( string UIKey, int layer )
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
                // This screen gets a unique ID
                id = ++_keyIndex;
                _loadedScreens[id] = screenGO;

                // This screen gets set up
                screen.Setup( _app );
                screenGO.name += " (" + id.ToString() + ")";
                
                // Parent the new screen to the canvas
                screenGO.transform.SetParent( _canvas.transform, false );

                // Fix the transform and assign a z-depth for layering
                topRect.position += Vector3.back * (float)layer * -(float)MetersPerLayer;
            } else
            {
                GameObject.Destroy(screenGO);
            }
        }

        return id;
    }
}
