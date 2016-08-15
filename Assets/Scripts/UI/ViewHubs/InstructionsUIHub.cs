using UnityEngine;
using System.Collections;

public class InstructionsUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "instructions";

    private KatamariApp _app;

    public void Setup(KatamariApp app, object param) {
        _app = app;
    }

    public void Teardown()
    {
        _app = null;
    }

    public void OnDismissClick()
    {
        _app.GetEventManager().SendEvent(BootScreenController.InstructionsScreenClickedEventName, null);
    }
}
