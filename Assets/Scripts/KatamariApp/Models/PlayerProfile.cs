using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile {

    private Dictionary<string, LevelScore> _levelScores;
    private LevelData _levelData;
    
    public void Setup( LevelData levels )
    {
        _levelData = levels;
        _levelScores = new Dictionary<string, LevelScore>();

        SetupLevelData(levels);
    }

    void SetupLevelData( LevelData levels )
    {
        LevelData.LevelDefinition[] definitions = levels.Levels;
        for ( int i = 0; i < definitions.Length; ++i )
        {
            LevelData.LevelDefinition def = definitions[i];
            LevelScore score = new LevelScore()
            {
                LevelID = def.LevelID,
                BaseScore = 0,
                TotalScore = 0,
                BonusScore = 0,
                TimeRemaining = 0
            };

            _levelScores.Add(def.LevelID, score);
        }
    }

    public LevelScore GetLevelScore( string levelID )
    {
        LevelScore score = null;

        if( !_levelScores.TryGetValue( levelID, out score ) )
        {
            Debug.LogError("PlayerProfile: Requesting score for undefined level " + levelID);
        }

        return score;
    }

    public void UpdateLevelScore( string levelID, int baseScore, int timeRemaining )
    {
        LevelData.LevelDefinition def = _levelData.FindByLevelID(levelID);
        LevelScore score = GetLevelScore(levelID);

        // Calculate bonus
        int bonus = timeRemaining * def.BonusPointsPerSecondRemaining;
        int total = bonus + baseScore;

        if ( score.TotalScore < total)
        {
            score.BaseScore = baseScore;
            score.BonusScore = bonus;
            score.TotalScore = total;
            score.TimeRemaining = timeRemaining;
            score.NewHighScore = true;
        } else
        {
            score.NewHighScore = false;
        }
    }
}
