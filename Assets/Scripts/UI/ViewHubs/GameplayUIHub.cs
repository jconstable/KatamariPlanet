using UnityEngine;
using System.Collections;

public class GameplayUIHub : MonoBehaviour, UIManager.IUIScreen {

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "level.hud";

    [SerializeField]
    private LevelScoreHub LevelScoreHub;

    [SerializeField]
    private MassUIHub MassUIHub;

    [SerializeField]
    private UnityEngine.UI.Text HighScoreText;

	public void Setup(KatamariApp app, object param)
    {
        int highScore = (int)param;
        HighScoreText.text = highScore.ToString("N0");

        LevelScoreHub.Setup(app.GetEventManager(), app.GetSoundManager());
        MassUIHub.Setup(app.GetEventManager());
    }

    public void Teardown()
    {
        LevelScoreHub.Teardown();
        MassUIHub.Teardown();
    }
}
