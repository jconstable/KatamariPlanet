using UnityEngine;
using System.Collections;

public class BootScreenController {
    public static readonly string BootScreenClickedEventName = "BootScreenClicked";
    public static readonly string InstructionsScreenClickedEventName = "InstructionScreenClicked";

    private KatamariApp _app;

	public void Setup( KatamariApp app )
    {
        _app = app;

        _app.GetEventManager().AddListener(BootScreenClickedEventName, OnBootScreenClicked);
        _app.GetEventManager().AddListener(InstructionsScreenClickedEventName, OnInstructionsScreenClicked);
    }

    public void Teardown()
    {
        _app.GetEventManager().RemoveListener(BootScreenClickedEventName, OnBootScreenClicked);
        _app.GetEventManager().RemoveListener(InstructionsScreenClickedEventName, OnInstructionsScreenClicked);

        _app = null;
    }

    public void ShowBootScreen()
    {
        UIHelpers.FadeToUI(_app, BootScreenHub.UIKey, null, (int)UILayers.Layers.DefaultUI);
    }

    public bool OnBootScreenClicked( object param )
    {
        UIHelpers.FadeToUI(_app, InstructionsUIHub.UIKey, null, (int)UILayers.Layers.DefaultUI );

        return false;
    }

    public bool OnInstructionsScreenClicked( object param )
    {
        UIHelpers.FadeToUIAction(_app, () =>
        {
            _app.SwitchToState(typeof(LevelSelectState).ToString());
        });

        return false;
    }
}
