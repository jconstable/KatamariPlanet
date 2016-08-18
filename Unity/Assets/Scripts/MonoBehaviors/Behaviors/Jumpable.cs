using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Jumpable : MonoBehaviour
{
    public float JumpForce = 2f;
    public float DistanceToJumpableSurfaceImprecision = 0.1f;

    private Rigidbody _rigidBody;
    private bool _allowInput = true;

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();

        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().AddListener(LevelPlayState.GameplayOverEventName, DisableInput);
        }
    }

    void OnDestroy()
    {
        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().RemoveListener(LevelPlayState.GameplayOverEventName, DisableInput);
        }
    }

    public void HandleJump()
    {
        if( !_allowInput )
        {
            return;
        }

        // Cast a ray straight "down", and see if the collider hit is directly underneath us (touching)
        RaycastHit info;
        if( Physics.Raycast( _rigidBody.position, -_rigidBody.position.normalized, out info ) )
        {
            float radius = _rigidBody.transform.lossyScale.y * 0.5f;
            if ( Mathf.Abs( info.distance - radius) < DistanceToJumpableSurfaceImprecision )
            {
                KatamariApp app = KatamariAppProxy.instance;
                if (app != null)
                {
                    app.GetSoundManager().PlayCustomSound("Jump");
                }
                _rigidBody.AddForce(_rigidBody.transform.position.normalized * JumpForce, ForceMode.Impulse);
            }
        }
    }

    public bool DisableInput( object o )
    {
        _allowInput = false;

        return false;
    }
}