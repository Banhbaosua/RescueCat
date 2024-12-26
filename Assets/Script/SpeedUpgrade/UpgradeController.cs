using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] UpgradeButton stamina;
    [SerializeField] UpgradeButton speed;
    [SerializeField] UpgradeButton income;
    [Header("Data")]
    [SerializeField] UpgradeData staminaData;
    [SerializeField] UpgradeData speedData;
    [SerializeField] UpgradeData incomeData;
    [SerializeField] CharacterData characterData;

    [SerializeField] TextMeshProUGUI moneyText;

    public void Initiallize()
    {
        ConnectView();

        stamina.UpdateButton(characterData.GetStamina(), staminaData.UpgradeCostByLevel());
        speed.UpdateButton(characterData.GetSpeed(), speedData.UpgradeCostByLevel());
        income.UpdateButton(characterData.GetIncom(), incomeData.UpgradeCostByLevel());
        characterData.OnMoneyChange += (value) =>
        {
            stamina.ChangeButtonState(value - staminaData.UpgradeCostByLevel()>0);
            speed.ChangeButtonState(value - speedData.UpgradeCostByLevel() > 0);
            income.ChangeButtonState(value - incomeData.UpgradeCostByLevel() > 0);
            moneyText.text = value.ToString();
        };
    }

    void UpgradeStamina(float value,float cost)
    {
        characterData.IncreaseStamina(value);
        characterData.DecreaseMoney(cost);
        staminaData.IncreaseLevel();
        stamina.UpdateButton(characterData.GetStamina(), cost);
    }

    void UpgradeSpeed(float value, float cost) 
    { 
        characterData.IncreaseSpeed(value);
        characterData.DecreaseMoney(cost);
        speedData.IncreaseLevel();
        speed.UpdateButton(characterData.GetSpeed() , cost);
    }

    void UpgradeIncome(float value, float cost)
    {
        characterData.IncreaseIncome(value);
        characterData.DecreaseMoney(cost);
        incomeData.IncreaseLevel();
        income.UpdateButton(characterData.GetIncom(), cost);
    }

    void ConnectView()
    {
        stamina.Button.onClick.AddListener(() =>
        UpgradeStamina(
            staminaData.UpgradeValueByLevel(),
            staminaData.UpgradeCostByLevel()));

        speed.Button.onClick.AddListener(() =>
        UpgradeSpeed(
            speedData.UpgradeValueByLevel(),
            speedData.UpgradeCostByLevel()
            ));

        income.Button.onClick.AddListener(() =>
        UpgradeIncome(
            incomeData.UpgradeValueByLevel(),
            incomeData.UpgradeCostByLevel()
            ));
    }
}
