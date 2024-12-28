using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class SpeedSceneBehaviour : MonoBehaviour
{
    [SerializeField] StaminaBar staminaBar;
    [SerializeField] SpeedoMeter meter;
    [SerializeField] CharacterData characterData;
    [SerializeField] CharacterSpeedScene character;
    [SerializeField] PropsLoop propsLoop;
    [SerializeField] Button speedArea;
    [SerializeField] Button playButton;

    [Header("Behaviour Data")]
    [SerializeField] float maxPropsSpeed;
    private float staminaRecoverSpeed => maxStamina*10/3f;
    private float speedDecAmount => maxSpeed/2f;
    private float maxSpeed;
    private float maxStamina;
    private float currentPropsSpeed = 0f    ;
    private float currentSpeed;
    private float currentStamina;

    private bool isFatigued;

    public event Action OnSpeedingTouch = delegate { };
    public event Action<float,float> OnStaminaChange = delegate { };
    public event Action<float,float> OnSpeedChange = delegate { };

    CancellationTokenSource staminaCancellationTokenSource;
    CancellationTokenSource speedCancellationTokenSource;
    public void Initialized()
    {
        characterData.Initialize();
        maxSpeed = characterData.GetSpeed() * 10f;
        maxStamina = characterData.GetStamina();
        currentStamina = maxStamina;

        staminaBar.SetText(currentStamina, maxStamina);
        speedArea.onClick.AddListener(() => OnSpeedingTouch?.Invoke());
        playButton.onClick.AddListener(() => LoadSceneAsyncUtil.Instance.LoadAsync("RescueScene").Forget());
        characterData.OnMaxStaminaChange += UpdateStamina;
        OnSpeedingTouch += IncreaseSpeed;
        OnSpeedingTouch += DecreaseStamina;
        OnSpeedingTouch += RewardMoney;

        OnStaminaChange += staminaBar.SetText;
        OnStaminaChange += staminaBar.UpdateFill;

        OnSpeedChange += meter.UpdateFill;
        OnSpeedChange += character.SetMove;
    }

    void IncreaseSpeed()
    {
        currentSpeed = Mathf.Min(currentSpeed += maxSpeed / 10, maxSpeed);
        currentPropsSpeed = (maxPropsSpeed*currentSpeed)/maxSpeed;

        propsLoop.SetSpeed(currentPropsSpeed);

        OnSpeedChange?.Invoke(currentSpeed, maxSpeed);
        
        speedCancellationTokenSource?.Cancel();
        speedCancellationTokenSource?.Dispose();

        speedCancellationTokenSource = new CancellationTokenSource();

        DecreaseSpeed(0.5f,speedCancellationTokenSource.Token).Forget();
    }
    void DecreaseStamina()
    {
        if (isFatigued) return;
        float speed = characterData.GetSpeed();
        if (currentStamina > speed)
        {
            staminaCancellationTokenSource?.Cancel();
            staminaCancellationTokenSource?.Dispose();
            staminaCancellationTokenSource = new CancellationTokenSource();
            
            currentStamina -= speed;
            OnStaminaChange?.Invoke(currentStamina, maxStamina);
            RecoverStaminaTask(2f,staminaCancellationTokenSource.Token).Forget();
        }
        else
        {
            currentStamina = 0;
            StaminaDepleted();
        }
    }

    void StaminaDepleted()
    {
        isFatigued = true;
        speedArea.interactable = false;
        staminaBar.ChangeFillSpriteDisabled(true);

        staminaCancellationTokenSource?.Cancel();
        staminaCancellationTokenSource?.Dispose();
        staminaCancellationTokenSource = new CancellationTokenSource();
        RecoverStaminaTask(0f,staminaCancellationTokenSource.Token).Forget();
    }

    async UniTaskVoid RecoverStaminaTask(float delay,CancellationToken token)
    {
        try
        {
            await UniTask.WaitForSeconds(delay);
            while (true)
            {
                token.ThrowIfCancellationRequested();
                currentStamina += staminaRecoverSpeed * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
                OnStaminaChange?.Invoke(currentStamina, maxStamina);
                if (currentStamina == maxStamina)
                {
                    if (isFatigued)
                    {
                        isFatigued = false;
                        speedArea.interactable = true;
                        staminaBar.ChangeFillSpriteDisabled(false);
                    }
                    break;
                }
                await UniTask.WaitForSeconds(0.1f, cancellationToken: token);
            }
        }
        catch (Exception ex) { }
    }

    async UniTaskVoid DecreaseSpeed(float delay,CancellationToken token)
    {
        try
        {
            await UniTask.WaitForSeconds(delay);
            while (true)
            {
                token.ThrowIfCancellationRequested();
                currentSpeed -= speedDecAmount * Time.deltaTime;
                currentPropsSpeed = (maxPropsSpeed* currentSpeed) / maxSpeed;
                propsLoop.SetSpeed(currentPropsSpeed);
                OnSpeedChange.Invoke(currentSpeed, maxSpeed);
                if (currentSpeed <= 0.01f)
                    break;
                await UniTask.Yield(cancellationToken:token);
            }
        }
        catch(Exception ex) { }
    }
    void UpdateStamina(float value)
    {
        maxStamina = value;
        staminaBar.SetText(currentStamina, maxStamina);
    }

    void RewardMoney()
    {
        var reward = characterData.GetIncom();
        characterData.IncreaseMoney(reward);
    }

    private void OnDisable()
    {
        OnSpeedingTouch -= IncreaseSpeed;
        OnSpeedingTouch -= DecreaseStamina;
        characterData.OnMaxStaminaChange -= UpdateStamina;
        OnStaminaChange -= staminaBar.SetText;
        OnStaminaChange -= staminaBar.UpdateFill;
    }
}
