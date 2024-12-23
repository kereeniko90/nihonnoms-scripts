using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyWaveUI : MonoBehaviour {


    [SerializeField] private EnemyWaveManager enemyWaveManager;
    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private TextMeshProUGUI bestScoreText;

    private GameObject waveMessageBg;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;

    private int currentBestScore;

    private Camera mainCamera;
    private void Awake() {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        bestScoreText = transform.Find("bestScoreText").GetComponent<TextMeshProUGUI>();
        waveMessageBg = GameObject.Find("waveMessageBackground");
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
        currentBestScore = PlayerPrefs.GetInt("bestScore", 0);
        
        
    }

    private void Start() {
        mainCamera = Camera.main;
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        if (currentBestScore > 1) {
            SetBestScoreText("Best Score : " + currentBestScore + " Waves");
        } else {
            SetBestScoreText("Best Score : " + currentBestScore + " Wave");
        }

    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        if (enemyWaveManager.GetWaveNumber() > currentBestScore) {
            PlayerPrefs.SetInt("bestScore", currentBestScore);
            currentBestScore = enemyWaveManager.GetWaveNumber();
            if (currentBestScore > 1) {
            SetBestScoreText("Best Score : " + currentBestScore + " Waves");
            } else {
            SetBestScoreText("Best Score : " + currentBestScore + " Wave");
            }
        }
    }

    private void Update() {
        
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
        
    }

    private void HandleNextWaveMessage() {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer < 0) {
            SetMessageText("");
            HideMessageBackground();
        } else {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F0") + "s");
            ShowMessageBackground();
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator() {
        Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 350f;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
        //enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);

    }

    private void HandleEnemyClosestPositionIndicator() {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);


        Enemy targetEnemy = null;
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




        if (targetEnemy != null) {
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 300f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

            float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
            enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        } else {
            //no enemies alive
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void SetMessageText(string message) {
        waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string message) {
        waveNumberText.SetText(message);
    }

    private void HideMessageBackground() {
        waveMessageBg.SetActive(false);

    }

    private void ShowMessageBackground() {
        waveMessageBg.SetActive(true);

    }

    private void SetBestScoreText(string text) {
        bestScoreText.SetText(text);
    }
    
}
