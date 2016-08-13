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
        
        _cam.SetNewPositioning(dirToCam);
    }
}
