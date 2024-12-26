using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] CharacterData characterData;
    [SerializeField] Animator animator;
    private InputManager inputManager;
    private float currentSpeed;
    private float maxSpeed;
    private float currentStamina;
    private float maxStamina;
    private float kickStartTime;
    private Vector2 direction;
    private CancellationTokenSource moveCancellationTokenSource;

    private bool isRunning;
    private void FixedUpdate()
    {
        if(isRunning)
            Move(direction);

        AnimateMovement(currentSpeed / maxSpeed);
    }
    public void Move(Vector2 velocity)
    {
        var direction = new Vector3(velocity.x, 0, velocity.y).normalized;
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, 10f*Time.deltaTime);
        characterController.SimpleMove(direction*currentSpeed);

        characterController.transform.forward = direction;
    }
    public void Initialize(InputManager inputManager)
    {
        direction = Vector2.zero;
        this.inputManager = inputManager;
        currentSpeed = 0;
        currentStamina = 0;
        maxSpeed = characterData.GetMaxSpeed();
        maxStamina = characterData.GetStamina();
        kickStartTime = characterData.GetKickStartTime();

        inputManager.OnTouchMove += StartMove;
        inputManager.OnTouchUp += StopMove;
    }
    public void AnimateMovement( float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void StartMove(Vector2 direction)
    {
        isRunning = true;
        moveCancellationTokenSource?.Cancel();
        this.direction = direction;
    }

    public void StopMove()
    {
        isRunning = false;
        moveCancellationTokenSource = new CancellationTokenSource();
        StopMoveTask(moveCancellationTokenSource.Token).Forget();
    }

    async UniTaskVoid StopMoveTask(CancellationToken token)
    {
        try
        {
            while (currentSpeed > 0f)
            {
                token.ThrowIfCancellationRequested();
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, 10f*Time.deltaTime);
                if (currentSpeed < 0.01f)
                {
                    currentSpeed = 0f;
                    break;
                }
                await UniTask.Yield(cancellationToken: token);
            }
        }
        catch (Exception ex) { }
    }
}
