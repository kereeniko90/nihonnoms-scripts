using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


public class ResourceGeneratorFading : MonoBehaviour {

    

    private Transform fadingSpriteTransform;
    private SpriteRenderer fadingSprite;
    private Color fadingSpriteColor;
    private Vector3 fadingSpriteStartPosition;
    private Transform fadingTextTransform;
    private TextMeshPro fadingText;
    private Color fadingTextColor;
    private Vector3 fadingTextStartPosition;

    private float fadeDuration = 2f; 
    private float floatSpeed = 1f; 
    private float fadeTimer; 
    private bool isFading; 

    
    
    
    
    [SerializeField] private ResourceGenerator resourceGenerator;

    private void Awake() {
        
        fadingSpriteTransform = transform.Find("fadingIcon");
        fadingTextTransform = transform.Find("fadingText");
        fadingTextStartPosition = fadingTextTransform.position;
        fadingSpriteStartPosition = fadingSpriteTransform.position;
        
        
    }


    private void Start() {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        fadingText = fadingTextTransform.GetComponent<TextMeshPro>();
        fadingTextColor = fadingText.color;
        fadingTextTransform.GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F2"));

        fadingSprite = fadingSpriteTransform.GetComponent<SpriteRenderer>();
        fadingSpriteColor = fadingSprite.color;

        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;

        
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e)
    {
        ShowResourceFading();
        
    }

   

    

    private void ShowResourceFading() {
        
        fadeTimer = 0f;
        
        isFading = true; 
        fadingText.color = new Color(fadingTextColor.r, fadingTextColor.g, fadingTextColor.b, 1f); 
        fadingTextTransform.position = fadingTextStartPosition;

        fadingSprite.color = new Color (fadingSpriteColor.r, fadingSpriteColor.g, fadingSpriteColor.b, 1f); 
        fadingSpriteTransform.position = fadingSpriteStartPosition;
               
    }

    private void Update() {
        if (isFading) {
            fadeTimer += Time.deltaTime; 
            float fadeProgress = fadeTimer / fadeDuration; 

            
            fadingText.color = new Color(fadingTextColor.r, fadingTextColor.g, fadingTextColor.b, Mathf.Clamp01(1 - fadeProgress));
            fadingSprite.color = new Color (fadingSpriteColor.r, fadingSpriteColor.g, fadingSpriteColor.b, Mathf.Clamp01(1 - fadeProgress)); 


            
            fadingTextTransform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);
            fadingSpriteTransform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);

            
            if (fadeTimer >= fadeDuration) {
                isFading = false; 
                fadingTextTransform.position = fadingTextStartPosition; 
                fadingSpriteTransform.position = fadingSpriteStartPosition;
            }
        }
    }
    

}
