using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KatamariApp {
    
    // Managers
    private EventManager _eventManager;
    public EventManager GetEventManager() { return _eventManager; }

    private UIManager _uiManager;
    public UIManager GetUIManager() { return _uiManager; }

    private SoundManager _soundManager;
    public SoundManager GetSoundManager() { return _soundManager; }

    // Models
    private LevelStats _levelStats;
    public LevelStats GetLevelStats() { return _levelStats; }

    private LevelData _levelData;
    public LevelData GetLevelData() { return _levelData; }

    private PlayerProfile _profile;
    public PlayerProfile GetPlayerProfile() { return _profile;  }

    // Controllers
    private GameplayUIController _gameplayUIController;
    public GameplayUIController GetGameplayUIController() { return _gameplayUIController; }

    private BootScreenController _bootScreenController;
    public BootScreenController GetBootScreenController() { return _bootScreenController; }

    private FadeUIController _fadeUIController;
    public FadeUIController GetFadeUIController() { return _fadeUIController; }

    private LevelSelectController _levelSelectController;
    public LevelSelectController GetLevelSelectController() { return _levelSelectController; }

    private PopupUIController _popupUIController;
    public PopupUIController GetPopupUIController() { return _popupUIController; }

    // Game States
    private Dictionary<string, GameState> _appStates;
    private GameState _currentAppState;

    private KatamariAppProxy _proxy = null;
    public KatamariAppProxy KatamariAppProxy { get { return _proxy; } }

    public LevelData.LevelDefinition CurrentlySelectedLevel { get; set; }

    // KatamariAppProxy Hooks
    public void Setup( KatamariAppProxy proxy, int musicChannels, int sfxChannels )
    {
        Application.logMessageReceived += LogMessage;

        _proxy = proxy;

        // Managers
        _eventManager = new EventManager();
        _uiManager = new UIManager();
        _soundManager = new SoundManager();
        
        _eventManager.Setup();
        _uiManager.Setup( this );
        _soundManager.Setup(this, musicChannels, sfxChannels);

        // Models
        _levelData = Resources.Load(Files.LevelDataResourcePath) as LevelData;
        DebugUtils.Assert(_levelData != null, "Unable to load LevelData from resource path " + Files.LevelDataResourcePath);

        _profile = new PlayerProfile();
        _profile.Setup(_levelData);

        _levelStats = new LevelStats();
        _levelStats.Setup(_eventManager,_soundManager);
        
        // Controllers
        _gameplayUIController = new GameplayUIController();
        _bootScreenController = new BootScreenController();
        _fadeUIController = new FadeUIController();
        _levelSelectController = new LevelSelectController();
        _popupUIController = new PopupUIController();

        _gameplayUIController.Setup(this);
        _bootScreenController.Setup(this);
        _fadeUIController.Setup(this);
        _levelSelectController.Setup(this);
        _popupUIController.Setup(this);
        
        // Go to first state
        LoadGameStates();
        SwitchToState( typeof(BootGameState).ToString() );
    }

    public void Teardown()
    {
        _gameplayUIController.Teardown();
        _bootScreenController.Teardown();
        _fadeUIController.Teardown();
        _levelSelectController.Teardown();
        _popupUIController.Teardown();

        _appStates.Clear();
        _appStates = null;
        _currentAppState = null;

        _eventManager.Teardown();
        _uiManager.Teardown();

        _eventManager = null;
        _uiManager = null;

        Application.logMessageReceived -= LogMessage;
    }

    public void OnUpdate( float dt )
    {
        if( _currentAppState != null )
        {
            _currentAppState.OnUpdate(dt);
        }
        if( _soundManager != null )
        {
            _soundManager.OnUpdate(dt);
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
        _appStates = new Dictionary<string, GameState>();

        GameStates statesData = Resources.Load(Files.AppGameStatesDataPrefabPath) as GameStates;
        for (int i = 0; i < statesData.GameStateDataList.Count; ++i)
        {
            GameStates.GameStateData data = statesData.GameStateDataList[i];

            try
            {
                // A little bit of reflection
                GameState state = System.Activator.CreateInstance(System.Type.GetType( data.ClassName )) as GameState;
                DebugUtils.Assert(state != null, "Unable to instantiate IAppState class named " + data.ClassName);

                if (state != null)
                {
                    _appStates.Add(data.ClassName, state);

                    // Build a Dictionary from the list that was setup in the data ScriptableObject
                    Dictionary<string, Object> refDict = new Dictionary<string, Object>();
                    for( int r = 0; r < data.ObjectRefs.Count; ++r )
                    {
                        GameStates.GameStateRef ro = data.ObjectRefs[r];
                        refDict.Add(ro.RefName, ro.Ref);
                    }

                    state.Setup(refDict);
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
        GameState newState = null;

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
        } else
        {
            Debug.Log("Unable to find game state: " + stateName);
        }
    }

    void LogMessage( string message, string stackTrace, LogType logType )
    {
        if( logType == LogType.Error || logType == LogType.Exception )
        {
            _popupUIController.ShowUnhandledExceptionPopup(message, stackTrace);

            Debug.LogError(message);
        } else
        {
            Debug.Log(message);
        }
    }
}
