using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour
{
    public float FollowDistance = 2f;
    public float CamYHeight = 2f;
    public float CameraRotateSpeed = 5f;

    // Helper function to get direction between katamari ball and camera
    public static Vector3 GetSurfaceDirToCam(KatamariCore core, FollowCamera cam)
    {
        // Grab the distance of the katamari ball from the center of the planet
        float coreDistance = core.transform.position.magnitude;

        // Figure out what direction the camera should be from the ball
        Vector3 dirToCam = ((cam.transform.position.normalized * coreDistance) - core.transform.position).normalized;

        return dirToCam;
    }

    public interface IFollowCameraState
    {
        void Setup(FollowCamera camera, KatamariCore followTarget);
        void OnEnter();
        void OnExit();
        void Update(float dt);
    }

    public enum FollowState
    {
        Auto,
        UserPan,
        _NumStates
    }

    private static readonly FollowState DefaultState = FollowState.Auto;

    private IFollowCameraState _currentState;
    private Dictionary<FollowState, IFollowCameraState> _states;

    private KatamariCore _katamariCore;

    private Vector3 _desiredPosition;
    private Quaternion _desiredRotation;

    // Use this for initialization
    void Start()
    {
        _katamariCore = GameObject.FindObjectOfType<KatamariCore>();
        Debug.Assert(_katamariCore != null, "Could not find a GameObject with a KatamariCore component in scene!");

        // Instatniate the camera states
        _states = new Dictionary<FollowState, IFollowCameraState>((int)FollowState._NumStates)
        {
            { FollowState.Auto, new FollowCameraAutoState() },
            { FollowState.UserPan, new FollowCameraPanState() }
        };

        // Setup the camera states
        foreach (KeyValuePair<FollowState, IFollowCameraState> statePair in _states)
        {
            statePair.Value.Setup(this, _katamariCore);
        }

        // Default state
        SwitchState(DefaultState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            SwitchState(FollowState.UserPan);
        }
        else
        {
            SwitchState(FollowState.Auto);
        }

        if (_currentState != null)
        {
            _currentState.Update(Time.deltaTime);
        }

        gameObject.transform.position = _desiredPosition;
        gameObject.transform.rotation = _desiredRotation;
    }

    public void SetNewPositioning( Vector3 dirToCam )
    {
        // Put the camera at the proper distance from the ball, and give it some height
        Vector3 camPos = _katamariCore.transform.position + (dirToCam * FollowDistance * _katamariCore.transform.lossyScale.y);
        Vector3 add = (camPos.normalized * CamYHeight * _katamariCore.transform.lossyScale.y);
        camPos += add;

        // Create a rotation (that respects our proper "down". transform.forward does NOT respect this well) to face the camera at the ball
        Quaternion q = Quaternion.identity;
        q.SetLookRotation((_katamariCore.transform.position - camPos), camPos);

        _desiredPosition = camPos;
        _desiredRotation = q;
    }

    void SwitchState(FollowState state)
    {
        IFollowCameraState desiredState = null;
        if (!_states.TryGetValue(state, out desiredState))
        {
            Debug.LogError("Unable to find desired CameraFollow state " + state.ToString());
            return;
        }

        if (desiredState != _currentState)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = desiredState;

            if (_currentState != null)
            {
#if VERBOSE_LOGGING
                Debug.Log("Switching FollowCamera state to " + state.ToString());
#endif
                _currentState.OnEnter();
            }
        }
    }
}