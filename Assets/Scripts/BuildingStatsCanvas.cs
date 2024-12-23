using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingStatsCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private ResourceTypeSO[] nodeResourceType;
    [SerializeField] private ResourceTypeSO[] spdUpgradeResourceType;
    [SerializeField] private GameObject upAtkGameObject;
    [SerializeField] private GameObject upSpdGameObject;

    public event EventHandler StatsUpgraded;

    private TextMeshProUGUI description;
    private Image backgroundImage;
    private bool mouseEntered = false;
    private bool fadeOutStarted = false;
    private BuildingTypeHolder buildingTypeHolder;
    private ResourceGenerator resourceGenerator;
    private BoxCollider2D[] boxColliders;
    private BuildingTypeSO buildingTypeSO;
    private bool isTower;
    public int damage;
    public float shootTimerMax;
    private RectTransform upAtkTransform;
    private RectTransform upSpdTransform;
    private Image upAtkSprite;
    private Image upSpdSprite;
    private TextMeshProUGUI upAtkText;
    private TextMeshProUGUI upAtkLvlText;
    private TextMeshProUGUI upSpdText;
    private TextMeshProUGUI upSpdLvlText;

    private Button upAtkButton;
    private Button upSpdButton;

    private int attackLevel = 1;
    private int speedLevel = 1;

    private float[] atkResourceCosts;
    private float[] spdResourceCosts;

    private readonly float[] startingSpeedCosts = {100f,75f,50f};

    private readonly float[] startingAttackCosts = {75f,50f,25f};


    private void Awake() {
        boxColliders = GetComponents<BoxCollider2D>();

        atkResourceCosts = (float[])startingAttackCosts.Clone();
        
        spdResourceCosts = (float[])startingSpeedCosts.Clone();

        upAtkButton = transform.Find("increaseAtk").GetComponent<Button>();
        upSpdButton = transform.Find("increaseSpd").GetComponent<Button>();

        upAtkButton.onClick.AddListener(()=> {

            IncreaseAtk();
            //TooltipUI.Instance.Hide();            

        });

        upSpdButton.onClick.AddListener(()=> {
            
            if (shootTimerMax > 0.05) {
                IncreaseSpd();
                //TooltipUI.Instance.Hide();
            } else {
                TooltipUI.Instance.Show("Speed already at max level!");
            }
            

        });

        

    }

    private void Start() {
        boxColliders[1].enabled = false;

        buildingTypeHolder = transform.parent.GetComponent<BuildingTypeHolder>();
        resourceGenerator = transform.parent.GetComponent<ResourceGenerator>();

        upAtkTransform = transform.Find("increaseAtk").GetComponent<RectTransform>();
        upSpdTransform = transform.Find("increaseSpd").GetComponent<RectTransform>();

        description = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundImage = transform.Find("background").GetComponent<Image>();

        upAtkSprite = upAtkTransform.GetComponent<Image>();
        upSpdSprite = upSpdTransform.GetComponent<Image>();

        upAtkText = upAtkTransform.Find("text").GetComponent<TextMeshProUGUI>();
        upSpdText = upSpdTransform.Find("text").GetComponent<TextMeshProUGUI>();
        upAtkLvlText = upAtkTransform.Find("atkLvl").GetComponent<TextMeshProUGUI>();
        upSpdLvlText = upSpdTransform.Find("spdLvl").GetComponent<TextMeshProUGUI>();

        

        

        

        buildingTypeSO = buildingTypeHolder.buildingType;
        damage = buildingTypeSO.GetDamageValue();
        shootTimerMax = buildingTypeSO.GetShootTimerMax();

        
        if (buildingTypeSO.hasResourceGeneratorData) {
            if (buildingTypeHolder.buildingType.nameString != "Onigiri HQ") {
                upAtkTransform.gameObject.SetActive(false);
                upSpdTransform.gameObject.SetActive(false);
            } else {
                upSpdTransform.gameObject.SetActive(false);
            }
        }

        
        SetAlphaForAllUI(0f);

       
        isTower = !buildingTypeHolder.buildingType.hasResourceGeneratorData;
    }

    private void OnMouseEnter() {
        
        if (!isTower && buildingTypeHolder.buildingType.nameString == "Onigiri HQ") {
            updateMainBaseDamageString();
        } else if (isTower) {
            updateTowerString();
        } else {
            description.text = $"{buildingTypeSO.nameString}\nResources: {resourceGenerator.GetAmountGeneratedPerSecond():F1} {buildingTypeHolder.buildingType.resourceName}/s";
        }

        if (!mouseEntered) {
            StopAllCoroutines();
            StartCoroutine(FadeUIElements(1f, 0.25f)); 
        }

        

        

    }

    private void OnMouseExit() {
        if (mouseEntered && !fadeOutStarted) {
            StartCoroutine(DelayedFadeOut(0.25f));
        }
    }

    
    private IEnumerator FadeUIElements(float targetAlpha, float fadeDuration) {
        mouseEntered = targetAlpha > 0;
        fadeOutStarted = false;
        boxColliders[1].enabled = targetAlpha > 0;

        float timer = 0f;
        float initialAlpha = backgroundImage.color.a;

        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(initialAlpha, targetAlpha, timer / fadeDuration);
            SetAlphaForAllUI(currentAlpha);
            yield return null;
        }

        SetAlphaForAllUI(targetAlpha); 
    }

    private IEnumerator DelayedFadeOut(float delayTime) {
        fadeOutStarted = true;
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeUIElements(0f, 1f)); 
    }

    
    private void SetAlphaForAllUI(float alpha) {
        SetAlpha(backgroundImage, alpha);
        SetAlpha(description, alpha);
        SetAlpha(upAtkSprite, alpha);
        SetAlpha(upSpdSprite, alpha);
        SetAlpha(upAtkText, alpha);
        SetAlpha(upSpdText, alpha);
        SetAlpha(upAtkLvlText,alpha);
        SetAlpha(upSpdLvlText,alpha);
    }

    
    private void SetAlpha(Image Image, float alpha) {
        if (Image != null) {
            Color color = Image.color;
            color.a = alpha;
            Image.color = color;
        }
    }

    private void SetAlpha(TextMeshProUGUI textMesh, float alpha) {
        if (textMesh != null) {
            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
    }

    private void updateMainBaseDamageString() {
        string dps = $"{damage:F0} dmg per atk";
        description.text = $"{buildingTypeSO.nameString}\nAtk: {dps}\nAtk Spd: {shootTimerMax:F2}s\nResources: {resourceGenerator.GetAmountGeneratedPerSecond():F1} {buildingTypeHolder.buildingType.resourceName}/s";
    }

    private void updateTowerString() {
        string dps = $"{damage:F0} dmg per atk";
        description.text = $"{buildingTypeSO.nameString}\nAtk: {dps}\nAtk Spd: {shootTimerMax:F2}s";
    }

    private void IncreaseAtk () {
        

        ResourceAmount[] resourceAmountCost = new ResourceAmount[nodeResourceType.Length];
    
        for (int i = 0; i < nodeResourceType.Length; i++) {
            resourceAmountCost[i] = new ResourceAmount { resourceType = nodeResourceType[i], amount = atkResourceCosts[i] };
        }

        if (ResourceManager.Instance.CanAfford(resourceAmountCost)) {
                
            attackLevel++;
            damage += 2;

            for (int i = 0; i < nodeResourceType.Length; i++) {
                
            atkResourceCosts[i] += 50f;
            
              
            }

            if (!isTower && buildingTypeHolder.buildingType.nameString == "Onigiri HQ") {
                updateMainBaseDamageString();
            } else if (isTower) {
                updateTowerString();
            }

            upAtkLvlText.text = "Lv " + attackLevel;

            StatsUpgraded?.Invoke(this, EventArgs.Empty);
                
            ResourceManager.Instance.SpendResources(resourceAmountCost);
                

        } else {

                
            ShowInsufficientResourcesTooltip(atkResourceCosts, nodeResourceType, "Attack");
        }
    }

    private void IncreaseSpd() {
        

        ResourceAmount[] resourceAmountCost = new ResourceAmount[spdUpgradeResourceType.Length];
    
        for (int i = 0; i < spdUpgradeResourceType.Length; i++) {
            resourceAmountCost[i] = new ResourceAmount { resourceType = spdUpgradeResourceType[i], amount = spdResourceCosts[i] };
        }

        if (ResourceManager.Instance.CanAfford(resourceAmountCost)) {
                
            speedLevel++;
            shootTimerMax -= 0.05f;
            
            
            
            for (int i = 0; i < spdUpgradeResourceType.Length; i++) {
            
                
            spdResourceCosts[i] += 60f;
            
                
            }

            if (!isTower && buildingTypeHolder.buildingType.nameString == "Onigiri HQ") {
                updateMainBaseDamageString();
            } else if (isTower) {
                updateTowerString();
            }

            upSpdLvlText.text = "Lv " + speedLevel;

            StatsUpgraded?.Invoke(this, EventArgs.Empty);
                
            ResourceManager.Instance.SpendResources(resourceAmountCost);
                

        } else {

                ShowInsufficientResourcesTooltip(spdResourceCosts, spdUpgradeResourceType, "Speed");
            
        }
    }

    private void ShowInsufficientResourcesTooltip(float[] resourceCosts, ResourceTypeSO[] resourceTypes, string upgradeType) {
    string tooltipMessage = $"Insufficient resources for {upgradeType} upgrade! Need: ";

    for (int i = 0; i < resourceTypes.Length; i++) {
        tooltipMessage += resourceCosts[i] + " " + resourceTypes[i].nameShort;
        if (i < resourceTypes.Length - 1) {
            tooltipMessage += ", ";  // Add a comma separator between resources
        }
    }

    TooltipUI.Instance.Show(tooltipMessage);
}

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter == upAtkGameObject && mouseEntered) {
            AtkToolTip();
        } else if (eventData.pointerEnter == upSpdGameObject && mouseEntered) {
            SpdToolTip();
        } else {
            TooltipUI.Instance.Hide();
        }
        


    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipUI.Instance.Hide();
    }

    
    private void AtkToolTip() {
        float[] resourceAmount = atkResourceCosts;

        

        ResourceAmount[] resourceAmountCost = new ResourceAmount[nodeResourceType.Length];
    
        for (int i = 0; i < nodeResourceType.Length; i++) {
            resourceAmountCost[i] = new ResourceAmount { resourceType = nodeResourceType[i], amount = resourceAmount[i] };
        }

        string tooltipMessage = "Need: ";

            for (int i = 0; i < nodeResourceType.Length; i++) {
                    tooltipMessage += resourceAmount[i] + " " + nodeResourceType[i].nameShort;
                    if (i < nodeResourceType.Length - 1) {
                        tooltipMessage += ", ";  
                        }
            }
        tooltipMessage += " to upgrade Attack";

        TooltipUI.Instance.Show(tooltipMessage);
    }

    private void SpdToolTip() {
        float[] resourceAmount = spdResourceCosts;

        

        ResourceAmount[] resourceAmountCost = new ResourceAmount[spdUpgradeResourceType.Length];
    
        for (int i = 0; i < spdUpgradeResourceType.Length; i++) {
            resourceAmountCost[i] = new ResourceAmount { resourceType = spdUpgradeResourceType[i], amount = resourceAmount[i] };
        }

        string tooltipMessage = "Need: ";

            for (int i = 0; i < spdUpgradeResourceType.Length; i++) {
                    tooltipMessage += resourceAmount[i] + " " + spdUpgradeResourceType[i].nameShort;
                    if (i < spdUpgradeResourceType.Length - 1) {
                        tooltipMessage += ", ";  // Add a comma separator between resources
                        }
            }
        tooltipMessage += " to upgrade Speed";

        TooltipUI.Instance.Show(tooltipMessage);
    }

    public string GetCurrentStatsLevel() {
        int totalLevel = attackLevel + speedLevel - 1;

        return totalLevel.ToString();
    }

    
}
