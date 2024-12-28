using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class MainPhase : BaseState
{
    public MainPhase(StateMachine machine) : base(machine)
    {
    }

    public override void OnStateEnter()
    {
        machine.PlayerController.OnMove += machine.SpeedoMeter.UpdateFill;
        KickStartTimer().Forget();
    }

    public override void OnStateExit()
    {
        machine.PlayerController.OnMove -= machine.SpeedoMeter.UpdateFill;
    }

    public override void StateUpdate()
    {
    }
    
    async UniTaskVoid KickStartTimer()
    {
        await UniTask.WaitForSeconds(machine.characterData.GetKickStartTime());
        machine.PlayerController.StopKickStart();
    }
}
