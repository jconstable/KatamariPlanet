using System;
using UnityEngine;
using System.Collections;

public class UnhandledExceptionHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "popup.exception";

    [SerializeField]
    private UnityEngine.UI.Text ExceptionText;

    private KatamariApp _app;

    public void Setup(KatamariApp app, object param)
    {
        _app = app;

        ExceptionText.text = param as string;
    }

    public void Teardown()
    {

    }

    public void OnDismissed()
    {
        _app.GetEventManager().SendEvent(PopupUIController.PopupDismissedEventName, null);
    }
}
