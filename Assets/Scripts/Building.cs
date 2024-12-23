using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    
    public BuildingTypeSO buildingType;
    private HealthSystem healthSystem;

    private Transform buildingDemolishButton;
    private Transform buildingRepairButton;

    private Transform dieParticle;

    private void Awake() {
        buildingDemolishButton = transform.Find("pfBuildingDemolishBtn");
        buildingRepairButton = transform.Find("pfBuildingRepairBtn");
        HideDemolishBtn();
        HideRepairBtn();
        dieParticle = GameAssets.Instance.pfBuildingDestroyedParticles;
        
    }
    private void Start() {

        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        healthSystem.OnDied += HealthSystem_OnDied;

    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        if (healthSystem.isFullHealth()) {
            HideRepairBtn();
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {   
        
        ShowRepairBtn();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        //CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(0.5f);
        
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        //CinemachineShake.Instance.ShakeCamera(10f, 2f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void OnMouseEnter() {
        
        ShowDemolishBtn();
    }

    private void OnMouseExit() {
        HideDemolishBtn();
    }

    private void ShowDemolishBtn() {
        if (buildingDemolishButton != null) {
            buildingDemolishButton.gameObject.SetActive(true);
        }
    }

    private void HideDemolishBtn() {
        if (buildingDemolishButton != null) {
            buildingDemolishButton.gameObject.SetActive(false);
        }
    }

    private void ShowRepairBtn() {
        if (buildingRepairButton != null) {
            buildingRepairButton.gameObject.SetActive(true);
        }
    }

    private void HideRepairBtn() {
        if (buildingRepairButton != null) {
            buildingRepairButton.gameObject.SetActive(false);
        }
    }
}
