using UnityEngine;
using System.Collections;

public class LevelSelectItemHub : MonoBehaviour {
    public static readonly string EmptyTimeText = "-:--";
    public static readonly string EmptyScoreText = "--";

    [SerializeField]
    private UnityEngine.UI.Text LevelNumberLabel;
    [SerializeField]
    private UnityEngine.UI.Text LevelNameLabel;
    [SerializeField]
    private UnityEngine.UI.Text TimeLabel;
    [SerializeField]
    private UnityEngine.UI.Text HighScoreLabel;

    [SerializeField]
    private UnityEngine.UI.Graphic[] StarBackgrounds;
    [SerializeField]
    private UnityEngine.UI.Graphic[] StarForegrounds;

    public void Setup( LevelData.LevelDefinition def, LevelScore score, bool locked )
    {
        LevelNumberLabel.text = def.LevelNumberText;
        LevelNameLabel.text = def.LevelNameText.Replace( "\n", "\n" );

        if (locked)
        {
            for (int i = 0; i < StarBackgrounds.Length; ++i)
            {
                StarBackgrounds[i].enabled = false;
                StarForegrounds[i].enabled = false;
            }

            TimeLabel.text = EmptyTimeText;
            HighScoreLabel.text = EmptyScoreText;
        }
        else
        {
            if (score.HighScore == 0)
            {
                HighScoreLabel.text = EmptyScoreText;
            }
            else
            {
                HighScoreLabel.text = score.HighScore.ToString("N0");
            }

            TimeLabel.text = UIHelpers.FormatTime(def.TimeDuration);

            // Show the appropriate number of stars
            for (int i = 0; i < StarForegrounds.Length; ++i)
            {
                if (i < def.StarPointRequirements.Length)
                {
                    if (def.StarPointRequirements[i] > score.HighScore)
                    {
                        StarForegrounds[i].enabled = false;
                    }
                } else
                {
                    Debug.LogError("Level Definition " + def.LevelID + " does not define star requirement " + i.ToString());
                }
            }
        }
    }
}
