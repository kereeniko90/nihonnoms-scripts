using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyVisual;
    [SerializeField] private List<Transform> spawnPosition;

    [SerializeField] private float spawnInterval = 3.0f;
    [SerializeField] private float enemySpeed = 10.0f;

    

    private int enemyNumber;
    private float nextEnemySpawnTimer;

    private float waitTimer = 1f;


    private void Start()
    {

        nextEnemySpawnTimer = spawnInterval;
        enemyNumber = Random.Range(4, 5);

    }

    private void Update()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer < 0) {
            SpawnEnemies();
        }


    }


    private void SpawnEnemies() {
        if (enemyNumber > 0)
        {
            nextEnemySpawnTimer -= Time.deltaTime;
            if (nextEnemySpawnTimer <= 0f)
            {



                nextEnemySpawnTimer = Random.Range(0.2f, 0.5f);
                int spawnIndex = Random.Range(0, spawnPosition.Count);
                Transform spawnPoint = spawnPosition[spawnIndex];


                GameObject newEnemy = Instantiate(enemyVisual, spawnPoint.position, Quaternion.identity);


                newEnemy.transform.position += UtilsClass.GetRandomDir();


                MainMenuEnemyVisualMovement enemyMovement = newEnemy.GetComponent<MainMenuEnemyVisualMovement>();



                if (spawnIndex == 0)
                {
                    enemyMovement.SetMovementDirection(Vector3.right);  // Move to the right
                }
                else if (spawnIndex == 1)
                {
                    enemyMovement.SetMovementDirection(Vector3.left);   // Move to the left
                }


                enemyMovement.SetSpeed(enemySpeed);
                enemyNumber--;

            }

        } else {
            enemyNumber = Random.Range(1, 5);
            waitTimer = 3f;
        }
    }



}
