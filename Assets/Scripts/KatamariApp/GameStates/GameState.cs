using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameState {
    protected KatamariApp _app;
    protected Dictionary<string, Object> _refs;

    public virtual void Setup(Dictionary<string, Object> refs)
    {
        _refs = refs;
    }

    public virtual void OnEnter(KatamariApp app)
    {
        _app = app;

        Object musicAssetRef = null;
        if (_refs.TryGetValue("music", out musicAssetRef))
        {
            AudioClip musicAsset = musicAssetRef as AudioClip;
            if( musicAsset == null )
            {
                Debug.Log("Hmm");

            }
            DebugUtils.Assert(musicAsset != null, "Unable to cast 'music' asset ref to AudioClip");

            if (musicAsset != null)
            {
                _app.GetSoundManager().PlayMusic(musicAsset, 0.2f, 2f);
            }
        }
        else
        {
            Debug.LogWarning(this.GetType().ToString() + ": No music ref definied in game state refs");
        }
    }

    public virtual void OnExit()
    {
        _app = null;
    }

    public virtual void OnUpdate(float dt)
    {

    }
}
