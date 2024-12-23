using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour {
    
    [SerializeField] private ResourceGenerator resourceGenerator;

    private Transform barTransform;


    private void Start() {
        //gameObject.SetActive(false);

        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        barTransform = transform.Find("bar");

        transform.Find("icon").GetComponent<SpriteRenderer> ().sprite = resourceGeneratorData.resourceType.sprite;
        
        transform.Find("text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F2"));

        


    }

    private void Update() {
        barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(),1, 1);
    }

}
