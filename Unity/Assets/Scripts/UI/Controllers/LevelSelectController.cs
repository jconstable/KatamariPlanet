﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectController {

    private KatamariApp _app;

    public class LevelSelectParams
    {
        public class ParamSet
        {
            public LevelData.LevelDefinition levelDef;
            public LevelScore playerScore;
            public bool locked;
        }
        public List<ParamSet> Sets;
    }

    public void Setup(KatamariApp app)
    {
        _app = app;

        _app.GetEventManager().AddListener(LevelStats.LevelSelectedEventName, OnLevelSelected);
    }

    public void Teardown()
    {
        _app.GetEventManager().RemoveListener(LevelStats.LevelSelectedEventName, OnLevelSelected);

        _app = null;
    }

    public void ShowLevelSelect()
    {
        LevelData data = _app.GetLevelData();
        PlayerProfile profile = _app.GetPlayerProfile();

        // Create the params we will pass to the view
        LevelSelectParams param = new LevelSelectParams()
        {
            Sets = new List<LevelSelectParams.ParamSet>()
        };

        // For each level in data, get the player's score, and whether the player has met the requirements to play
        for (int i = 0; i < data.Levels.Length; ++i)
        {
            LevelData.LevelDefinition def = data.Levels[i];
            LevelScore score = profile.GetLevelScore(def.LevelID);

            bool locked = DetermineIfLocked( data, def, profile);

            param.Sets.Add(new LevelSelectParams.ParamSet()
            {
                levelDef = def,
                playerScore = score,
                locked = locked
            });
        }

        _app.GetUIManager().LoadUI(LevelSelectUIHub.UIKey, param, (int)UILayers.Layers.DefaultUI);
    }

    // Decide if this level should be locked for the player. 
    private bool DetermineIfLocked( LevelData data, LevelData.LevelDefinition def, PlayerProfile player)
    {
        bool locked = true;
        if (string.IsNullOrEmpty(def.DependentLevelID))
        {
            locked = false;
        }
        else
        {
            LevelData.LevelDefinition dependencyDef = data.FindByLevelID(def.DependentLevelID);
            LevelScore dependency = player.GetLevelScore(def.DependentLevelID);
            if (dependency.TotalScore > dependencyDef.StarPointRequirements[def.RequiredStarsInDependencyLevel])
            {
                locked = false;
            }
        }

        return locked;
    }

    private bool OnLevelSelected( object param )
    {
        string levelID = param as string;

        LevelData.LevelDefinition levelDef = _app.GetLevelData().FindByLevelID(levelID);
        if( levelDef != null )
        {
           UIHelpers.FadeToUIAction(_app, () =>
           {
               _app.CurrentlySelectedLevel = levelDef;
               _app.SwitchToState(typeof(LevelPlayState).ToString());
           });
        }

        return false;
    }
}
