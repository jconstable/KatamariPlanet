using UnityEngine;
using System.Collections;

public class GameplayUIHub : MonoBehaviour, UIManager.IUIScreen {

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "level.hud";

	public void Setup()
    {

    }

    public void Teardown()
    {

    }
}
