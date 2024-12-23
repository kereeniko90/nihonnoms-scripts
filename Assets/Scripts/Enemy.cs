using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    
    public static Enemy Create(Vector3 position) {
        
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);
        
        

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.enemyType = "normal";
        return enemy;

    }

    public static Enemy CreateBoss (Vector3 position) {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfBossEnemy, position, Quaternion.identity);
        
        

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.enemyType = "boss";
        return enemy;
    }


    private Transform targetTransform;
    private Rigidbody2D enemyRigidbody2D;

    private string enemyType;

    [SerializeField] private EnemyWaveManager enemyWaveManager;

    private Transform dieParticle;
    

    private HealthSystem healthSystem;

    private DamagedFadingOverlay damagedFadingOverlay;

    
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private float moveSpeed = 6f;

    private float bossMoveSpeed = 4f;

    private SpriteRenderer visualSprite;

    private void Awake() {
        
        
        healthSystem = GetComponent<HealthSystem>();
        damagedFadingOverlay = GetComponent<DamagedFadingOverlay>();

        if (enemyWaveManager == null) {
        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
        }
        
    }

    private void Start() {

        
        if (enemyType == "normal") {
            dieParticle = GameAssets.Instance.pfEnemyDieParticles;
        } else if (enemyType == "boss") {
            dieParticle = GameAssets.Instance.pfBossDieParticles;
            ChromaticAberrationEffect.Instance.SetWeight(0.5f);
        }
        enemyRigidbody2D = GetComponent<Rigidbody2D>();

        int currentMaxHealth = healthSystem.GetHealthAmountMax();
        int waveNumber = enemyWaveManager.GetWaveNumber();
        

        if (enemyType == "normal") {
            visualSprite = transform.Find("pfWasabiVisual").Find("Body").GetComponent<SpriteRenderer>();
            healthSystem.SetHealthAmountMax(30 + (waveNumber * 1 - 1), true);
            moveSpeed += (float)waveNumber * 0.05f;
        } else if (enemyType == "boss") {
            healthSystem.SetHealthAmountMax(100 + (waveNumber * 10), true);
            bossMoveSpeed += (float)waveNumber * 0.05f;
            visualSprite = transform.Find("pfWasabiBoss").Find("Body").GetComponent<SpriteRenderer>();
            Color wasabiColor = GenerateRandomHexColor();
            visualSprite.color = wasabiColor;
        }

        

        if (BuildingManager.Instance.GetMainBuilding() != null) {
            targetTransform = BuildingManager.Instance.GetMainBuilding().transform;
        }
        

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        

        lookForTargetTimer = UnityEngine.Random.Range(0, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        //CinemachineShake.Instance.ShakeCamera(3f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(0.1f);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e) {
        Destroy(gameObject);
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);

        if (enemyType == "boss") {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BossDie);
        }
        //CinemachineShake.Instance.ShakeCamera(7f, 0.2f);
        ChromaticAberrationEffect.Instance.SetWeight(0.2f);
    }

    private void Update() {

        HandleMovement();
        HandleTargetting();

        
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Building building = collision.gameObject.GetComponent<Building>();
        ResourceNode resourceNode = collision.gameObject.GetComponent<ResourceNode>();

        if (building != null) {
            //collided with building
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();

            if (enemyType == "normal") {
                healthSystem.Damage(10, true);
                this.healthSystem.Damage(999, false);
            } else if (enemyType == "boss" && building.buildingType.nameString == "Onigiri HQ") {
                healthSystem.Damage(99999, true);
                this.healthSystem.Damage(99999, false);
            }
            
            
        } else if (resourceNode != null) {
            
            AvoidResourceNode(resourceNode);
        }
    }

    private void HandleMovement() {
        if (targetTransform != null) {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            if (enemyType == "normal") {
                enemyRigidbody2D.velocity = moveDir * moveSpeed;
            } else if (enemyType == "boss") {
                enemyRigidbody2D.velocity = moveDir * bossMoveSpeed;
            }
            
            
        } else {
            
        }
        
    }

    private void HandleTargetting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets() {
        float targetMaxRadius = 10f;

        if (enemyType == "normal") {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

            foreach (Collider2D collider2D in collider2DArray) {
            
            Building building = collider2D.GetComponent<Building>();
            if (building != null) {
                if (targetTransform == null) {
                    targetTransform = building.transform;
                } else {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position)) {
                            //closer
                            targetTransform = building.transform;
                        }
                    }
                } 
            
            }
                
        } else if (enemyType == "boss") {
               if (targetTransform == null) {
            //if no target
            if (BuildingManager.Instance.GetMainBuilding() != null) {
                targetTransform = BuildingManager.Instance.GetMainBuilding().transform;
            }
            
        } 
        }

        
        

        
    }

    private void AvoidResourceNode(ResourceNode resourceNode) {
        
        // Calculate the direction away from the resource node
        Vector3 avoidanceDirection = (transform.position - resourceNode.transform.position).normalized;

        // Randomly decide whether to adjust movement along the X or Y axis
        bool moveAlongXAxis = UnityEngine.Random.value > 0.5f;  // 50% chance to pick X or Y

        Vector3 newMoveDirection;

        if (moveAlongXAxis) {
            // Move along the X-axis by adding or subtracting some distance
            newMoveDirection = new Vector3(avoidanceDirection.x + UnityEngine.Random.Range(1f, 2f), avoidanceDirection.y, 0f).normalized;
        } else {
            // Move along the Y-axis by adding or subtracting some distance
            newMoveDirection = new Vector3(avoidanceDirection.x, avoidanceDirection.y + UnityEngine.Random.Range(1f, 2f), 0f).normalized;
        }

        // Directly set the enemy's velocity instead of using AddForce
        enemyRigidbody2D.velocity = newMoveDirection * moveSpeed;

        // Check if the enemy is stuck and reset the velocity if necessary
        if (enemyRigidbody2D.velocity.magnitude < 0.1f) {
            enemyRigidbody2D.velocity = newMoveDirection * moveSpeed;  // Reset with minimum movement velocity
        }

        HandleMovement();
        HandleTargetting();
    }

    private Color GenerateRandomHexColor() {

        int randomInt = UnityEngine.Random.Range(0, (int)Mathf.Pow(16, 6));  // Generate a random number up to the hex limit
        string hexString = randomInt.ToString("X" + 6); // Convert to hex and pad with zeros

        byte r = byte.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r,g,b,255);


    }
    
}