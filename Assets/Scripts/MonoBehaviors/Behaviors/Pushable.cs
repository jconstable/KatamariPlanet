using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    public float PushForce = 2f;
    public float MaxVelocityMag = 5;
    public float IncreasedPushByMassFactor = 1f;

    private Rigidbody _rigidBody;
    private KatamariCore _core;
    private FollowCamera _followCam;

    private Vector3 _initialPositionForReset;

    public Vector3 CurrentPushDir { get; private set; }

    public bool _allowInput = true;

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _core = gameObject.GetComponent<KatamariCore>();

        _followCam = GameObject.FindObjectOfType<FollowCamera>();
        _initialPositionForReset = _rigidBody.transform.position;

        KatamariApp app = KatamariAppProxy.instance;
        if( app != null )
        {
            app.GetEventManager().AddListener(LevelPlayState.GameplayOverEventName, DisableInput );
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

    public void HandlePush(Vector3 cameraFacingInput)
    {
        if (!_allowInput)
        {
            return;
        }

        Quaternion q = Quaternion.identity;

        CurrentPushDir = -FollowCamera.GetSurfaceDirToCam(_core, _followCam);
        q.SetLookRotation(CurrentPushDir, _core.transform.position.normalized);

        Vector3 cameraCorrectedInput = q * (cameraFacingInput * PushForce );

        // Add force if below max speed
        float factoredSpeedIncrease = MaxVelocityMag * (_rigidBody.transform.localScale.y * IncreasedPushByMassFactor);
        if (Vector3.Dot(cameraCorrectedInput, _rigidBody.velocity) < Mathf.Max(MaxVelocityMag, factoredSpeedIncrease ))
        {
            _rigidBody.AddForce(cameraCorrectedInput);
        }
    }

    public void HandleJump()
    {
        if (!_allowInput)
        {
            return;
        }
    }

    public void Reset()
    {
        _rigidBody.transform.position = _initialPositionForReset;
    }

    public bool DisableInput( object o )
    {
        _allowInput = false;

        return false;
    }
}
