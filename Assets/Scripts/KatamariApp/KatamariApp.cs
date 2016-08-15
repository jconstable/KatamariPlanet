using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KatamariApp {

    public interface IAppState
    {
        void Setup(Dictionary<string, string> refs);
        void OnEnter(KatamariApp app);
        void OnExit();
        void OnUpdate(float dt);
    }

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

    // Game States
    private Dictionary<string, IAppState> _appStates;
    private IAppState _currentAppState;

    private KatamariAppProxy _proxy = null;
    public KatamariAppProxy KatamariAppProxy { get { return _proxy; } }

    // KatamariAppProxy Hooks
    public void Setup( KatamariAppProxy proxy )
    {
        _proxy = proxy;

        // Managers
        _eventManager = new EventManager();
        _uiManager = new UIManager();
        _soundManager = new SoundManager();
        
        _eventManager.Setup();
        _uiManager.Setup( this );
        _soundManager.Setup(this, 2); // Hardcoding 2 channels

        // Models
        _levelStats = new LevelStats();
        _levelStats.Setup(_eventManager);

        _levelData = Resources.Load(Files.LevelDataResourcePath) as LevelData;
        Debug.Assert(_levelData != null, "Unable to load LevelData from resource path " + Files.LevelDataResourcePath);

        _profile = new PlayerProfile();
        _profile.Setup(_levelData);

        // Controllers
        _gameplayUIController = new GameplayUIController();
        _bootScreenController = new BootScreenController();
        _fadeUIController = new FadeUIController();
        _levelSelectController = new LevelSelectController();

        _gameplayUIController.Setup(this);
        _bootScreenController.Setup(this);
        _fadeUIController.Setup(this);
        _levelSelectController.Setup(this);
        
        // Go to first state
        LoadGameStates();
        SwitchToState(BootGameState.StateKey);
    }

    public void Teardown()
    {
        _fadeUIController.Teardown();

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
        _appStates = new Dictionary<string, IAppState>();

        GameStates statesData = Resources.Load(Files.AppGameStatesDataPrefabPath) as GameStates;
        for (int i = 0; i < statesData.GameStateDataList.Count; ++i)
        {
            GameStates.GameStateData data = statesData.GameStateDataList[i];

            try
            {
                // A little bit of reflection
                IAppState state = System.Activator.CreateInstance(System.Type.GetType( data.ClassName )) as IAppState;
                Debug.Assert(state != null, "Unable to instantiate IAppState class named " + data.ClassName);

                if (state != null)
                {
                    _appStates.Add(data.ClassName, state);

                    // Build a Dictionary from the list that was setup in the data ScriptableObject
                    Dictionary<string, string> refDict = new Dictionary<string, string>();
                    for( int r = 0; r < data.ObjectRefs.Count; ++r )
                    {
                        GameStates.GameStateRef ro = data.ObjectRefs[i];
                        refDict.Add(ro.RefName, ro.RefPath);
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
