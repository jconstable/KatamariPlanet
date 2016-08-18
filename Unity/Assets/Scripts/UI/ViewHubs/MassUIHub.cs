using UnityEngine;
using System.Collections;

public class MassUIHub : MonoBehaviour {

    public UnityEngine.UI.Text MassLabel;
    
    private float _lastMass = 0f;

    private EventManager _eventManager;

    public void Setup(EventManager eventManager)
    {
        _eventManager = eventManager;

        if (_eventManager != null)
        {
            _eventManager.AddListener(LevelStats.UpdatedMassEventName, OnMassChanged);
        }

        MassLabel.text = 1.ToString("N0");
    }

    public void Teardown()
    {
        if (_eventManager != null)
        {
            _eventManager.RemoveListener(LevelStats.UpdatedMassEventName, OnMassChanged);
        }

        _eventManager = null;
    }
    
    bool OnMassChanged( object p )
    {
        float newMass = (float)p;

        if (newMass > _lastMass)
        {
            MassLabel.text = newMass.ToString("N1");
            _lastMass = newMass;
        }

        return false;
    }
}
