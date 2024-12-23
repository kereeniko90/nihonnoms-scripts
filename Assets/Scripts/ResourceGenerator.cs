using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
    
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
        
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);
        

        int nearbyResource = 0;
        foreach (Collider2D collider2D in collider2DArray) {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null) {
                //it is a resource node
                if (resourceNode.resourceType == resourceGeneratorData.resourceType) {
                    //Debug.Log("Resource type: " + resourceNode.resourceType);
                    //same type
                    nearbyResource++;
                }
                
            } 
        }

        nearbyResource = Mathf.Clamp(nearbyResource, 0, resourceGeneratorData.maxResourceAmount); 


        return nearbyResource;
    }
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;


    private void Awake() {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start() {

        int nearbyResource = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        if (nearbyResource == 0) {
            //no resource nearby
            enabled = false; 
        }

        //Debug.Log("nearby resource: " + nearbyResource + "timerMax" + timerMax);
    }    

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            timer += timerMax;
            //Debug.Log("Ding! " + buildingType.resourceGeneratorData.resourceType.nameString);
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, GetAmountGeneratedPerSecond());

            
        }
    }


    public ResourceGeneratorData GetResourceGeneratorData() {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized() {
        return timer / timerMax;
    }

    public float GetAmountGeneratedPerSecond() {

        float generatedAmountPerSecond = 1/timerMax * GetNearbyResourceAmount(resourceGeneratorData, transform.position) /resourceGeneratorData.maxResourceAmount;
        return generatedAmountPerSecond;
    }
}
