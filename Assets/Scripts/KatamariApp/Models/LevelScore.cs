using UnityEngine;
using System.Collections;

public class LevelScore {
    public string LevelID { get; set; }

    public int TimeRemaining { get; set; }

    public int BonusScore { get; set; }

    public int BaseScore { get; set; }

    public int TotalScore { get; set; }

    public bool NewHighScore { get; set; }
}
