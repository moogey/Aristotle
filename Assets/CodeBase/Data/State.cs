using System.Collections.Generic;
using System;
public class State<T> : AEvent
{
    public T name { get; private set; }


    public State(T stateName) 
    {
        name = stateName;
    }

}
