using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBaseOffense : MonoBehaviour {

    

        
    [SerializeField] private float targetMaxRadius = 30f;

    
    private float shootTimer;
    private float shootTimerMax;
    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private Vector3 projectileSpawnPosition;

    private BuildingTypeHolder buildingTypeHolder;

    [SerializeField] BuildingStatsCanvas buildingStatsCanvas;

    [SerializeField] private GameObject targetGameObject;

    public int damage;

    private void Awake() {
        
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
        targetGameObject.SetActive(false);
    }

    private void Start() {
        buildingTypeHolder = GetComponentInParent<BuildingTypeHolder>();
        

        if (buildingStatsCanvas == null) {
            Debug.Log("not found");
        }

        damage = buildingTypeHolder.buildingType.damage;
        shootTimerMax = buildingTypeHolder.buildingType.shootTimerMax;
        targetMaxRadius = buildingTypeHolder.buildingType.targetMaxRadius;
        
    }

    private void Update () {
        HandleTargetting();
        HandleShooting();
        if (targetEnemy != null) {
            targetGameObject.SetActive(true);
            targetGameObject.transform.position = Vector3.Lerp(targetGameObject.transform.position, targetEnemy.transform.position, Time.deltaTime * 7.5f);
        } else {
            targetGameObject.SetActive(false);
            targetGameObject.transform.position = Vector3.Lerp(targetGameObject.transform.position, transform.position, Time.deltaTime * 20f);
        }
    }

    private void HandleTargetting() {
        lookForTargetTimer -= Time.deltaTime;
        damage = buildingStatsCanvas.damage;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleShooting() {
        shootTimer -= Time.deltaTime;

        if(shootTimer < 0f) {
            shootTimer += shootTimerMax;
            if (targetEnemy != null) {
            
            
            //damage = buildingTypeHolder.buildingType.damage;
            ArrowProjectile.Create(projectileSpawnPosition, targetEnemy, transform, damage);
            } 
        }
        
        
    }


    private void LookForTargets() {

        targetMaxRadius = buildingTypeHolder.buildingType.targetMaxRadius;
        
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            
                Enemy enemy = collider2D.GetComponent<Enemy>();
                if (enemy != null) {
                    
                    if (targetEnemy == null) {
                        targetEnemy = enemy;
                        
                        
                    } else {
                        if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position)) {
                            //closer
                            targetEnemy = enemy;
                            
                            
                        }
                    }
                }
            
        }

        
    }

    


}
