using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Controller : MonoBehaviour
{
    [HideInInspector]
    public static Controller instance;
    [HideInInspector]
    public LocalizationController LocalizationController;
    [HideInInspector]
    public Gamestate state { get { return _engineStateMachine._currentState.name; } }

    [SerializeField]
    private LocalizationData _localizationData;
    [SerializeField]
    private string _defaultLanguageCode = "en";
    private Dictionary<string, List<Action>> _eventList;
    private StateMachine<Gamestate> _engineStateMachine;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LocalizationController = new LocalizationController(_localizationData);
        LocalizationController.setLanguage(_defaultLanguageCode);

        _eventList = new Dictionary<string, List<Action>>();
        _engineStateMachine = new StateMachine<Gamestate>();
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.ACTIVE));
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.CUTSCENES));
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.LOADING_STATE));
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.MENU));
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.PLAYER_DEAD));
        _engineStateMachine.RegisterState(new State<Gamestate>(Gamestate.STAGE_END));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            Dispatch(GameEventType.ENGINE_GAME_START);
        else if (Input.GetKeyUp(KeyCode.KeypadEnter))
            Dispatch(GameEventType.ENGINE_GAME_PAUSE);
    }

    public void AddEventListener(string type, Action callback)
    {
        if (!_eventList.ContainsKey(type))
            _eventList[type] = new List<Action>();

        _eventList[type].Add(callback);
    }

    public void RemoveEventListener(string type, Action callback)
    {
        if (_eventList.ContainsKey(type))
            _eventList[type].Remove(callback); 
    }

    public void AddStateListener(Gamestate state, Action callback)
    {
        State<Gamestate> foundState = _engineStateMachine.Find(state);
        if (foundState != null)
            foundState.AddListener(callback);
    }

    public void RemoveStateLisntener(Gamestate state, Action callback)
    {
        State<Gamestate> foundState = _engineStateMachine.Find(state);
        if (foundState != null)
            foundState.RemoveListener(callback);
    }

    public void Dispatch(string type)
    {
        if (type == GameEventType.ENGINE_STATE_CHANGE)
        {
            Debug.Log("State Change is a ReadOnly event");
            return;
        }

        if (_eventList.ContainsKey(type))
            foreach (Action callback in _eventList[type])
                callback.DynamicInvoke();
        else
            Debug.Log("Warning: No currenlty registered listners for event :" + type);

        CheckState(type);
    }

    private void CheckState(string type)
    {
        switch (type)
        {
            case GameEventType.ENGINE_GAME_START: _engineStateMachine.SetState(Gamestate.ACTIVE); break;
            case GameEventType.ENGINE_STAGE_COMPLETE: _engineStateMachine.SetState(Gamestate.STAGE_END); break;
            case GameEventType.ENGINE_LOAD_LEVEL: _engineStateMachine.SetState(Gamestate.LOADING_STATE); break;
            case GameEventType.ENGINE_GAME_PAUSE: _engineStateMachine.SetState(Gamestate.MENU); break;
            case GameEventType.PLAYER_DEATH: _engineStateMachine.SetState(Gamestate.PLAYER_DEAD); break;

            default: return;
        }

        foreach (Action callback in _eventList[GameEventType.ENGINE_STATE_CHANGE])
            callback.DynamicInvoke();


    }
}
