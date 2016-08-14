using UnityEngine;
using System.Collections;

public class BootGameState : KatamariApp.IAppState {

    public static readonly string StateKey = "BootGameState";

    private KatamariApp _app;

    public void OnEnter(KatamariApp app)
    {
        _app = app;

        _app.GetUIManager().LoadUI(BootScreenHub.UIKey, 0);
    }

    public void OnExit()
    {
        _app = null;
    }

    public void OnUpdate(float dt)
    {
    }
}
