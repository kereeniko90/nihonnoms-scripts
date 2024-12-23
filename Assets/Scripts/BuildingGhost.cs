using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {

    private GameObject spriteGameObject;
    private Transform detectionRadius;
    private ResourceNearbyOverlay resourceNearbyOverlay;
    private void Awake() {
        spriteGameObject = transform.Find("sprite").gameObject;
        detectionRadius = transform.Find("areaDetection");
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if (e.activeBuildingType == null) {
            Hide();
            resourceNearbyOverlay.Hide();
        } else {
            Show(e.activeBuildingType.sprite, 
                e.activeBuildingType.resourceGeneratorData.resourceDetectionRadius, 
                e.activeBuildingType.hasResourceGeneratorData, 
                e.activeBuildingType.targetMaxRadius != 0f ? e.activeBuildingType.targetMaxRadius: 0f);
            if (e.activeBuildingType.hasResourceGeneratorData) {
                resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            } else {
                resourceNearbyOverlay.Hide();
            }
            
        }
    }

    private void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite, float radius, bool generateResource, float targetRadius) {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;

        float finalRadius;

        if (generateResource) {
            finalRadius = radius * 2;        
            
        } else {
            finalRadius = targetRadius * 2;
        }

        detectionRadius.gameObject.SetActive(true);
        detectionRadius.localScale = new Vector3(finalRadius + 1.1f, finalRadius + 1.1f, detectionRadius.localScale.z);
        
    }

    private void Hide() {
        spriteGameObject.SetActive(false);
        detectionRadius.gameObject.SetActive(false);
    }
    
}
