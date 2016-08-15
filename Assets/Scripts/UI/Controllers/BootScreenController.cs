using UnityEngine;
using System.Collections;

public class BootScreenController {
    public static readonly string BootScreenClickedEventName = "BootScreenClicked";
    public static readonly string InstructionsScreenClickedEventName = "InstructionScreenClicked";

    private KatamariApp _app;
    private float _timeScreenShown = 0;

    private int _bootScreenId;
    private int _instructionsScreenId;

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
        _bootScreenId = _app.GetUIManager().LoadUI(BootScreenHub.UIKey, null, (int)UILayers.Layers.DefaultUI);
        _app.GetFadeUIController().FadeOut( null );
        _timeScreenShown = Time.time;
    }

    public void ShowInstructionsScreen()
    {
        _instructionsScreenId = _app.GetUIManager().LoadUI(InstructionsUIHub.UIKey, null, (int)UILayers.Layers.DefaultUI);
    }

    public bool OnBootScreenClicked( object param )
    {
        float configuredTimeDelay = (float)param;

        float timeDelta = (Time.time - _timeScreenShown);
        if (timeDelta > configuredTimeDelay)
        {
            _app.GetFadeUIController().FadeIn( () =>
            {
                _app.GetUIManager().DismissUI(_bootScreenId);
                ShowInstructionsScreen();
                _app.GetFadeUIController().FadeOut(null);
            });
            
        } else
        {
            Debug.Log("BootScreenController swallowing click for " + (configuredTimeDelay - timeDelta) + " more sec");
        }
        return false;
    }

    public bool OnInstructionsScreenClicked( object param )
    {
        _app.GetFadeUIController().FadeIn(() =>
        {
            _app.GetUIManager().DismissUI(_instructionsScreenId);
            _app.GetLevelSelectController().ShowLevelSelect();
            _app.GetFadeUIController().FadeOut(null);
        });
        return false;
    }
}
