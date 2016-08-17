using UnityEngine;
using System.Collections;
using System.Text;

public class UIHelpers {
    public static void FadeToUI( KatamariApp app, string uiKey, object param, int layer )
    {
        System.Action onFadedIn = () =>
        {
            app.GetUIManager().LoadUI(uiKey, param, layer);
            app.GetFadeUIController().FadeOut(null);
        };

        if( app.GetFadeUIController().CurrentFadeState == FadeUIController.FadeState.Out )
        {
            app.GetFadeUIController().FadeIn(onFadedIn);
        } else
        {
            onFadedIn();
        }
    }

    public static void FadeToUIAction( KatamariApp app, System.Action action)
    {
        System.Action onFadedIn = () =>
        {
            if( action != null )
            {
                action();
            }
            app.GetFadeUIController().FadeOut(null);
        };

        if (app.GetFadeUIController().CurrentFadeState == FadeUIController.FadeState.Out)
        {
            app.GetFadeUIController().FadeIn(onFadedIn);
        }
        else
        {
            onFadedIn();
        }
    }

    public static IEnumerator TweenTextNumberValueCoroutine(UnityEngine.UI.Text textField, float from, float to, float time, string format = "N0", System.Action clickCallback = null, float updateInterval = 0.05f)
    {
        float diff = to - from;
        float startTime = Time.time;
        
        while( (Time.time - startTime) < time )
        {
            float temp = Mathf.Lerp(from, to, (Time.time - startTime) / time);
            if( Mathf.Abs( to - temp ) < Mathf.Abs( diff ) )
            {
                textField.text = temp.ToString(format);
                if(clickCallback != null )
                {
                    clickCallback();
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }

        textField.text = to.ToString(format);
    }

    public static string FormatTime( int seconds )
    {
        StringBuilder b = new StringBuilder();

        while( seconds > 0 )
        {
            int seg = seconds % 60;

            if(b.Length > 0)
            {
                b.Insert(0, ":");
            }

            b.Insert(0, seg.ToString("00"));
            seconds /= 60;
        }

        if( b.Length == 0 )
        {
            b.Append(seconds.ToString("00"));
        }

        // If it's just seoncds, still show 0:XX beacuse it looks nicer
        if( !b.ToString().Contains(":") )
        {
            b.Insert(0, "0:");
        }

        return b.ToString();
    }
}
