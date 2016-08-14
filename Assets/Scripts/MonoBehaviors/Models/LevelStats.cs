using UnityEngine;
using System.Collections;

public class LevelStats : MonoBehaviour { 

    public static readonly string AddScoreEventName = "LevelStatsAddScoreEventName";
    public static readonly string UpdatedScoreEventName = "LevelStatsUpdatedScoreEventName";

    public int CurrentScore = 0;

    void Start()
    {
        EventManager.AddListener(AddScoreEventName, ScoreAdded);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(AddScoreEventName, ScoreAdded);
    }

    bool ScoreAdded( object p )
    {
        int add = (int)p;

        CurrentScore += add;

        // Notify the game that the new score has been calculated
        EventManager.SendEvent(UpdatedScoreEventName, CurrentScore);

        return false;
    }
}
