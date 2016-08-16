using UnityEngine;
using System.Collections;

public class FadeUIController {

    public static readonly string FadeInEventName = "FadeUIFadeIn";
    public static readonly string FadeOutEventName = "FadeUIFadeOut";

    public enum FadeState
    {
        Out,
        In
    }

    public FadeState CurrentFadeState { get; private set; }

    private KatamariApp _app;
    

    public void Setup( KatamariApp app )
    {
        _app = app;

         _app.GetUIManager().LoadUI(FadeUIHub.UIKey, null, (int)UILayers.Layers.FadeUI);
    }

    public void Teardown()
    {
        _app = null;
    }

    public void FadeIn( System.Action completeCallback )
    {
        _app.GetEventManager().SendEvent(FadeInEventName, completeCallback);
        CurrentFadeState = FadeState.In;
    }

    public void FadeOut(System.Action completeCallback)
    {
        _app.GetEventManager().SendEvent(FadeOutEventName, completeCallback);
        CurrentFadeState = FadeState.Out;
    }
}
