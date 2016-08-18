using UnityEngine;
using System.Collections;

public class KatamariTracker : MonoBehaviour {

    private KatamariCore _core;

	// Use this for initialization
	void Start () {
        _core = GameObject.FindObjectOfType<KatamariCore>();
        _core.SetTracker(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (_core != null)
        {
            transform.position = _core.transform.position;
            transform.rotation = _core.transform.rotation;
        }
	}
}
