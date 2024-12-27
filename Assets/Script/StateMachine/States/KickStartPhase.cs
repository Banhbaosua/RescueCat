using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class KickStartPhase : BaseState
{
    public KickStartPhase(StateMachine machine) : base(machine)
    {
    }

    public override async void OnStateEnter()
    {
        await StartingPhase();
        AsignTouchAction();
        KickStartCountDown().Forget();
        machine.PlayerController.DisableMove();
    }

    public override void OnStateExit()
    {
        DisposeTouchAction();
        machine.PlayerController.RecoverStamina().Forget();
    }

    public override void StateUpdate()
    {
        machine.PlayerController.DisableMove();
    }
    async UniTask StartingPhase()
    {
        machine.mainCamera.enabled = true;
        await UniTask.WaitUntil(() => machine.cameraBrain.IsBlending);
        await UniTask.WaitUntil(() => !machine.cameraBrain.IsBlending);
        Debug.Log("done");
    }


    async UniTaskVoid KickStartCountDown()
    {
        await UniTask.WaitForSeconds(machine.KickStartTime);
        machine.ChangeState(GameState.Main);
    }

    void AsignTouchAction()
    {
        Touch.onFingerDown += Touch_onFingerDown;
        machine.PlayerController.OnMaxSpeedChange += machine.SpeedoMeter.UpDateMaxSpeed;
        machine.PlayerController.OnStaminaChange += machine.StaminaBar.SetText;
        machine.PlayerController.OnStaminaChange += machine.StaminaBar.UpdateFill;
    }

    void DisposeTouchAction()
    {
        Touch.onFingerDown -= Touch_onFingerDown;
        machine.PlayerController.OnMaxSpeedChange -= machine.SpeedoMeter.UpDateMaxSpeed;
    }

    private void Touch_onFingerDown(Finger obj)
    {
        machine.PlayerController.InCreaseKickStartSpeed();
        machine.PlayerController.DecreaseStamina();
    }
}
