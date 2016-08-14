using UnityEngine;
using System.Collections;

public class GameplayUIController {
    private KatamariApp _app;

    public void Setup( KatamariApp app )
    {
        _app = app;
    }

    public void Teardown()
    {
        _app = null;
    }

    public int ShowGameplayUI()
    {
        return _app.GetUIManager().LoadUI(GameplayUIHub.UIKey, 1);
    }
}
