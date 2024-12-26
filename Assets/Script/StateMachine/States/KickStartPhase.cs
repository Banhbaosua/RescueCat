using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickStartPhase : BaseState
{
    public KickStartPhase(StateMachine machine) : base(machine)
    {
    }

    public override async void OnStateEnter()
    {
        await StartingPhase();
        KickStartTimer().Forget();
    }

    public override void OnStateExit()
    {

    }

    public override void StateUpdate()
    {

    }
    async UniTask StartingPhase()
    {
        machine.mainCamera.enabled = true;
        await UniTask.WaitUntil(() => machine.cameraBrain.IsBlending);
        await UniTask.WaitUntil(() => !machine.cameraBrain.IsBlending);
        Debug.Log("done");
    }


    async UniTaskVoid KickStartTimer()
    {
        await UniTask.WaitForSeconds(machine.kickStartStateTime);
        machine.ChangeState(GameState.Main);
    }
}
