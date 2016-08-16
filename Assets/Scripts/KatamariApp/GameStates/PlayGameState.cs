using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayGameState : GameState
{
    public static readonly string GameplayOverEventName = "GameplayOverEvent";

    public override void OnEnter(KatamariApp app)
    {
        base.OnEnter(app);

        Object musicAssetRef = null;
        if (_refs.TryGetValue("music", out musicAssetRef))
        {
            AudioClip musicAsset = musicAssetRef as AudioClip;
            Debug.Assert(musicAsset != null, "Unable to cast 'music' asset ref to AudioClip");

            if (musicAsset != null)
            {
                _app.GetSoundManager().PlayMusic(musicAsset, 1f, 2f);
            }
        }
        else
        {
            Debug.LogWarning("BootGameState: No music ref definied in game state refs");
        }

        LevelData.LevelDefinition def = app.CurrentlySelectedLevel;

        UnityEngine.SceneManagement.SceneManager.LoadScene(def.SceneName);

        _app.GetGameplayUIController().ShowGameplayUI();

        _app.GetEventManager().AddListener(GameplayOverEventName, OnLevelEnded);
    }

    public override void OnExit()
    {
        _app.GetEventManager().AddListener(GameplayOverEventName, OnLevelEnded);

        base.OnExit();
    }

    public override void OnUpdate(float dt)
    {
    }

    bool OnLevelEnded(object param)
    {
        LevelStats stats = _app.GetLevelStats();
        _app.GetPlayerProfile().UpdateLevelScore( _app.CurrentlySelectedLevel.LevelID, stats.CurrentScore);

        _app.GetGameplayUIController().ShowGameplayResultsUI();

        return false;
    }
}
