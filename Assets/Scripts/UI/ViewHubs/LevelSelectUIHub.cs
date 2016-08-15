using UnityEngine;
using System.Collections;

public class LevelSelectUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "level.select";

    public class LevelSelectUIParams
    {
        public LevelData data;
        public PlayerProfile profile;
    }

    [SerializeField]
    private LevelSelectItemHub LevelSelectItemTemplate;

    private KatamariApp _app;
    private LevelSelectUIParams _params;

    public void Setup(KatamariApp app, object param)
    {
        _app = app;

        _params = param as LevelSelectUIParams;
        Popuplate();
    }

    public void Teardown()
    {
        _app = null;
    }

    private void Popuplate()
    {
        // Make sure the template object is deactivated. It exists only for authoring/cloning
        LevelSelectItemTemplate.gameObject.SetActive(false);

        PlayerProfile player = _app.GetPlayerProfile();
        RectTransform templateRect = LevelSelectItemTemplate.GetComponent<RectTransform>();

        Transform parent = LevelSelectItemTemplate.transform.parent;
        RectTransform parentRect = parent.GetComponent<RectTransform>();

        for ( int i = 0; i < _params.data.Levels.Length; ++i )
        {
            LevelData.LevelDefinition def = _params.data.Levels[i];
            LevelScore score = player.GetLevelScore(def.LevelID);

            GameObject newCellOb = GameObject.Instantiate(LevelSelectItemTemplate.gameObject) as GameObject;
            newCellOb.SetActive(true);

            bool locked = DetermineIfLocked(def, player);

            LevelSelectItemHub item = newCellOb.GetComponent<LevelSelectItemHub>();
            item.Setup(def, score, locked);

            // Parent the newly created cell to the scroll view
            newCellOb.transform.SetParent(parent, false);

            newCellOb.transform.localPosition += Vector3.up * -templateRect.rect.size.y * i;
        }
    }

    // Look up the level that is marked as a dependency, and if there is one, make sure the player has scored enough stars
    private bool DetermineIfLocked(LevelData.LevelDefinition def, PlayerProfile player)
    {
        bool locked = true;
        if (string.IsNullOrEmpty(def.DependentLevelID))
        {
            locked = false;
        }
        else
        {
            LevelData.LevelDefinition dependencyDef = _params.data.FindByLevelID(def.DependentLevelID);
            LevelScore dependency = player.GetLevelScore(def.DependentLevelID);
            if (dependency.HighScore > dependencyDef.StarPointRequirements[def.RequiredStarsInDependencyLevel])
            {
                locked = false;
            }
        }

        return locked;
    }
}
