using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceNearbyOverlay : MonoBehaviour {


    private ResourceGeneratorData resourceGeneratorData;

    private void Awake() {
        Hide();
    }

    private void Update() { 
        int nearbyResource = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position - transform.localPosition);

        float percent = Mathf.RoundToInt((float)nearbyResource / resourceGeneratorData.maxResourceAmount * 100f);
        transform.Find("text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }
    public void Show(ResourceGeneratorData resourceGeneratorData){
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;

        
    }
    
    public void Hide(){
        gameObject.SetActive(false);
    }
}