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

    [SerializeField]
    private UnityEngine.UI.Graphic ProgressBarFill;
    private RectTransform ProgressBarRect;

    [SerializeField]
    private Color ProgressBarFull;
    [SerializeField]
    private Color ProgressBarEmpty;

    private KatamariApp _app;
    private float _progressBarFullWidth = 0f;
    private float _totalTime = 0f;
    private float _startTime = 0;

	public void Setup(KatamariApp app, object param)
    {
        _app = app;

        GameplayUIController.GameplayViewParams gameplayParams = param as GameplayUIController.GameplayViewParams;
        DebugUtils.Assert(gameplayParams != null, "No gameplayParams passed into GameplayUIHub");

        if (gameplayParams != null )
        {
            HighScoreText.text = gameplayParams.HighScore.ToString("N0");

            LevelScoreHub.Setup(app.GetEventManager(), app.GetSoundManager());
            MassUIHub.Setup(app.GetEventManager());

            _totalTime = (float)gameplayParams.TimeLimitSeconds;
            UpdateTimeLeft(gameplayParams.TimeLimitSeconds);

            ProgressBarRect = ProgressBarFill.GetComponent<RectTransform>();
            _progressBarFullWidth = (float)(ProgressBarRect.rect.width);
            _startTime = Time.time;
        }

        _app.GetEventManager().AddListener(LevelPlayState.UpdatedTimeLeftEventName, UpdateTimeLeft);
    }

    public void Teardown()
    {
        LevelScoreHub.Teardown();
        MassUIHub.Teardown();

        _app.GetEventManager().RemoveListener(LevelPlayState.UpdatedTimeLeftEventName, UpdateTimeLeft);
        _app = null;
    }

    void Update()
    {
        float t = (Time.time - _startTime) / _totalTime;
        ProgressBarFill.color = Color.Lerp(ProgressBarFull, ProgressBarEmpty, t);
        ProgressBarRect.sizeDelta = new Vector2(-_progressBarFullWidth * t, 1f);
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
