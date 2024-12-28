using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI boardText;
    [SerializeField] TextMeshProUGUI catText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] Button okButton;
    public Button Button => okButton;

    public void UpdateText(float catRescued, float reward,bool win = true)
    {
        catText.text = $"Cat Saved: {catRescued}/6";
        rewardText.text = $"Reward: {reward}";
        if (!win)
            boardText.text = "FAILED";
    }
}
