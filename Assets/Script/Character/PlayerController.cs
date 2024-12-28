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
    private float kickStartSpeed;
    private float currentStamina;
    private float maxStamina;
    private bool isFatigued;
    private Vector2 direction;
    private CancellationTokenSource moveCancellationTokenSource;

    public event Action<float,float> OnMove = delegate { };
    public event Action<float> OnMaxSpeedChange = delegate { };
    public event Action<float,float> OnStaminaChange = delegate { };
    private bool isRunning;

    public float CurrentSpeed => currentSpeed;
    private void FixedUpdate()
    {
        if(isRunning)
            Move(direction);

        AnimateMovement(currentSpeed / maxSpeed);
    }
    public void Move(Vector2 velocity)
    {
        var direction = new Vector3(velocity.x, 0, velocity.y).normalized;
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed+kickStartSpeed, 10f*Time.deltaTime);
        characterController.SimpleMove(direction*currentSpeed);
        characterController.transform.forward = direction;
        OnMove?.Invoke(currentSpeed, maxSpeed + kickStartSpeed);
    }

    public void MoveForward()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed + kickStartSpeed, 10f * Time.deltaTime);
        characterController.SimpleMove(Vector3.forward * currentSpeed);
        OnMove?.Invoke(currentSpeed, maxSpeed + kickStartSpeed);
    }
    public void Initialize(InputManager inputManager)
    {
        direction = Vector2.zero;
        this.inputManager = inputManager;
        currentSpeed = 0;
        maxSpeed = characterData.GetMaxSpeed();
        maxStamina = characterData.GetStamina();
        currentStamina = maxStamina;

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
                OnMove?.Invoke(currentSpeed, maxSpeed);
                await UniTask.Yield(cancellationToken: token);
            }
        }
        catch (Exception ex) { }
    }

    public void InCreaseKickStartSpeed()
    {
        if (isFatigued) return;
        kickStartSpeed += characterData.GetSpeed() / 10f;
        OnMaxSpeedChange?.Invoke(maxSpeed + kickStartSpeed);
    }

    public void StopKickStart()
    {
        kickStartSpeed = 0f;
        OnMaxSpeedChange?.Invoke(maxSpeed + kickStartSpeed);
    }

    public void DecreaseStamina()
    {
        if (isFatigued) return;
        if(currentStamina - characterData.GetSpeed() > 0)
        {
            currentStamina -= characterData.GetSpeed();
        }
        else
        {
            currentStamina = 0f;
            isFatigued = true ;
            RecoverStamina().Forget();
        }
        OnStaminaChange?.Invoke(currentStamina, maxStamina);
    }

    public void DecreaseMaxSpeed(float value)
    {
        maxSpeed -= value;
        OnMaxSpeedChange?.Invoke(maxSpeed);
    }

    public async UniTaskVoid RecoverStamina()
    {
        while(currentStamina<maxStamina)
        {
            currentStamina = Mathf.Min(currentStamina+(maxStamina/10)*Time.deltaTime,maxStamina);
            OnStaminaChange?.Invoke(currentStamina, maxStamina);
            await UniTask.Yield();
            if(currentStamina == maxStamina)
                isFatigued= false;
        }
    }

    public void StartMarathon()
    {
        isRunning = false;
        moveCancellationTokenSource?.Cancel();
        inputManager.OnTouchMove -= StartMove;
        inputManager.OnTouchUp -= StopMove;
    }

    public void DisableMove()
    {
        isRunning = false;
    }
}
