using UnityEngine;
using System.Collections;

public class FadeUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "fade";

    public UnityEngine.UI.Graphic FadeScrim;
    public float FadeDuration = 0.2f;

    private KatamariApp _app;
    private GameObject _canvasOb;

    public void Setup(KatamariApp app, object param)
    {
        _app = app;

        _app.GetEventManager().AddListener(FadeUIController.FadeInEventName, FadeIn);
        _app.GetEventManager().AddListener(FadeUIController.FadeOutEventName, FadeOut);

        _canvasOb = FadeScrim.canvas.gameObject;
    }

    public void Teardown()
    {
        _app.GetEventManager().RemoveListener(FadeUIController.FadeInEventName, FadeIn);
        _app.GetEventManager().RemoveListener(FadeUIController.FadeOutEventName, FadeOut);
    }

    bool FadeIn( object p )
    {
        System.Action cb = p as System.Action;

        _canvasOb.SetActive(true);
        FadeTo(1f, cb);

        return false;
    }

    bool FadeOut(object p)
    {
        System.Action cb = p as System.Action;

        _canvasOb.SetActive(true); // Just in case we are fading out twice for some reason
        FadeTo(0f, () =>
        {
            _canvasOb.SetActive(false);
            if( cb != null )
            {
                cb();
            }
        });

        return false;
    }

    void FadeTo( float a, System.Action cb )
    {
        StopAllCoroutines();
        FadeScrim.CrossFadeAlpha(a, FadeDuration, true);
        StartCoroutine(ExecuteCB(cb));
    }

    IEnumerator ExecuteCB( System.Action cb )
    {
        yield return new WaitForSeconds(FadeDuration);

        if( cb != null )
        {
            cb();
        }
    }
}
