using UnityEngine;
using System.Collections;

public class GameplayUIController {
    public static readonly string UpdatedTimeLeftEventName = "GameplayTimeLeftUpdated";
    public static readonly string LeaveGameplayEventName = "LeaveGameplayEvent";

    private KatamariApp _app;
    private float _timeStarted = 0f;
    private int _secondsInGameplay = 0;
    private int _lastSecondsLeft = 0;

    public class GameplayViewParams
    {
        public int HighScore;
        public int TimeLimitSeconds;
    }

    public void Setup( KatamariApp app )
    {
        _app = app;

        _app.GetEventManager().AddListener(LeaveGameplayEventName, LeaveGameplay);
    }

    public void Teardown()
    {
        _app.GetEventManager().RemoveListener(LeaveGameplayEventName, LeaveGameplay);

        _app = null;
    }

    public int ShowGameplayUI()
    {
        PlayerProfile profile = _app.GetPlayerProfile();
        LevelData.LevelDefinition def = _app.CurrentlySelectedLevel;
        LevelScore score = profile.GetLevelScore(def.LevelID);

        _secondsInGameplay = def.TimeDuration;

        GameplayViewParams p = new GameplayViewParams()
        {
            HighScore = score.HighScore,
            TimeLimitSeconds = _secondsInGameplay
        };

        StartTimer();

        return _app.GetUIManager().LoadUI(GameplayUIHub.UIKey, p, (int)UILayers.Layers.DefaultUI);
    }

    public int ShowGameplayResultsUI()
    {
        PlayerProfile profile = _app.GetPlayerProfile();
        LevelData.LevelDefinition def = _app.CurrentlySelectedLevel;
        LevelScore score = profile.GetLevelScore(def.LevelID);

        return _app.GetUIManager().LoadUI( GameplayResultsUIHub.UIKey, score, (int)UILayers.Layers.DefaultUI );
    }

    public void StartTimer()
    {
        _timeStarted = Time.time;
    }

    public void OnUpdate( float dt )
    {
        int secondsLeft = _secondsInGameplay - (int)(Time.time - _timeStarted);

        if(secondsLeft >= 0 && secondsLeft != _lastSecondsLeft)
        {
            _lastSecondsLeft = secondsLeft;
            _app.GetEventManager().SendEvent(UpdatedTimeLeftEventName, secondsLeft);

            if( secondsLeft == 0 )
            {
                _app.GetEventManager().SendEvent(PlayGameState.GameplayOverEventName, null);
            }
        }
    }

    public bool LeaveGameplay( object p )
    {
        UIHelpers.FadeToUIAction(_app, () => {
            _app.SwitchToState(typeof(LevelSelectState).ToString());
        });

        return false;
    }
}
