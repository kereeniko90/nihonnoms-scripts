using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakoyakiBall : MonoBehaviour {

    
    public static TakoyakiBall Create(Vector3 position, Enemy enemy, Transform towerPosition, int currentDamage) {
        Transform pfTakoyakiBall = GameAssets.Instance.pfTakoyakiBall;
        Transform takoyakiTransform = Instantiate(pfTakoyakiBall, position, Quaternion.identity);
        takoyakiTransform.SetParent(towerPosition); 
        TakoyakiBall takoyakiBall = takoyakiTransform.GetComponent<TakoyakiBall>();
        takoyakiBall.SetTarget(enemy);
        takoyakiBall.SetDamage(currentDamage);
        return takoyakiBall;

    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    

    private int damage;

    [SerializeField] private EnemyWaveManager enemyWaveManager;
    
    

    

    

    private void Update() {
        Vector3 moveDir; 
        if(targetEnemy != null) {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        } else {
            moveDir = lastMoveDir;
        }
        

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        //transform.eulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVector(moveDir));

        transform.Rotate(0,0, 360f * Time.deltaTime);

        

        timeToDie -= Time.deltaTime;

        if(timeToDie < 0) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy) {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null) {
            int damageAmount = damage;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount, true);
            Destroy(gameObject);
        }
    }

    private void SetDamage(int damage) {
        this.damage = damage;
    }

}
