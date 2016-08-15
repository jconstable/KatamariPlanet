using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayGameState : GameState
{
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
        _app.GetFadeUIController().FadeOut( null );

    }

    public override void OnUpdate(float dt)
    {
    }
}
