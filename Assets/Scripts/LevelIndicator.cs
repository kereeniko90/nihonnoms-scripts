using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] TextMeshPro levelText;
    [SerializeField] BuildingStatsCanvas buildingStatsCanvas;


    private void Start() {
        buildingStatsCanvas.StatsUpgraded += BuildingStatsCanvas_StatsUpgraded;
        levelText.text = "Lv. " + buildingStatsCanvas.GetCurrentStatsLevel();
    }

    private void BuildingStatsCanvas_StatsUpgraded(object sender, EventArgs e)
    {
        levelText.text = "Lv. " + buildingStatsCanvas.GetCurrentStatsLevel();
    }
}
