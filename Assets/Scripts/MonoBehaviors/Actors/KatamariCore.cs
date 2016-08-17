using UnityEngine;
using System.Collections;

[RequireComponent(typeof(KatamariMass))]
[RequireComponent(typeof(Rigidbody))]
public class KatamariCore : MonoBehaviour
{
    public static readonly string MassChangedEventName = "KatamariCoreMassChanged";

    public float SizeComparisonFactor = 0.5f;
    public float SwallowScaleAdditionFactor = 0.1f;
    public float GrowthAnimationFactor = 0.1f;
    public float Gravity = -9.8f;

    public float ScorePerMassMultiplier = 10000f;

    public string SwallowSoundName;

    private Rigidbody _rigidBody;
    private KatamariMass _mass;
    private KatamariTracker _tracker;

    float _lastMass = 0f;
    float _additionToProcess = 0f;

    // Use this for initialization
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _mass = gameObject.GetComponent<KatamariMass>();

        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().AddListener(LevelPlayState.GameplayOverEventName, DisablePhysics );
        }
    }

    void OnDestroy()
    {
        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().RemoveListener(LevelPlayState.GameplayOverEventName, DisablePhysics);
        }
    }

    public void SetTracker( KatamariTracker tracker)
    {
        _tracker = tracker;
    }

    // Update is called once per frame
    void Update()
    {
        // Update gravity for the entire physics model
        Physics.gravity = transform.position.normalized * Gravity;


        if( _lastMass < _mass.Mass )
        {
            _lastMass = _mass.Mass;

            KatamariApp app = KatamariAppProxy.instance;
            if (app != null)
            {
                app.GetEventManager().SendEvent(MassChangedEventName, _lastMass * SizeComparisonFactor);
            }
#if UNITY_EDITOR
            _mass.SetName();
#endif
        }

        // Lerp up the size of the katamari ball, and it's spin
        if( _additionToProcess > 0f )
        {
            float addition = Mathf.Lerp(_additionToProcess, 0f, Time.deltaTime) * GrowthAnimationFactor;
            _rigidBody.transform.localScale += (Vector3.one * addition);

            _rigidBody.angularDrag = Mathf.Max(1f, _rigidBody.angularDrag - addition);

            _additionToProcess -= addition;
        }
    }

    // Everything I touch should have a mass
    void OnCollisionEnter(Collision collision)
    {
        KatamariMass m = collision.collider.gameObject.GetComponent<KatamariMass>();
        DebugUtils.Assert(m != null, "Collision with something that doesn't have a KatamariMass. Not good.");

        if( m != null )
        {
            if( m.MassExceeded )
            {
                Swallow(m);
            }
        }
    }

    private void Swallow( KatamariMass m )
    {
        m.transform.parent = _tracker.transform;

        Collider c = m.GetComponent<Collider>();
        c.enabled = false;

        float addition = (m.Mass * SwallowScaleAdditionFactor);

        _additionToProcess += addition;

        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            float score = addition * ScorePerMassMultiplier;
            app.GetEventManager().SendEvent(LevelStats.AddScoreEventName, (int)score);

            if (!string.IsNullOrEmpty(SwallowSoundName))
            {
                app.GetSoundManager().PlayCustomSound(SwallowSoundName);
            }

            app.GetEventManager().SendEvent(LevelStats.SwallowableObjectSwallowedEventName, m.gameObject);
        }
        
    }

    public void OnDrawGizmos()
    {
        MeshFilter mesh = gameObject.GetComponent<MeshFilter>();
        Bounds b = mesh.sharedMesh.bounds;
        Gizmos.DrawWireCube(transform.position, b.size);
    }

    public bool DisablePhysics( object o )
    {
        // Kind of a hack to halt the appearance of physics sim
        _rigidBody.isKinematic = true;

        return false;
    }
}
