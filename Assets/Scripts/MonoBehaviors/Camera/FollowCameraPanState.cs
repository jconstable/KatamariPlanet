using UnityEngine;

public class FollowCameraPanState : FollowCamera.IFollowCameraState
{
    private FollowCamera _cam;
    private KatamariCore _core;

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
    }

    public void OnExit()
    {

    }

    private float _totalDrag = 0f;

    public void Update(float dt)
    {
        Vector3 dirToCam = FollowCamera.GetSurfaceDirToCam(_core, _cam);
        float dragX = Input.mousePosition.x - _mouseDragLastPos.x;

        // Figure out the number of radians to rotate
        _totalDrag += dragX;

        if (!Mathf.Approximately(_totalDrag, 0f))
        {
            float amt = _totalDrag * dt * _cam.CameraRotateSpeed;

            // Set up a quaternion to represent the rotation, and apply it to the direction that points to where we want the camera
            dirToCam = Quaternion.Euler(_core.transform.position.normalized * amt) * dirToCam;

            _totalDrag -= amt;
        }

        Vector3 camPos = _core.transform.position + (dirToCam * _cam.FollowDistance);
        Vector3 add = (camPos.normalized * _cam.CamYHeight);
        camPos += add;

        // Create a rotation (that respects our proper "down". transform.forward does NOT respect this well) to face the camera at the ball
        Quaternion q = Quaternion.identity;
        q.SetLookRotation((_core.transform.position - camPos), camPos);

        _cam.SetNewPositioning(camPos, q);

        _mouseDragLastPos = Input.mousePosition;
    }
}