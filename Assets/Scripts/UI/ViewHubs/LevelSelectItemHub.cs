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

    [SerializeField]
    private UnityEngine.UI.Image BgSprite;

    [SerializeField]
    private Sprite OffSprite;
    [SerializeField]
    private Sprite OnSprite;

    private string _levelID;
    private EventManager _eventManager;

    public void Setup( EventManager eventManager, LevelData.LevelDefinition def, LevelScore score, bool locked )
    {
        LevelNumberLabel.text = def.LevelNumberText;
        LevelNameLabel.text = def.LevelNameText.Replace( "\n", "\n" );

        _eventManager = eventManager;
        
        if (locked)
        {
            BgSprite.sprite = OffSprite;

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
            _levelID = def.LevelID;

            BgSprite.sprite = OnSprite;

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

    public void OnLevelSelected()
    {
        if( !string.IsNullOrEmpty( _levelID ) )
        {
            _eventManager.SendEvent(LevelSelectController.LevelSelectedEventName, _levelID);
        }
    }
}
