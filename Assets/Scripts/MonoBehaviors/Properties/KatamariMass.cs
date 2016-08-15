using UnityEngine;
using System.Collections;

public class KatamariMass : MonoBehaviour
{
    private static readonly string NameLabelSeparator = "::";

    public bool MassExceeded { get; private set; }

    private MeshFilter _mesh;

    // Use this for initialization
    void OnEnable()
    {
        _mesh = gameObject.GetComponent<MeshFilter>();

#if UNITY_EDITOR
        SetName();
#endif

        KatamariApp app = KatamariAppProxy.instance;
        if(app != null)
        {
            app.GetEventManager().AddListener(KatamariCore.MassChangedEventName, CompareMass);
        }
        
    }

    void OnDisable()
    {
        KatamariApp app = KatamariAppProxy.instance;
        if (app != null && app.GetEventManager() != null )
        {
            app.GetEventManager().RemoveListener(KatamariCore.MassChangedEventName, CompareMass);
        }
    }

    bool CompareMass( object param )
    {
        float newMass = (float)param;
        
        if( newMass >= Mass )
        {
            MassExceeded = true;
            SetName();
        }

        return false; // Don't stop processing this event
    }

    public void SetName()
    {
        if( gameObject.name.Contains(NameLabelSeparator))
        {
            gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf(NameLabelSeparator));
        }
        gameObject.name = gameObject.name + NameLabelSeparator + Mass.ToString() + "ccm " + ( MassExceeded ? "X" : "" );
    }

    public float Mass
    {
        get
        {
            float m = 0f;

            if (_mesh != null)
            {
                Vector3 size = _mesh.sharedMesh.bounds.size;
                m = size.x * size.y * size.z * transform.localScale.magnitude;
            }

            return m;
        }
    }
}
