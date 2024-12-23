using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}
    private Dictionary<ResourceTypeSO, float> resourceAmountDictionary;

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;

    private void Awake() {

        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, float>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
            resourceAmountDictionary[resourceType] = 0f;
        }

        foreach (ResourceAmount resourceAmount in startingResourceAmountList) {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }

        
    }

    
    

    private void TestLog() {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys) {
            Debug.Log(resourceType.nameString + ":" + resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource (ResourceTypeSO resourceType, float amount) {
        resourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        
    }

    public float GetResourceAmount(ResourceTypeSO resourceType) {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            if(GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount) {
                //can afford
            } else {
                //cant afford
                return false;
            }
        }

        //can afford all
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
            if(GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount) {
                
            } 
    }
    }

}
