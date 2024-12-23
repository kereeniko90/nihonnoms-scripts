using System.Collections;
using TMPro;
using UnityEngine;

public class BossLevelMessage : MonoBehaviour {

    public static BossLevelMessage Instance { get; private set; }

    [SerializeField] private GameObject warningMessage;
    [SerializeField] private float fadeDuration = 1f; 

    private TextMeshProUGUI warningText;
    private Coroutine fadeCoroutine; 
    private void Awake() {
        Instance = this;
    }

    void Start() {   
        warningText = warningMessage.GetComponent<TextMeshProUGUI>();
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 0f); 
        warningMessage.SetActive(false); 
    }

    public void ShowMessage() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine); 
        }

        warningMessage.SetActive(true); 
        fadeCoroutine = StartCoroutine(FadeInAndOut()); 
    }

    private IEnumerator FadeInAndOut() {
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); 
            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, alpha); 
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1f); 

        
        yield return new WaitForSeconds(1f);

        
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); 
            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, alpha); 
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 0f); 

        warningMessage.SetActive(false);
    }

    public void HideMessage() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine); 
        }
        warningMessage.SetActive(false);
    }
}
