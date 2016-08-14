using UnityEngine;
using System.Collections;

public class MassUIHub : MonoBehaviour {

    public UnityEngine.UI.Text MassLabel;

    private float _mass = 0f;
    private float _lastMass = 0f;

    private EventManager _eventManager;

    public void Setup(EventManager eventManager)
    {
        _eventManager = eventManager;

        _eventManager.AddListener(LevelStats.UpdatedMassEventName, OnMassChanged);

        MassLabel.text = 1.ToString("N0");
    }

    public void Teardown()
    {
        _eventManager.RemoveListener(LevelStats.UpdatedMassEventName, OnMassChanged);

        _eventManager = null;
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
