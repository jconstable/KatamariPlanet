using UnityEngine;
using System.Collections;

public class UIHelpers {

    public static IEnumerator TweenTextNumberValueCoroutine( UnityEngine.UI.Text textField, float from, float to, float time, string format = "N0" )
    {
        float diff = to - from;
        float startTime = Time.time;
        
        while( (Time.time - startTime) < time )
        {
            float temp = Mathf.Lerp(from, to, (Time.time - startTime) / time);
            if( Mathf.Abs( to - temp ) < Mathf.Abs( diff ) )
            {
                textField.text = temp.ToString(format);
            }
            

            yield return null;
        }

        textField.text = to.ToString(format);
    }
}
