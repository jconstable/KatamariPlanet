using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelPlayState : GameState
{
    public static readonly string UpdatedTimeLeftEventName = "GameplayTimeLeftUpdated";
    public static readonly string GameplayOverEventName = "GameplayOverEvent";

    private float _timeStarted = 0f;
    private int _secondsInGameplay = 0;
    private int _lastSecondsLeft = 0;

    public override void OnEnter(KatamariApp app)
    {
        base.OnEnter(app);

        // Reset the app's game stat tracker
        _app.GetLevelStats().Reset();

        // Grab the current level definition and set it up
        LevelData.LevelDefinition def = app.CurrentlySelectedLevel;
        _secondsInGameplay = def.TimeDuration;

        // Load the level's scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(def.SceneName);
        
        // Show the gameplay UI
        _app.GetGameplayUIController().ShowGameplayUI( def );

        StartTimer();

        _app.GetEventManager().AddListener(GameplayOverEventName, OnLevelEnded);
    }

    public override void OnExit()
    {
        _app.GetEventManager().RemoveListener(GameplayOverEventName, OnLevelEnded);

        base.OnExit();
    }

    bool OnLevelEnded(object param)
    {
        _app.SwitchToState(typeof(LevelResultsState).ToString());

        return false;
    }

    public void StartTimer()
    {
        _timeStarted = Time.time;
    }

    public override void OnUpdate( float dt )
    {
        int secondsLeft = _secondsInGameplay - (int)(Time.time - _timeStarted);

        if (secondsLeft >= 0 && secondsLeft != _lastSecondsLeft)
        {
            _lastSecondsLeft = secondsLeft;
            _app.GetEventManager().SendEvent(UpdatedTimeLeftEventName, secondsLeft);

            if (secondsLeft == 0)
            {
                _app.GetEventManager().SendEvent(LevelPlayState.GameplayOverEventName, null);
            }
        }
    }
}
