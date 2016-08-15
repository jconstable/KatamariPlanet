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
    }

    public virtual void OnExit()
    {
        _app = null;
    }

    public abstract void OnUpdate(float dt);
}
