using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] Sprite zeroStaminaFillSprite;
    [SerializeField] Sprite staminaFillSprite;

    public void SetText(float value, float maxValue)
    {
        staminaText.text = value.ToString("F0") + "/" + maxValue.ToString("F0");
    }

    public void UpdateFill(float value, float maxValue) 
    { 
        slider.value = value/maxValue;
    }
    public void ChangeFillSpriteDisabled(bool disabled)
    {
        if (disabled)
            fill.sprite = zeroStaminaFillSprite;
        else
            fill.sprite = staminaFillSprite;
    }
}
