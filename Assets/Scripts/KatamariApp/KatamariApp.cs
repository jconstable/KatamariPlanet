using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KatamariApp {

    public interface IAppState
    {
        void OnEnter(KatamariApp app);
        void OnExit();
        void OnUpdate(float dt);
    }

    // Managers
    private EventManager _eventManager;
    public EventManager GetEventManager() { return _eventManager; }

    private UIManager _uiManager;
    public UIManager GetUIManager() { return _uiManager; }

    // Models
    private LevelStats _levelStats;
    public LevelStats GetLevelStats() { return _levelStats; }

    // Controllers
    private GameplayUIController _gameplayUIController;
    public GameplayUIController GetGameplayUIController() { return _gameplayUIController; }

    // Game States
    private Dictionary<string, IAppState> _appStates;
    private IAppState _currentAppState;

    // KatamariAppProxy Hooks
    public void Setup()
    {
        // Managers
        _eventManager = new EventManager();
        _uiManager = new UIManager();
        _levelStats = new LevelStats();

        _eventManager.Setup();
        _uiManager.Setup( this );
        _levelStats.Setup(_eventManager);

        // Models
        _levelStats = new LevelStats();
        _levelStats.Setup(_eventManager);

        // Controllers
        _gameplayUIController = new GameplayUIController();
        _gameplayUIController.Setup(this);
        
        // Go to first state
        LoadGameStates();
        SwitchToState(BootGameState.StateKey);
    }

    public void Teardown()
    {
        _appStates.Clear();
        _appStates = null;
        _currentAppState = null;

        _eventManager.Teardown();
        _uiManager.Teardown();

        _eventManager = null;
        _uiManager = null;
    }

    public void OnUpdate( float dt )
    {
        if( _currentAppState != null )
        {
            _currentAppState.OnUpdate(dt);
        }
    }

    public void OnLateUpdate( float dt )
    {
        if(_eventManager != null)
        {
            _eventManager.LateUpdate(dt);
        }
    }

    private void LoadGameStates()
    {
        _appStates = new Dictionary<string, IAppState>();

        GameStates statesData = Resources.Load(Files.AppGameStatesDataPrefabPath) as GameStates;
        for (int i = 0; i < statesData.GameStateClassNames.Length; ++i)
        {
            string stateClassName = statesData.GameStateClassNames[i];

            try
            {
                // A little bit of reflection
                IAppState state = System.Activator.CreateInstance(System.Type.GetType(stateClassName)) as IAppState;
                Debug.Assert(state != null, "Unable to instantiate IAppState class named " + stateClassName);

                if (state != null)
                {
                    _appStates.Add(stateClassName, state);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Unable to create IAppState " + e.Message);
            }
        }
    }

    public void SwitchToState( string stateName )
    {
        IAppState newState = null;

        if( _appStates.TryGetValue( stateName, out newState ) )
        {
            if( _currentAppState != newState )
            {
                if (_currentAppState != null)
                {
                    _currentAppState.OnExit();
                }

                _currentAppState = newState;
                _currentAppState.OnEnter(this);
            }
        }
    }
}
