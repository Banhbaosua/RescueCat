using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MarathonPhase : BaseState
{
    public MarathonPhase(StateMachine machine) : base(machine)
    {
    }

    public override async void OnStateEnter()
    {
        
        machine.PlayerController.StartMarathon();
        machine.PlayerController.transform.rotation = Quaternion.identity;

        await machine.CameraTransition(machine.marathonCamera);
        machine.WaveBehaviour.SetSpeed(40f);
        machine.PlayerController.OnMaxSpeedChange += machine.SpeedoMeter.UpDateMaxSpeed;
        Touch.onFingerDown += Touch_onFingerDown;

        var playerPos = machine.PlayerController.transform.position;
        var newPlayerPos = new Vector2(playerPos.x, playerPos.z);
        var colliderPos = machine.MarathonDestination.transform.position;
        var newColPos = new Vector2(playerPos.x, colliderPos.z);
    }

    private void Touch_onFingerDown(Finger obj)
    {
        machine.PlayerController.InCreaseKickStartSpeed();
        machine.PlayerController.DecreaseStamina();
    }

    public override void OnStateExit()
    {
        Touch.onFingerDown -= Touch_onFingerDown;
        machine.PlayerController.OnStaminaChange -= machine.StaminaBar.SetText;
        machine.PlayerController.OnStaminaChange -= machine.StaminaBar.UpdateFill;
    }

    public override void StateUpdate()
    {

        machine.PlayerController.MoveForward();
    }

}
