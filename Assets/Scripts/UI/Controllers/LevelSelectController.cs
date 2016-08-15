using UnityEngine;
using System.Collections;

public class LevelSelectController : MonoBehaviour {

    private KatamariApp _app;

    public void Setup(KatamariApp app)
    {
        _app = app;
    }

    public void Teardown()
    {
        _app = null;
    }

    public void ShowLevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Files.LevelSelectSceneName);

        LevelData data = _app.GetLevelData();
        PlayerProfile profile = _app.GetPlayerProfile();

        LevelSelectUIHub.LevelSelectUIParams param = new LevelSelectUIHub.LevelSelectUIParams()
        {
            data = data,
            profile = profile
        };

        _app.GetUIManager().LoadUI(LevelSelectUIHub.UIKey, param, (int)UILayers.Layers.DefaultUI);
    }
}
