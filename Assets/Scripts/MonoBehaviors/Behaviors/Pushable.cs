using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    public float PushForce = 2f;

    private Rigidbody _rigidBody;
    private KatamariCore _core;
    private FollowCamera _followCam;

    private Vector3 _initialPositionForReset;

    public Vector3 CurrentPushDir { get; private set; }

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _core = gameObject.GetComponent<KatamariCore>();

        _followCam = GameObject.FindObjectOfType<FollowCamera>();
        _initialPositionForReset = _rigidBody.transform.position;
    }

    public void HandlePush(Vector3 cameraFacingInput)
    {
        Quaternion q = Quaternion.identity;

        CurrentPushDir = -FollowCamera.GetSurfaceDirToCam(_core, _followCam);
        q.SetLookRotation(CurrentPushDir, _core.transform.position.normalized);

        Vector3 cameraCorrectedInput = q * (cameraFacingInput * PushForce);

        _rigidBody.AddForce(cameraCorrectedInput);
    }

    public void HandleJump()
    {

    }

    public void Reset()
    {
        _rigidBody.transform.position = _initialPositionForReset;
    }
}
