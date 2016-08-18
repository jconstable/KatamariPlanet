using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelScore {
    public string LevelID { get; set; }

    public int TimeRemaining { get; set; }

    public int BonusScore { get; set; }

    public int BaseScore { get; set; }

    public int TotalScore { get; set; }

    public bool NewHighScore { get; set; }
}
