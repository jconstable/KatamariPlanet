using UnityEngine;
using System.Collections;

public class LevelStats { 

    public static readonly string AddScoreEventName = "LevelStatsAddScoreEventName";
    public static readonly string UpdatedScoreEventName = "LevelStatsUpdatedScoreEventName";
    public static readonly string UpdatedMassEventName = "LevelStatsUpdatedMassEventName";

    public int CurrentScore = 0;
    public float CurrentMass = 0;

    private EventManager _eventManager;

    public void Setup( EventManager eventManager )
    {
        _eventManager = eventManager;
        _eventManager.AddListener(AddScoreEventName, ScoreAdded);
        _eventManager.AddListener(KatamariCore.MassChangedEventName, UpdateMass);
    }

    public void Teardown()
    {
        _eventManager.RemoveListener(AddScoreEventName, ScoreAdded);
        _eventManager.RemoveListener(KatamariCore.MassChangedEventName, UpdateMass);
        _eventManager = null;
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
}
