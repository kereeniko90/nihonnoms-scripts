using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;
    

    private TextMeshPro hpText;

    private void Awake() {
        barTransform = transform.Find("bar");
        hpText = transform.Find("hpText").Find("text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {   
        UpdateHealthText();

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;
        UpdateBar();
        UpdateHealthBarVisible();
        UpdateHealthText();
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, EventArgs e)
    {
        UpdateHealthText();
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
        UpdateHealthText();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
        UpdateHealthText();
    }

    private void UpdateHealthText() {
        
        hpText.SetText(healthSystem.GetHealthAmount() + "/" + healthSystem.GetHealthAmountMax());


        
    }

    private void UpdateBar() {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1,1);
    }


    private void UpdateHealthBarVisible() {
        if (healthSystem.isFullHealth()) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }


}
