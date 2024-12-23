using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsUI : MonoBehaviour {




    private void Start() {
        gameObject.SetActive(false);
    }


    public void ToggleVisible() {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }
    
}
