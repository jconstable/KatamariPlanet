using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BootGameState : KatamariApp.IAppState {

    public static readonly string StateKey = "BootGameState";

    private KatamariApp _app;
    private Dictionary<string, string> _refs;

    public void Setup( Dictionary<string, string> refs )
    {
        _refs = refs;
    }

    public void OnEnter(KatamariApp app)
    {
        _app = app;
        string musicAssetPath = null;
        if( _refs.TryGetValue( "music", out musicAssetPath ) )
        {
            _app.GetSoundManager().PlayMusic(musicAssetPath, 1f, 2f);
        } else
        {
            Debug.LogWarning("BootGameState: No music ref definied in game state refs");
        }

        _app.GetBootScreenController().ShowBootScreen();
        
    }

    public void OnExit()
    {
        _app = null;
    }

    public void OnUpdate(float dt)
    {
    }
}
