using UnityEngine;
using System.Collections;

public class MassUIHub : MonoBehaviour {

    public UnityEngine.UI.Text MassLabel;

    private float _mass = 0f;
    private float _lastMass = 0f;

	// Use this for initialization
	void Start () {
        EventManager.AddListener(LevelStats.UpdatedMassEventName, OnMassChanged);
	}
	
	// Update is called once per frame
	void OnDestroy () {
        EventManager.RemoveListener(LevelStats.UpdatedMassEventName, OnMassChanged);
    }

    bool OnMassChanged( object p )
    {
        float newMass = (float)p;
        _mass = newMass;

        StartCoroutine(UIHelpers.TweenTextNumberValueCoroutine(MassLabel, _lastMass, _mass, 0.5f, "N1"));
        _lastMass = _mass;

        return false;
    }
}
