using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Character Data", menuName ="Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public float speed;
    public float stamina;
    public float incom;
    public float money;
    public float kickStartTime;
    public event Action<float> OnMoneyChange = delegate { };
    public event Action<float> OnMaxStaminaChange = delegate { };
    public float GetKickStartTime()
    {
        return kickStartTime;
    }
    public void IncreaseSpeed(float speed)
    { 
        this.speed += speed; 
    }

    public void IncreaseIncome(float income)
    { 
        this.incom += income;
    }

    public void IncreaseStamina(float stamina)
    {
        this.stamina += stamina;
        OnMaxStaminaChange?.Invoke(this.stamina);
    }

    public void IncreaseMoney(float money)
    {
        this.money += money;
        OnMoneyChange?.Invoke(this.money);
    }

    public void DecreaseMoney(float cost)
    { 
        this.money -= cost;
        OnMoneyChange?.Invoke(this.money);
    }

    public void Initialize()
    {
        OnMoneyChange?.Invoke(this.money);
    }

    public float GetSpeed()
    {
        return Mathf.Round(speed * 10.0f) * 0.1f;
    }

    public float GetMaxSpeed()
    {
        return 10 + speed;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetIncom() 
    { 
        return incom;
    }
}
