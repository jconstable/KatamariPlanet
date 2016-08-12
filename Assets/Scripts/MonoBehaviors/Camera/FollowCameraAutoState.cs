using UnityEngine;
using System.Collections;

public class FollowCameraAutoState : FollowCamera.IFollowCameraState
{
    private FollowCamera _cam;
    private KatamariCore _core;

    public void Setup(FollowCamera camera, KatamariCore core)
    {
        _cam = camera;
        _core = core;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void Update(float dt)
    {
        // Figure out what direction the camera should be from the ball
        Vector3 dirToCam = FollowCamera.GetSurfaceDirToCam(_core, _cam);

        // Put the camera at the proper distance from the ball, and give it some height
        Vector3 camPos = _core.transform.position + (dirToCam * _cam.FollowDistance);
        Vector3 add = (camPos.normalized * _cam.CamYHeight);
        camPos += add;

        // Create a rotation (that respects our proper "down". transform.forward does NOT respect this well) to face the camera at the ball
        Quaternion q = Quaternion.identity;
        q.SetLookRotation((_core.transform.position - camPos), camPos);

        _cam.SetNewPositioning(camPos, q);
    }
}
