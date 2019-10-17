using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine<T>
{
    public State<T> _currentState { get; private set; }


    private List<State<T>> _stateList;

    public StateMachine()
    {
        _stateList = new List<State<T>>();   
    }

    public void RegisterState(State<T> state)
    {
        if (!_stateList.Contains(state))
            _stateList.Add(state);
    }

    public bool SetState(T state)
    {
        foreach (State<T> registerdState in _stateList)
        {
            if(EqualityComparer<T>.Default.Equals(registerdState.name,state))
            {
                _currentState = registerdState;
                _currentState.Dispatch();
                return true;
            }
        }
        return false;
    }

    public State<T> Find(T state)
    {
        foreach (State<T> registerdState in _stateList)
            if (EqualityComparer<T>.Default.Equals(registerdState.name, state))
                return registerdState;


        return null;
    }


    


}
