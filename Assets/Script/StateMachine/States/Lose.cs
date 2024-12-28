using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Lose : BaseState
{
    public Lose(StateMachine machine) : base(machine)
    {
    }

    public override void OnStateEnter()
    {
        Time.timeScale = 0f;
        machine.VictoryBoard.gameObject.SetActive(true);
        machine.VictoryBoard.UpdateText(0, 0,false);
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1f;
    }

    public override void StateUpdate()
    {
        
    }
}
