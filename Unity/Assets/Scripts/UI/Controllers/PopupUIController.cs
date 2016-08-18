using UnityEngine;
using System.Collections;

public class PopupUIController {

    public static readonly string PopupDismissedEventName = "PopupDismissed";

    private KatamariApp _app;
    private int _popupID = -1;

    public void Setup( KatamariApp app )
    {
        _app = app;

        _app.GetEventManager().AddListener(PopupDismissedEventName, DismissPopup);
    }

    public void Teardown()
    {
        _app = null;
    }

    public void ShowUnhandledExceptionPopup( string message, string stackTrace )
    {
        _popupID = _app.GetUIManager().LoadUI(UnhandledExceptionHub.UIKey, message + "\n" + stackTrace, (int)UILayers.Layers.PopupUI);
    }

    bool DismissPopup( object p )
    {
        _app.GetUIManager().DismissUI(_popupID);

        return false;
    }
}
