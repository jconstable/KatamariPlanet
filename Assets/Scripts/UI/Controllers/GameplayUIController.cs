using UnityEngine;
using System.Collections;

public class GameplayUIController {

    // Singleton, yuck
    private static GameplayUIController _instance;
    public static GameplayUIController instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = new GameplayUIController();
            }

            return _instance;
        }
    }

    public int ShowGameplayUI()
    {
        return UIManager.LoadUI(GameplayUIHub.UIKey, 1);
    }
}
