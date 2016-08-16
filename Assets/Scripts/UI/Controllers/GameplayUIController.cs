using UnityEngine;
using System.Collections;

public class GameplayUIController {
    public static readonly string LeaveGameplayEventName = "LeaveGameplayEvent";

    private KatamariApp _app;

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

    public int ShowGameplayUI( LevelData.LevelDefinition def )
    {
        PlayerProfile profile = _app.GetPlayerProfile();
        LevelScore score = profile.GetLevelScore(def.LevelID);

        GameplayViewParams p = new GameplayViewParams()
        {
            HighScore = score.HighScore,
            TimeLimitSeconds = def.TimeDuration
        };

        return _app.GetUIManager().LoadUI(GameplayUIHub.UIKey, p, (int)UILayers.Layers.DefaultUI);
    }

    public int ShowGameplayResultsUI()
    {
        PlayerProfile profile = _app.GetPlayerProfile();
        LevelData.LevelDefinition def = _app.CurrentlySelectedLevel;
        LevelScore score = profile.GetLevelScore(def.LevelID);

        return _app.GetUIManager().LoadUI( GameplayResultsUIHub.UIKey, score, (int)UILayers.Layers.DefaultUI );
    }
    
    public bool LeaveGameplay( object p )
    {
        UIHelpers.FadeToUIAction(_app, () => {
            _app.SwitchToState(typeof(LevelSelectState).ToString());
        });

        return false;
    }
}
