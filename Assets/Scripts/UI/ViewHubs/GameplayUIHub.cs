using UnityEngine;
using System.Collections;

public class GameplayUIHub : MonoBehaviour, UIManager.IUIScreen {

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "level.hud";

    [SerializeField]
    private LevelScoreHub LevelScoreHub;

    [SerializeField]
    private MassUIHub MassUIHub;

	public void Setup(KatamariApp app, object param)
    {
        EventManager eventManager = app.GetEventManager();
        LevelScoreHub.Setup(eventManager);
        MassUIHub.Setup(eventManager);
    }

    public void Teardown()
    {
        LevelScoreHub.Teardown();
        MassUIHub.Teardown();
    }
}
