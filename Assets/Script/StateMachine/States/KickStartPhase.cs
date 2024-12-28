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
        await UniTask.Delay(1000);
        machine.mainCamera.enabled = false;
        await machine.CameraTransition(machine.mainCamera);
        machine.WaveBehaviour.Move();
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
