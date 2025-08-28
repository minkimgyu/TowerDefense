using System;
using UnityEngine;

abstract public class BaseState<T>
{
    protected FSM<T> _fsm;
    public BaseState(FSM<T> fsm) 
    { 
        _fsm = fsm; 
    }

    public virtual void OnStateEnter(AreaData data) { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }
}
