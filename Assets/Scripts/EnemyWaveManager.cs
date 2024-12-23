using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {
    
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;

    
    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;

    private bool soundHorn = false;

    
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 30f;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;

                if (nextWaveSpawnTimer <= 4 && !soundHorn) {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.ConchShell);
                    soundHorn = true;
                }
                
                if (nextWaveSpawnTimer < 0) {
                    waveNumber++;

                    if (waveNumber % 5 != 0) {
                        SpawnWave();
                    } else {
                        SpawnBoss();
                    }
                    
                }
                break;
            case State.SpawningWave:

                    if (waveNumber % 5 == 0) {
                        
                            BossLevelMessage.Instance.ShowMessage();
                    } 
                    

                
                    if (remainingEnemySpawnAmount > 0) {
                    nextEnemySpawnTimer -= Time.deltaTime;

                


                    if (nextEnemySpawnTimer < 0) {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, 0.3f);
                        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;


                         if (waveNumber % 5 == 0) {
                            // Boss wave logic
                            Enemy.CreateBoss(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0, 10f));
                        } else {
                            // Regular enemy wave logic
                            Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0, 10f));
                        }

                        
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0) {
                            
                            state = State.WaitingToSpawnNextWave;
                            //spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 30f - Mathf.Clamp((float)waveNumber * 0.5f, 0f, 15f);
                            
                            soundHorn = false;
                        }
                    }

                    
                
                    }

                    
                 

                
                break;

        }
        

        
        
    }

    private void SpawnWave() {
        
        
        
        remainingEnemySpawnAmount = 6 + 4 * waveNumber;
        SoundManager.Instance.PlaySound(SoundManager.Sound.Spawn);
        state = State.SpawningWave;
        
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SpawnBoss () {
        remainingEnemySpawnAmount = 1;
        SoundManager.Instance.PlaySound(SoundManager.Sound.BossSpawn);
        state = State.SpawningWave;
        
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer() {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition(){
        return spawnPosition;
    }



}

