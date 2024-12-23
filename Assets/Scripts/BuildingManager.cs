using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {



    public static BuildingManager Instance {get; private set;}

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }

    [SerializeField] private Building mainBuilding;

    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        
    }
    private void Start() {
        mainCamera = Camera.main;

        mainBuilding.GetComponent<HealthSystem>().OnDied += MainBase_OnDied;
            
    }

    private void MainBase_OnDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
    }

    private void Update() {
        
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            
            if (activeBuildingType != null) {
               if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage)){

                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray)) {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDone);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                        SetActiveBuildingType(null);

                    } else {
                        TooltipUI.Instance.Show("Insufficient resources!", new TooltipUI.TooltipTimer { timer = 2f});
                    }

                } else {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f});
                }

                
                
                // Debug.Log(activeBuildingType.prefab.GetComponent<BoxCollider2D>());
            } 
            
            
        }

        if (Input.GetKeyDown(KeyCode.Escape) && activeBuildingType != null) {
            SetActiveBuildingType(null);
        }

        
        
    }



    
    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType}
        );
    }

    public BuildingTypeSO GetActiveBuildingType() {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage) {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size,0);

        bool isAreaClear = collider2DArray.Length == 0;

        if (!isAreaClear) {
            errorMessage = "Can't build here!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructDistance);

        foreach (Collider2D collider2D in collider2DArray) {
            
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null) {
                
                if (buildingTypeHolder.buildingType == buildingType) {
                    errorMessage = "There's a same building nearby!";
                    return false;
                }
            }

        }

        if (buildingType.hasResourceGeneratorData) {
            ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbyResource = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResource == 0) {
                errorMessage = "No resources nearby to build here!";
                return false;
            }
        }
        




        float maxConstructDistance = 25;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructDistance);

        foreach (Collider2D collider2D in collider2DArray) {
            
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null) {
                errorMessage = "";
                return true;
            }

        }

        errorMessage = "Must be in close proximity of other building!";
        return false;
    }


    public Building GetMainBuilding() {
        return mainBuilding;
    }
}
