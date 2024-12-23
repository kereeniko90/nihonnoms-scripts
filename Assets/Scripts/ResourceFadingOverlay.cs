using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceFadingOverlay : MonoBehaviour {

    [SerializeField] private GameObject fadingResourceOverlayPrefabs;
    
    [SerializeField] private ResourceGenerator resourceGenerator;

    private Transform fadingOverlayPosition;

    private float spawnInterval = 1f;
    private float spawnTimer = 0f;

    

    

    private void Awake() {
         fadingOverlayPosition = transform.Find("fadingOverlayPosition");   
    }

    
    
    
    private void Update() {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval) {
            CreateFadingOverlay();
            spawnTimer = 0f;
        }
    }

    private void CreateFadingOverlay() {
        
        GameObject fadingOverlay = Instantiate(fadingResourceOverlayPrefabs, fadingOverlayPosition.position, Quaternion.identity);
        fadingOverlay.transform.SetParent(fadingOverlayPosition.transform);
        Transform fadingSpriteTransform = fadingOverlay.transform.Find("fadingIcon");
        Transform fadingTextTransform = fadingOverlay.transform.Find("fadingText");

        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        fadingSpriteTransform.GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        
        fadingTextTransform.GetComponent<TextMeshPro>().SetText("+" + resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));

    }
    
    
}
