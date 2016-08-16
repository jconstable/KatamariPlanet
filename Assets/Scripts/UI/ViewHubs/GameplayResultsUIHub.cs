using UnityEngine;
using System.Collections;

public class GameplayResultsUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "level.results";

    public float InitialPauseDuration = 2f;
    public float PauseBetweenStars = 1f;
    public float FadeInTime = 0.4f;

    public UnityEngine.UI.Graphic[] StarsOn;
    public UnityEngine.UI.Graphic[] StarsOff;
    public UnityEngine.UI.Text NewHighScoreLabel;
    public UnityEngine.UI.Text ClickAnywhereLabel;
    public UnityEngine.UI.Graphic ClickAnywhereBG;

    private KatamariApp _app;
    private bool _respondToClicks = false;

    public void Setup(KatamariApp app, object param)
    {
        _app = app;

        LevelScore score = param as LevelScore;
        LevelData.LevelDefinition def = app.GetLevelData().FindByLevelID(score.LevelID);

        // Start with elements we will fade in set to disabled
        NewHighScoreLabel.CrossFadeAlpha(0f, 0f, true);
        ClickAnywhereLabel.CrossFadeAlpha(0f, 0f, true);
        ClickAnywhereBG.CrossFadeAlpha(0f, 0f, true);

        for (int i = 0; i < StarsOn.Length; ++i)
        {
            StarsOn[i].CrossFadeAlpha(0f, 0f, true);
            StarsOff[i].CrossFadeAlpha(0f, 0f, true);
        }

        StartCoroutine(ShowStars(score, def));
    }

    public void Teardown()
    {

    }

    IEnumerator ShowStars( LevelScore score, LevelData.LevelDefinition def )
    {
        yield return new WaitForSeconds(InitialPauseDuration);

        for( int i = 0; i < def.StarPointRequirements.Length; ++i )
        {
            if (score.HighScore >= def.StarPointRequirements[i])
            {
                if (i < StarsOn.Length)
                {
                    StarsOn[i].CrossFadeAlpha(1f, FadeInTime, true);
                    _app.GetSoundManager().PlayUISound(UISounds.SoundEvent.StarAwarded);
                }
            } else
            {
                if (i < StarsOff.Length)
                {
                    StarsOff[i].CrossFadeAlpha(1f, FadeInTime, true);
                }
            }

            yield return new WaitForSeconds(PauseBetweenStars);
        }

        if( score.NewHighScore )
        {
            _app.GetSoundManager().PlayUISound(UISounds.SoundEvent.NewHighScore);
            NewHighScoreLabel.CrossFadeAlpha(1f, FadeInTime, true);
            yield return new WaitForSeconds(PauseBetweenStars);
        }

        yield return new WaitForSeconds(InitialPauseDuration);

        ClickAnywhereLabel.CrossFadeAlpha(1f, FadeInTime, true);
        ClickAnywhereBG.CrossFadeAlpha(1f, FadeInTime, true);
        _respondToClicks = true;
    }

    public void OnDismissClick()
    {
        if( _respondToClicks )
        {
            _app.GetSoundManager().PlayUISound(UISounds.SoundEvent.LevelLeave);
            _app.GetEventManager().SendEvent(GameplayUIController.LeaveGameplayEventName, null);
        }
    }
}
