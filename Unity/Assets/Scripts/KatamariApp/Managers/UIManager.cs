﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager {
    // Interface that all UI screen Hubs should implement
    public interface IUIScreen
    {
        void Setup(KatamariApp app, object param);
        void Teardown();
    }

    private GameObject _canvasPrefab;
    private GameObject _eventSystem;
    
    private UIKeysToPrefabs _prefabMap;
    private Dictionary<int, Canvas> _loadedScreens;
    private Dictionary<int, int> _loadedScreenIdByLayer;
    private int _keyIndex = -1;

    private KatamariApp _app;

    public void Setup( KatamariApp app )
    {
        _app = app;

        GameObject eventSystemPrefab = Resources.Load(Files.PrefabDefaultUIEventSystemPath) as GameObject;
        DebugUtils.Assert(eventSystemPrefab != null, "UIManager: Unable to EventSystem prefab from " + Files.PrefabDefaultUIEventSystemPath);
        if (eventSystemPrefab != null)
        {
            _eventSystem = GameObject.Instantiate(eventSystemPrefab);
            GameObject.DontDestroyOnLoad(_eventSystem);
        }

        // Load UI data
        _prefabMap = Resources.Load(Files.PrefabMapResourcePath) as UIKeysToPrefabs;
        DebugUtils.Assert(_prefabMap != null, "UIManager: Unable to load prefab map from " + Files.PrefabMapResourcePath);

        _canvasPrefab = Resources.Load(Files.PrefabDefaultCanvasPath) as GameObject;
        DebugUtils.Assert(_canvasPrefab != null, "UIManager: Unable to load default canvas prefab from " + Files.PrefabDefaultCanvasPath);

        _loadedScreens = new Dictionary<int, Canvas>();
        _loadedScreenIdByLayer = new Dictionary<int, int>();
    }

    public void Teardown()
    {
        foreach( KeyValuePair<int,Canvas> pair in _loadedScreens )
        {
            if(pair.Value != null)
            {
                GameObject.Destroy(pair.Value.gameObject);
            }            
        }

        _loadedScreens.Clear();
        _loadedScreens = null;

        GameObject.Destroy(_eventSystem);
        _eventSystem = null;

        _app = null;
    }

    public int LoadUI( string UIKey, object param, int layer )
    {
        GameObject prefab = _prefabMap.GetPrefabForUIKey(UIKey);
        DebugUtils.Assert(prefab != null, "UIManager: Unable to load UI by key " + UIKey);

        int id = -1;
        
        if( prefab != null )
        {
            GameObject screenGO = GameObject.Instantiate(prefab);
            IUIScreen screen = screenGO.GetComponent<IUIScreen>();
            DebugUtils.Assert(screen != null, "UIManager: Loaded UI prefab does not contain a IUIScreen implementor");

            RectTransform topRect = screenGO.GetComponent<RectTransform>();
            DebugUtils.Assert(topRect != null, "UIManager: Loaded UI prefab does contain a RectTransform. Is it really a UI screen?");

            if ( screen != null && topRect != null )
            {
                // Enforce only one screen at a time per layer
                int currentlyLoadedId = -1;
                if( _loadedScreenIdByLayer.TryGetValue( layer, out currentlyLoadedId ) )
                {
                    DismissUI(currentlyLoadedId);
                }

                // This screen gets a unique ID
                    id = ++_keyIndex;

                // Create a Canvas for the screen
                GameObject canvasInstance = GameObject.Instantiate(_canvasPrefab);
                Canvas canvas = canvasInstance.GetComponent<Canvas>();
                canvas.transform.position = Vector3.zero;
                GameObject.DontDestroyOnLoad(canvasInstance);

                _loadedScreens[id] = canvas;
                _loadedScreenIdByLayer[layer] = id;

                // Parent the new screen to the canvas
                screenGO.transform.SetParent(canvas.transform, false );
                topRect.transform.position = Vector3.zero;

                // Fix the transform and assign a z-depth for layering
                canvas.sortingOrder = layer;

                // This screen gets set up
                screen.Setup(_app, param);
                canvas.gameObject.name = "Canvas: " + screenGO.name + " (" + id.ToString() + ")";
            } else
            {
                GameObject.Destroy(screenGO);
            }
        }

        return id;
    }

    public void DismissUI( int instanceId )
    {
        Canvas canvas = null;
        if( _loadedScreens.TryGetValue( instanceId, out canvas) )
        {
            IUIScreen screen = canvas.GetComponentInChildren<IUIScreen>();
            screen.Teardown();

            _loadedScreens.Remove(instanceId);
            GameObject.Destroy(canvas.gameObject);
        }
    }
}
