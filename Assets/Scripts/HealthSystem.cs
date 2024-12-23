using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
   
   public event EventHandler OnHealthAmountMaxChanged;
   public event EventHandler OnDamaged;
   public event EventHandler OnHealed;
   public event EventHandler OnDied;
   

   private Transform damageOverlay;
   private int healthAmount;
   [SerializeField] private int healthAmountMax;

   [SerializeField] private GameObject damagedOverlayPrefab;

   private void Awake() {
        healthAmount = healthAmountMax;
        damageOverlay = transform.Find("damageOverlay"); 
   }

   public void Damage(int damageAmount, bool notEnemy) {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        if (notEnemy) {
            ShowDamageText(damageAmount);
        }
        

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead()) {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
   }

   public void Heal(int healAmount) {
     healthAmount -= healAmount;
     healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

     OnHealed?.Invoke(this, EventArgs.Empty);

   }

   public void HealFull() {
     healthAmount = healthAmountMax;
     OnHealed?.Invoke(this, EventArgs.Empty);
   }

   public bool IsDead() {
        return healthAmount == 0;
   }

   public bool isFullHealth() {
        return healthAmount == healthAmountMax;
   }

   public int GetHealthAmount() {
    return healthAmount;
   }

   public int GetHealthAmountMax() {
    return healthAmountMax;
   }

   public float GetHealthAmountNormalized() {
     return (float)healthAmount / healthAmountMax;
   }

   public void SetHealthAmountMax(int healthAmountMax, bool updatehealthAmount) {
     this.healthAmountMax = healthAmountMax;

     if (updatehealthAmount) {

        healthAmount = healthAmountMax;
     }

     OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
   }

   private void ShowDamageText(int damage) {
      GameObject damageTextTransform = Instantiate(damagedOverlayPrefab, damageOverlay.transform.position, Quaternion.identity);
      
      TextMeshPro damageText = damageTextTransform.GetComponentInChildren<TextMeshPro>();

      damageText.text = "-" + damage.ToString();
      Destroy(damageTextTransform, 1.0f); 
      
      
   }
}
