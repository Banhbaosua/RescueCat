using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : BaseState
{
    public Win(StateMachine machine) : base(machine)
    {
    }

    public override void OnStateEnter()
    {
        machine.disposables.Clear();
        Time.timeScale = 0;
        machine.VictoryBoard.gameObject.SetActive(true);
        var catRescued = machine.CatCatcher.Cats;
        var reward = machine.GetReward();
        machine.VictoryBoard.UpdateText(catRescued, reward);
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1f;
    }

    public override void StateUpdate()
    {
        
    }
}
