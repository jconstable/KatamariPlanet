using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class PlayerProfile {
    private Dictionary<string, LevelScore> _levelScores;

    [System.NonSerialized]
    private LevelData _levelData;
    
    public void Setup( LevelData levels )
    {
        _levelData = levels;
        _levelScores = new Dictionary<string, LevelScore>();

        ReadFromDisk();
        
        SetupLevelData(levels);

        SaveToDisk();
    }

    void SetupLevelData( LevelData levels )
    {
        LevelData.LevelDefinition[] definitions = levels.Levels;
        for ( int i = 0; i < definitions.Length; ++i )
        {
            LevelData.LevelDefinition def = definitions[i];
            LevelScore score = GetLevelScore(def.LevelID);

            if( score == null )
            {
                score = new LevelScore()
                {
                    LevelID = def.LevelID,
                    BaseScore = 0,
                    TotalScore = 0,
                    BonusScore = 0,
                    TimeRemaining = 0
                };

                _levelScores.Add(def.LevelID, score);
            };
        }
    }

    public LevelScore GetLevelScore( string levelID )
    {
        LevelScore score = null;

        _levelScores.TryGetValue(levelID, out score);

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

        SaveToDisk();
    }

    string SaveFilePath()
    {
        return Application.persistentDataPath + Files.ProfileSaveFile;
    }

    void SaveToDisk()
    {
        try
        {
            string saveFilePath = SaveFilePath();
            Debug.Log("Saving profile to: " + saveFilePath);

            using (Stream stream = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read) )
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Close();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    void ReadFromDisk()
    {
        string saveFilePath = SaveFilePath();
        if ( System.IO.File.Exists( saveFilePath ) )
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (Stream stream = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PlayerProfile obj = (PlayerProfile)formatter.Deserialize(stream);

                this._levelScores = obj._levelScores;

                stream.Close();
            }
                
        }
    }
}
