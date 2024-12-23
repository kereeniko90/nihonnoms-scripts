using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTakoTower : MonoBehaviour {

      
    [SerializeField] private float targetMaxRadius = 30f;
    private float shootTimer;
    private float shootTimerMax;
    private Enemy targetEnemy;

    private List<Enemy> targetEnemies = new List<Enemy>();
    private List<Enemy> chosenEnemies = new List<Enemy>();

    
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private Vector3 projectileSpawnPosition;

    private BuildingTypeHolder buildingTypeHolder;

    [SerializeField] private GameObject[] targetGameObject;
    
    [SerializeField] BuildingStatsCanvas buildingStatsCanvas;

    private int damage;

    private void Awake() {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;

        foreach (GameObject targetObject in targetGameObject ) {
            targetObject.SetActive(false);
        }
        
    }

    private void Start() {
        buildingTypeHolder = GetComponentInParent<BuildingTypeHolder>();

        damage = buildingTypeHolder.buildingType.damage;
        shootTimerMax = buildingTypeHolder.buildingType.shootTimerMax;
    }

    private void Update () {
        HandleTargetting();
        HandleShooting();


        for (int i = 0; i < targetEnemies.Count; i++) {
                Enemy enemy = targetEnemies[i];
                if (enemy != null) {
                    

                    if (!targetGameObject[i].activeSelf) {
                    targetGameObject[i].SetActive(true);

                    }

                    targetGameObject[i].transform.position = Vector3.Lerp(targetGameObject[i].transform.position, enemy.transform.position, Time.deltaTime * 7.5f);   
                } else {
                    targetGameObject[i].SetActive(false);
                    targetGameObject[i].transform.position = Vector3.Lerp(targetGameObject[i].transform.position, transform.position, Time.deltaTime * 20f);
                }
            
        
        }

        if (targetEnemies.Count == 0) {
            foreach (GameObject target in targetGameObject) {
                target.SetActive(false);
                target.transform.position = Vector3.Lerp(target.transform.position, transform.position, Time.deltaTime * 20f);
            }
        }
        
    }

    private void HandleTargetting() {
        lookForTargetTimer -= Time.deltaTime;
        damage = buildingStatsCanvas.damage;
        shootTimerMax = buildingStatsCanvas.shootTimerMax;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;

            
            LookForTargets();
        }
    }

    private void HandleShooting() {
        shootTimer -= Time.deltaTime;

        if(shootTimer < 0f) {
            shootTimer += shootTimerMax;
            
            
            foreach (Enemy enemy in targetEnemies) {
                
                if (enemy != null) {
                    TakoyakiBall.Create(projectileSpawnPosition, enemy, transform, damage);
                    SoundManager.Instance.PlaySound(SoundManager.Sound.Blob);

                    
                     
                } else {
                    
                }
            
            }

            

            //targetEnemies.Clear();//EXPERIMENT

        }
        
        
    }


    private void LookForTargets() {

        targetEnemies.Clear();
        
        
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        

        chosenEnemies.Clear();

        foreach (Collider2D collider2D in collider2DArray)
        {
            
                Enemy enemy = collider2D.GetComponent<Enemy>();
                
                if (enemy != null) {
                                       
                        chosenEnemies.Add(enemy);
                }

                  
                               
            
        }

        chosenEnemies.Sort((enemyA, enemyB) => 

            Vector3.Distance(transform.position, enemyA.transform.position)
            .CompareTo(Vector3.Distance(transform.position, enemyB.transform.position))

        );

        for (int i = 0; i < Mathf.Min(3, chosenEnemies.Count); i++) {
            targetEnemies.Add(chosenEnemies[i]);
        }

        
    }


}
