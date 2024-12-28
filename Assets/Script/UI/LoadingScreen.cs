using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void UpdateFill(float value)
    {
        slider.value = value*100f;
    }
}
