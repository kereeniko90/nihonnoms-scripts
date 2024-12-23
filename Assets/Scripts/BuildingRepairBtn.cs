using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingRepairBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipMessage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }

    private void Awake() {

        
        

        transform.Find("button").GetComponent<Button>().onClick.AddListener(()=> {

            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;

            ResourceAmount[] resourceAmountCost = new ResourceAmount[] {new ResourceAmount { resourceType = goldResourceType, amount = repairCost }};
            if (ResourceManager.Instance.CanAfford(resourceAmountCost)){

                //can afford repair
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();

            } else {
                TooltipUI.Instance.Show("Insufficent gold for repairs! Need " + repairCost + " " + goldResourceType.nameShort);
            }
            
            
        
        });


        
    }

    private void ToolTipMessage() {
        int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
        int repairCost = missingHealth / 2;
        ResourceAmount[] resourceAmountCost = new ResourceAmount[] {new ResourceAmount { resourceType = goldResourceType, amount = repairCost }};
        TooltipUI.Instance.Show("Need " + repairCost + " " + goldResourceType.nameShort + " to repair" , new TooltipUI.TooltipTimer { timer = 2f});
    }

    
}
