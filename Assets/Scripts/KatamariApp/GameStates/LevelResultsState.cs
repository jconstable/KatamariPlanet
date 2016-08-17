using UnityEngine;
using System.Collections;

public class LevelResultsState : GameState { 
    public override void OnEnter( KatamariApp app )
    {
        base.OnEnter( app );

        LevelStats stats = app.GetLevelStats();
        _app.GetPlayerProfile().UpdateLevelScore(_app.CurrentlySelectedLevel.LevelID, stats.CurrentScore, stats.SecondsLeft);

        _app.GetGameplayUIController().ShowGameplayResultsUI();
    }
}
