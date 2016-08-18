using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class LevelDefinition
    {
        public string LevelID;
        public string SceneName;
        public int[] StarPointRequirements;
        public int TimeDuration;

        public string LevelNumberText;
        public string LevelNameText;

        public int BonusPointsPerSecondRemaining = 10;

        public string DependentLevelID;
        public int RequiredStarsInDependencyLevel;
    }

    public LevelDefinition[] Levels;

    public LevelDefinition FindByLevelID( string levelID )
    {
        // Currently implemented without cache
        for( int i = 0; i < Levels.Length; ++i )
        {
            LevelDefinition def = Levels[i];
            if( def.LevelID == levelID )
            {
                return def;
            }
        }

        return null;
    }
}
