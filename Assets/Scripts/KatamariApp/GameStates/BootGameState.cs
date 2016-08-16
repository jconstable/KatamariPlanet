using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BootGameState : GameState {
    public override void OnEnter(KatamariApp app)
    {
        base.OnEnter(app);

        UnityEngine.SceneManagement.SceneManager.LoadScene(Files.BootstrapSceneName);

        _app.GetBootScreenController().ShowBootScreen();
        
    }

    public override void OnUpdate(float dt)
    {
    }
}
