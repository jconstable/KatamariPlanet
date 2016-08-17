using UnityEngine;
using System.Collections;

public class GameplayResultsUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "level.results";

    public float InitialPauseDuration = 2f;
    public float PauseBetweenStars = 1f;
    public float FadeInTime = 0.4f;

    public UnityEngine.UI.Graphic[] StarsOn;
    public UnityEngine.UI.Graphic[] StarsOff;

    public UnityEngine.UI.Text BaseScoreLabel;
    public UnityEngine.UI.Text BaseScore;

    public UnityEngine.UI.Text TimeBonusLabel;
    public UnityEngine.UI.Text TimeBonus;

    public UnityEngine.UI.Text TotalScoreLabel;
    public UnityEngine.UI.Text TotalScore;

    public UnityEngine.UI.Text NewHighScoreLabel;
    public UnityEngine.UI.Text ClickAnywhereLabel;
    public UnityEngine.UI.Graphic ClickAnywhereBG;

    private KatamariApp _app;
    private LevelScore _score;
    private bool _respondToClicks = false;

    public void Setup(KatamariApp app, object param)
    {
        _app = app;

        _score = param as LevelScore;
        DebugUtils.Assert(_score != null, "No LevelScore passed into GameplayResultsUIHub");

        if (_score != null)
        {
            LevelData.LevelDefinition def = app.GetLevelData().FindByLevelID(_score.LevelID);

            // Start with elements we will fade in set to disabled
            NewHighScoreLabel.CrossFadeAlpha(0f, 0f, true);
            ClickAnywhereLabel.CrossFadeAlpha(0f, 0f, true);
            ClickAnywhereBG.CrossFadeAlpha(0f, 0f, true);

            BaseScoreLabel.CrossFadeAlpha(0f, 0f, true);
            BaseScore.CrossFadeAlpha(0f, 0f, true);
            BaseScore.text = 0.ToString();

            TimeBonusLabel.CrossFadeAlpha(0f, 0f, true);
            TimeBonus.CrossFadeAlpha(0f, 0f, true);
            TimeBonus.text = 0.ToString();

            TotalScoreLabel.CrossFadeAlpha(0f, 0f, true);
            TotalScore.CrossFadeAlpha(0f, 0f, true);
            TotalScore.text = 0.ToString();

            for (int i = 0; i < StarsOn.Length; ++i)
            {
                StarsOn[i].CrossFadeAlpha(0f, 0f, true);
                StarsOff[i].CrossFadeAlpha(0f, 0f, true);
            }

            StartCoroutine(ShowStars(_score, def));
        }
    }

    public void Teardown()
    {

    }

    IEnumerator ShowStars( LevelScore score, LevelData.LevelDefinition def )
    {
        yield return new WaitForSeconds(InitialPauseDuration);

        for( int i = 0; i < def.StarPointRequirements.Length; ++i )
        {
            if (score.TotalScore >= def.StarPointRequirements[i])
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

        BaseScoreLabel.CrossFadeAlpha(1f, FadeInTime, true);
        BaseScore.CrossFadeAlpha(1f, FadeInTime, true);
        StartCoroutine( UIHelpers.TweenTextNumberValueCoroutine(BaseScore, 0, _score.BaseScore, InitialPauseDuration) );

        yield return new WaitForSeconds(PauseBetweenStars);

        TimeBonusLabel.CrossFadeAlpha(1f, FadeInTime, true);
        TimeBonus.CrossFadeAlpha(1f, FadeInTime, true);
        StartCoroutine( UIHelpers.TweenTextNumberValueCoroutine(TimeBonus, 0, _score.BonusScore, InitialPauseDuration) );

        yield return new WaitForSeconds(PauseBetweenStars);

        TotalScoreLabel.CrossFadeAlpha(1f, FadeInTime, true);
        TotalScore.CrossFadeAlpha(1f, FadeInTime, true);
        StartCoroutine( UIHelpers.TweenTextNumberValueCoroutine(TotalScore, 0, _score.TotalScore, InitialPauseDuration) );

        yield return new WaitForSeconds(PauseBetweenStars);

        if ( score.NewHighScore )
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
