using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Sprite disableSprite;
    [SerializeField] Sprite enableSprite;
    [SerializeField] TextMeshProUGUI value;
    [SerializeField] TextMeshProUGUI cost;
    public Button Button => button;
    public void UpdateButton(float value,float cost)
    {
        this.value.text = value.ToString();
        this.cost.text = cost.ToString();
    }

    public void ChangeButtonState(bool value)
    {
        if (value)
        {
            button.image.sprite = enableSprite;
            button.interactable = true;
        }
        else
        {
            button.image.sprite = disableSprite;
            button.interactable = false;
        }
    }
}
