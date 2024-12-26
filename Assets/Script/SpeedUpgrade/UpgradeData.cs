using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Upgrade Data", menuName ="Upgrade/Data")]
public class UpgradeData : ScriptableObject
{
    public float baseCost;
    public float baseValue;
    public float upgradeValue;
    public float growthRateValue;
    public float growthRateCost;
    public float upgradeLevel;

    public float UpgradeCostByLevel()
    {
        return baseCost + upgradeLevel*(upgradeLevel+growthRateCost);
    }

    public float UpgradeValueByLevel()
    {
        return upgradeValue + growthRateValue*upgradeLevel;
    }

    public void IncreaseLevel()
    {
        upgradeLevel++;
    }
}
