using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Jumpable : MonoBehaviour
{
    public float JumpForce = 2f;
    public float VerticalDotMax = 0.01f;

    private Rigidbody _rigidBody;
    private bool _allowInput = true;

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();

        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().AddListener(PlayGameState.GameplayOverEventName, DisableInput);
        }
    }

    void OnDestroy()
    {
        KatamariApp app = KatamariAppProxy.instance;
        if (app != null)
        {
            app.GetEventManager().RemoveListener(PlayGameState.GameplayOverEventName, DisableInput);
        }
    }

    public void HandleJump()
    {
        if( !_allowInput )
        {
            return;
        }

        float dot = Vector3.Dot(_rigidBody.velocity.normalized, _rigidBody.transform.position.normalized);
        Debug.Log(Mathf.Abs(dot).ToString());
        if ( Mathf.Abs(dot) < VerticalDotMax)
        {
            _rigidBody.AddForce(_rigidBody.transform.position.normalized * JumpForce, ForceMode.Impulse);
        }
    }

    public bool DisableInput( object o )
    {
        _allowInput = false;

        return false;
    }
}