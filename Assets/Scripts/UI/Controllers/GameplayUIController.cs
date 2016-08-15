using UnityEngine;
using System.Collections;

public class GameplayUIController {
    private KatamariApp _app;

    public void Setup( KatamariApp app )
    {
        _app = app;
    }

    public void Teardown()
    {
        _app = null;
    }

    public int ShowGameplayUI()
    {
        PlayerProfile profile = _app.GetPlayerProfile();
        LevelData.LevelDefinition def = _app.CurrentlySelectedLevel;
        LevelScore score = profile.GetLevelScore(def.LevelID);

        return _app.GetUIManager().LoadUI(GameplayUIHub.UIKey, score.HighScore, (int)UILayers.Layers.DefaultUI);
    }
}
