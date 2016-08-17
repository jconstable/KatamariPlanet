using UnityEngine;
using System.Collections;

public class LevelStats {
    public static readonly string LevelSelectedEventName = "LevelSelected";

    public static readonly string SwallowableObjectAddedEventName = "SwallowableObjectAdded";
    public static readonly string SwallowableObjectSwallowedEventName = "SwallowableObjectSwallowed";

    public static readonly string AddScoreEventName = "LevelStatsAddScoreEventName";
    public static readonly string UpdatedScoreEventName = "LevelStatsUpdatedScoreEventName";
    public static readonly string UpdatedMassEventName = "LevelStatsUpdatedMassEventName";

    public int CurrentScore = 0;
    public float CurrentMass = 0;

    public int RemainingObjects { get; private set; }
    public int SecondsLeft { get; private set; }

    private EventManager _eventManager;
    private SoundManager _soundManager;

    public void Setup( EventManager eventManager, SoundManager soundManager )
    {
        _eventManager = eventManager;
        _soundManager = soundManager;
        _eventManager.AddListener(AddScoreEventName, ScoreAdded);
        _eventManager.AddListener(KatamariCore.MassChangedEventName, UpdateMass);
        _eventManager.AddListener(SwallowableObjectAddedEventName, AddSwallowableObject);
        _eventManager.AddListener(SwallowableObjectSwallowedEventName, ObjectSwallowed);
        _eventManager.AddListener(LevelPlayState.UpdatedTimeLeftEventName, UpdateSecondsLeft);
    }

    public void Teardown()
    {
        _eventManager.RemoveListener(AddScoreEventName, ScoreAdded);
        _eventManager.RemoveListener(KatamariCore.MassChangedEventName, UpdateMass);
        _eventManager.RemoveListener(SwallowableObjectAddedEventName, AddSwallowableObject);
        _eventManager.RemoveListener(SwallowableObjectSwallowedEventName, ObjectSwallowed);
        _eventManager.RemoveListener(LevelPlayState.UpdatedTimeLeftEventName, UpdateSecondsLeft);

        _eventManager = null;
        _soundManager = null;
    }

    public void Reset()
    {
        CurrentScore = 0;
        CurrentMass = 0;
        RemainingObjects = 0;
    }

    bool AddSwallowableObject( object p )
    {
        RemainingObjects++;

        GameObject o = p as GameObject;
        Debug.Log("Added swallowable object " + RemainingObjects.ToString() + ": " + o.name);

        return false;
    }

    bool ObjectSwallowed(object p)
    {
        RemainingObjects--;

        GameObject o = p as GameObject;
        Debug.Log("Swallowed object: " + o.name + ", " + RemainingObjects.ToString() + " remain");

        if( RemainingObjects == 0 )
        {
            _soundManager.PlayCustomSound("Tada");
            _eventManager.SendEvent(LevelPlayState.GameplayOverEventName, null);
        }

        return false;
    }

    bool ScoreAdded( object p )
    {
        int add = (int)p;

        CurrentScore += add;

        // Notify the game that the new score has been calculated
        _eventManager.SendEvent(UpdatedScoreEventName, CurrentScore);

        return false;
    }

    bool UpdateMass( object p )
    {
        CurrentMass = (float)p;

        // Notify the game that the new mass has been calculated
        _eventManager.SendEvent(UpdatedMassEventName, CurrentMass);

        return false;
    }

    bool UpdateSecondsLeft( object p )
    {
        SecondsLeft = (int)p;

        return false;
    }
}
