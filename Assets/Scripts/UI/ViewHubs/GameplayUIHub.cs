using UnityEngine;
using System.Collections;

public class GameplayUIHub : MonoBehaviour, UIManager.IUIScreen {

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "level.hud";

    [SerializeField]
    private LevelScoreHub LevelScoreHub;

    [SerializeField]
    private MassUIHub MassUIHub;

    [SerializeField]
    private UnityEngine.UI.Text HighScoreText;
    [SerializeField]
    private UnityEngine.UI.Text TimerText;

    private KatamariApp _app;

	public void Setup(KatamariApp app, object param)
    {
        _app = app;

        GameplayUIController.GameplayViewParams gameplayParams = param as GameplayUIController.GameplayViewParams;

        HighScoreText.text = gameplayParams.HighScore.ToString("N0");

        LevelScoreHub.Setup(app.GetEventManager(), app.GetSoundManager());
        MassUIHub.Setup(app.GetEventManager());

        UpdateTimeLeft(gameplayParams.TimeLimitSeconds);

        _app.GetEventManager().AddListener( GameplayUIController.UpdatedTimeLeftEventName, UpdateTimeLeft);
    }

    public void Teardown()
    {
        LevelScoreHub.Teardown();
        MassUIHub.Teardown();

        _app.GetEventManager().RemoveListener(GameplayUIController.UpdatedTimeLeftEventName, UpdateTimeLeft);
        _app = null;
    }

    public bool UpdateTimeLeft( object p )
    {
        int timeLeft = (int)p;

        if( timeLeft < 20 )
        {
            if( timeLeft <= 5 )
            {
                if (timeLeft == 0)
                {
                    _app.GetSoundManager().PlayCustomSound("Buzzer");
                } else
                {
                    _app.GetSoundManager().PlayCustomSound("Alarm");
                }
            } else
            {
                _app.GetSoundManager().PlayCustomSound("ClockTick");
            }
        }

        TimerText.text = UIHelpers.FormatTime(timeLeft);

        return false;
    }
}
