using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile {

    private Dictionary<string, LevelScore> _levelScores;
    
    public void Setup( LevelData levels )
    {
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
                HighScore = 0
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

    public void UpdateLevelScore( string levelID, int newScore )
    {
        LevelScore score = GetLevelScore(levelID);
        
        if ( score.HighScore < newScore )
        {
            score.HighScore = newScore;
            score.NewHighScore = true;
        } else
        {
            score.NewHighScore = false;
        }
    }
}
