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
    private SoundManager _soundManager;

    public void Setup( EventManager eventManager, SoundManager soundManager, LevelData.LevelDefinition def, LevelScore score, bool locked )
    {
        LevelNumberLabel.text = def.LevelNumberText;
        LevelNameLabel.text = def.LevelNameText.Replace( "\n", "\n" );

        _eventManager = eventManager;
        _soundManager = soundManager;

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

            if (score.TotalScore == 0)
            {
                HighScoreLabel.text = EmptyScoreText;
            }
            else
            {
                HighScoreLabel.text = score.TotalScore.ToString("N0");
            }

            TimeLabel.text = UIHelpers.FormatTime(def.TimeDuration);

            // Show the appropriate number of stars
            for (int i = 0; i < StarForegrounds.Length; ++i)
            {
                if (i < def.StarPointRequirements.Length)
                {
                    if (def.StarPointRequirements[i] > score.TotalScore)
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
            _soundManager.PlayUISound(UISounds.SoundEvent.MenuForwards);
            _soundManager.PlayUISound(UISounds.SoundEvent.LevelSelect);
            _eventManager.SendEvent(LevelStats.LevelSelectedEventName, _levelID);
        }
    }
}
