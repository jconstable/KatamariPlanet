using UnityEngine;
using System.Collections;

public class LevelSelectState : GameState
{
    public override void OnEnter(KatamariApp app)
    {
        base.OnEnter(app);

        UnityEngine.SceneManagement.SceneManager.LoadScene(Files.LevelSelectSceneName);

        _app.GetLevelSelectController().ShowLevelSelect();
    }

    public override void OnUpdate(float dt)
    {
    }
}