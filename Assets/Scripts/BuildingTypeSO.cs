using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]

public class BuildingTypeSO : ScriptableObject {

    public string nameString;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructDistance;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public float constructionTimerMax;

    public string resourceNodeName;

    public string description;

    public string resourceName;
    
    public int damage;

    public float shootTimerMax;

    public float targetMaxRadius;

    
    


    public string GetConstructionResourceCostString() {
        string str = "";
        

        for (int i = 0; i < constructionResourceCostArray.Length; i++) {
            ResourceAmount resourceAmount = constructionResourceCostArray[i];
            str += "<color=" + resourceAmount.resourceType.colorHex + ">" + resourceAmount.amount + " " + resourceAmount.resourceType.nameShort  + "</color>";

        // Add a space if it's not the last element
            if (i < constructionResourceCostArray.Length - 1) {
            str += " ";
            }   
        }

        
        return str;
    }

    public string GetBuildingDescription() {
        string str = "";

        
        if (hasResourceGeneratorData) {
            
            str = description + ". Produces " + (1/resourceGeneratorData.timerMax).ToString("F0") + " " 
            + resourceGeneratorData.resourceType.nameShort + " per second \nwith " + resourceGeneratorData.maxResourceAmount 
            + " " + resourceGeneratorData.resourceType.nameShort + " " + resourceNodeName + " nearby.";

        } else {
            str = description + ". " + "Shoots every " + shootTimerMax.ToString("F1") + "s";
        }
        

        return str;
    }

    

    public int GetDamageValue() {
        return damage;
    }

    public float GetShootTimerMax() {
        return shootTimerMax;
    }

    // public void IncreaseDamage(int damageAmount) {
    //     damage = damageAmount;
    //     Debug.Log(damage);
    // }

    
    
}
