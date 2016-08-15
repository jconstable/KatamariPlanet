using UnityEngine;
using System.Collections;

public class FadeUIController {

    public static readonly string FadeInEventName = "FadeUIFadeIn";
    public static readonly string FadeOutEventName = "FadeUIFadeOut";

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
    }

    public void FadeOut(System.Action completeCallback)
    {
        _app.GetEventManager().SendEvent(FadeOutEventName, completeCallback);
    }
}
