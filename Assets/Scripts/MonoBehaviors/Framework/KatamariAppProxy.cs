using UnityEngine;
using System.Collections;

// Small proxy MonoBehavior, so we can hook KatamariApp into Unity's player lifecycle
public class KatamariAppProxy : MonoBehaviour {

    private static KatamariApp _app;

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
        if (_app == null)
        {
            _app = new KatamariApp();

            _app.Setup();
        } else
        {
            // This is a secondary AppProxy, there can be only one
            GameObject.Destroy(gameObject);
        }
	}

    void OnDestroy()
    {
        _app.Teardown();
    }
	
	// Update is called once per frame
	void Update () {
        _app.OnUpdate(Time.deltaTime);
	}

    void LateUpdate()
    {
        _app.OnLateUpdate(Time.deltaTime);
    }
}
