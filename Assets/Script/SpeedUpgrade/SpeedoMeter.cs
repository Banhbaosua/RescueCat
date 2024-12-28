using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpeedoMeter : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI maxSpeed;
    [SerializeField] RectTransform maxSpeedTextContainer;
    private Tween tweenTask;
    // Start is called before the first frame update
    public void UpdateFill(float value, float maxValue)
    {
        slider.value = value/maxValue;
        maxSpeed.text = maxValue.ToString();
        fill.color = Color.Lerp(Color.green, Color.red, value/maxValue);
    }
    public async void UpDateMaxSpeed(float value)
    {
        maxSpeed.text = value.ToString();
        await PunchScaleTask();
    }

    async UniTask PunchScaleTask()
    {
        if (tweenTask != null) return;
        tweenTask = maxSpeedTextContainer.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetEase(Ease.InOutElastic);
        await tweenTask.ToUniTask();
        tweenTask = null;
    }
}
