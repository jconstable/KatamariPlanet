using UnityEngine;
using System.Collections;

public class LevelSelectState : GameState
{
    public override void OnEnter(KatamariApp app)
    {
        base.OnEnter(app);

        UnityEngine.SceneManagement.SceneManager.LoadScene(Files.LevelSelectSceneName);

        _app.GetLevelSelectController().ShowLevelSelect();

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
    }

    public override void OnUpdate(float dt)
    {
    }
}