using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    public static GameOverUI Instance { get; private set;}
    private void Awake() {

        Instance = this;
        transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(()=>{
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
            Time.timeScale = 1f;
            
        });

        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(()=>{
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
            Time.timeScale = 1f;            
        });

        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);

        transform.Find("waveSurvivedText").GetComponent<TextMeshProUGUI>().SetText("You Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves!");
        Time.timeScale = 0;
    }

    private void Hide() {
        gameObject.SetActive(false);
    }




}
