using UnityEngine;
using System.Collections;

public class BootScreenHub : MonoBehaviour, UIManager.IUIScreen
{
    public float TimeBeforeClickEnabled = 3f;

    public UnityEngine.UI.Graphic[] ClickNextElements;

    private KatamariApp _app;

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "boot";

    public void Setup( KatamariApp app, object param )
    {
        _app = app;
        SetFadeOnClickElements(0f, 0f);

        StartCoroutine( FadeInClickInstruction() );
    }

    public void Teardown()
    {
        StopAllCoroutines();
        _app = null;
    }

    IEnumerator FadeInClickInstruction()
    {
        yield return new WaitForSeconds(TimeBeforeClickEnabled);
        
        SetFadeOnClickElements(1f, 0.5f);
    }

    void SetFadeOnClickElements( float a, float time )
    {
        for( int i = 0; i < ClickNextElements.Length; ++i )
        {
            ClickNextElements[i].CrossFadeAlpha(a, time, true);
        }
    }

    public void OnClick()
    {
        _app.GetEventManager().SendEvent(BootScreenController.BootScreenClickedEventName, TimeBeforeClickEnabled);
    }
}
