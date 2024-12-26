using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedoMeter : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    // Start is called before the first frame update
    public void UpdateFill(float value, float maxValue)
    {
        slider.value = value/maxValue;
        fill.color = Color.Lerp(Color.green, Color.red, value/maxValue);
    }
}
