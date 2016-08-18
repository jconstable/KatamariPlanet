using UnityEngine;

public class FollowCameraPanState : FollowCamera.IFollowCameraState
{
    public float MouseSensitivity = 0.3f;

    private FollowCamera _cam;
    private KatamariCore _core;
    private float _maxYDegrees = 55f;

    private Vector3 _mouseDragLastPos;

    public void Setup(FollowCamera camera, KatamariCore core)
    {
        _cam = camera;
        _core = core;
    }

    public void OnEnter()
    {
        _mouseDragLastPos = Input.mousePosition;
        _totalDrag = 0f;
        _totalRot = 0f;
    }

    public void OnExit()
    {

    }

    private float _totalDrag = 0f;
    private float _totalRot = 0f;

    public void Update(float dt)
    {
        Vector3 dirToCam = FollowCamera.GetSurfaceDirToCam(_core, _cam);
        float dragX = ( Input.mousePosition.x - _mouseDragLastPos.x ) * MouseSensitivity;
        float dragY = ( Input.mousePosition.y - _mouseDragLastPos.y ) * MouseSensitivity * 0.01f;

        // Figure out the number of radians to rotate
        _totalDrag += dragX;
        _totalRot = Mathf.Max( 0, _totalRot - dragY);

        if (!Mathf.Approximately(_totalDrag, 0f))
        {
            float amt = _totalDrag * dt * _cam.CameraRotateSpeed;

            // Set up a quaternion to represent the rotation, and apply it to the direction that points to where we want the camera
            dirToCam = Quaternion.Euler(_core.transform.position.normalized * amt) * dirToCam;
            
            _totalDrag -= amt;
        }

        dirToCam = Vector3.RotateTowards(dirToCam, _core.transform.position.normalized, Mathf.Min(_totalRot, _maxYDegrees * Mathf.Deg2Rad), 1f);

        _cam.SetNewPositioning(dirToCam,false);

        _mouseDragLastPos = Input.mousePosition;
    }
}