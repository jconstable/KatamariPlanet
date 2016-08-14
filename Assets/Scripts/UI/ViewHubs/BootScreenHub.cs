using UnityEngine;
using System.Collections;

public class BootScreenHub : MonoBehaviour, UIManager.IUIScreen {

    // Reference string by which this UI element will be identified by code and in the UIKeysToPrefabs data
    public static readonly string UIKey = "boot";

    public void Setup( KatamariApp app )
    {

    }

    public void Teardown()
    {

    }
}
