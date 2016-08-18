using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectUIHub : MonoBehaviour, UIManager.IUIScreen {

    public static readonly string UIKey = "level.select";
    
    [SerializeField]
    private LevelSelectItemHub LevelSelectItemTemplate;

    public void Setup(KatamariApp app, object param)
    {
        LevelSelectController.LevelSelectParams p = param as LevelSelectController.LevelSelectParams;
        DebugUtils.Assert(p != null, "No LevelSelectParams passed into LevelSelectUIHub");

        if( p != null )
        {
            Popuplate(app.GetEventManager(), app.GetSoundManager(), p.Sets);
        }
    }

    public void Teardown()
    {
    }

    private void Popuplate(EventManager eventManager, SoundManager soundManager, List<LevelSelectController.LevelSelectParams.ParamSet> Sets )
    {
        // Make sure the template object is deactivated. It exists only for authoring/cloning
        LevelSelectItemTemplate.gameObject.SetActive(false);
        Transform parent = LevelSelectItemTemplate.transform.parent;

        for ( int i = 0; i < Sets.Count; ++i )
        {
            LevelSelectController.LevelSelectParams.ParamSet set = Sets[i];

            // Instantiate a copy of the template, and make it active
            GameObject newCellOb = GameObject.Instantiate(LevelSelectItemTemplate.gameObject) as GameObject;
            newCellOb.SetActive(true);

            // Grab the instance's item component, and set it up
            LevelSelectItemHub item = newCellOb.GetComponent<LevelSelectItemHub>();
            item.Setup( eventManager, soundManager, set.levelDef, set.playerScore, set.locked );

            // Parent the newly created cell to the scroll view
            newCellOb.transform.SetParent(parent, false);
        }
    }

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
