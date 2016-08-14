using UnityEngine;
using System.Collections;

public class LevelStats : MonoBehaviour { 

    public static readonly string AddScoreEventName = "LevelStatsAddScoreEventName";
    public static readonly string UpdatedScoreEventName = "LevelStatsUpdatedScoreEventName";
    public static readonly string UpdatedMassEventName = "LevelStatsUpdatedMassEventName";

    public int CurrentScore = 0;
    public float CurrentMass = 0;

    void Start()
    {
        EventManager.AddListener(AddScoreEventName, ScoreAdded);
        EventManager.AddListener(KatamariCore.MassChangedEventName, UpdateMass);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(AddScoreEventName, ScoreAdded);
        EventManager.RemoveListener(KatamariCore.MassChangedEventName, UpdateMass);
    }

    bool ScoreAdded( object p )
    {
        int add = (int)p;

        CurrentScore += add;

        // Notify the game that the new score has been calculated
        EventManager.SendEvent(UpdatedScoreEventName, CurrentScore);

        return false;
    }

    bool UpdateMass( object p )
    {
        CurrentMass = (float)p;

        // Notify the game that the new mass has been calculated
        EventManager.SendEvent(UpdatedMassEventName, CurrentMass);

        return false;
    }
}
