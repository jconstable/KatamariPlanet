﻿using UnityEngine;
using System.Collections;

// Small proxy MonoBehavior, so we can hook KatamariApp into Unity's player lifecycle
public class KatamariAppProxy : MonoBehaviour {

    public int NumMusicChannels = 2;
    public int NumSFXChannels = 8;

    private static KatamariApp _app;
    private static KatamariAppProxy _proxyOwner;

    // Gross, singletons, but this should be the only one in the game
    public static KatamariApp instance
    {
        get
        {
            return _app;
        }
    }

	// Use this for initialization
	void Start () {
        if (Application.isPlaying)
        {
            if (_app == null)
            {
                _proxyOwner = this;
                _app = new KatamariApp();

                _app.Setup(this, NumMusicChannels, NumSFXChannels);

                // This instance shall live on
                GameObject.DontDestroyOnLoad(gameObject);
            }
            else
            {
                // This is a secondary AppProxy, there can be only one
                GameObject.Destroy(gameObject);
            }
        }
	}

    void OnDestroy()
    {
        if(_app != null && _proxyOwner == this)
        {
            _app.Teardown();
            _app = null;
            _proxyOwner = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if( _app != null )
        {
            _app.OnUpdate(Time.deltaTime);
        }
	}

    void LateUpdate()
    {
        if( _app != null )
        {
            _app.OnLateUpdate(Time.deltaTime);
        }
    }
}
