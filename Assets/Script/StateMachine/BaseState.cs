using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine machine;
    public BaseState(StateMachine machine)
    {
        this.machine = machine;
    }
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void StateUpdate();
}
