using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingStats : MonoBehaviour {

    private TextMeshPro description;
    private SpriteRenderer backgroundImage;
    private bool mouseEntered = false;
    private bool fadeOutStarted = false;
    private BuildingTypeHolder buildingTypeHolder;
    private ResourceGenerator resourceGenerator;
    private BoxCollider2D[] boxColliders;
    private BuildingTypeSO buildingTypeSO;
    private bool isTower;
    private int damage;
    private float shootTimerMax;
    private Transform upAtkTransform;
    private Transform upSpdTransform;
    private SpriteRenderer upAtkSprite;
    private SpriteRenderer upSpdSprite;
    private TextMeshPro upAtkText;
    private TextMeshPro upAtkLvlText;
    private TextMeshPro upSpdText;
    private TextMeshPro upSpdLvlText;

    private void Awake() {
        boxColliders = GetComponents<BoxCollider2D>();
    }

    private void Start() {
        boxColliders[1].enabled = false;

        buildingTypeHolder = transform.parent.GetComponent<BuildingTypeHolder>();
        resourceGenerator = transform.parent.GetComponent<ResourceGenerator>();

        upAtkTransform = transform.Find("increaseAtk");
        upSpdTransform = transform.Find("increaseSpd");

        description = transform.Find("text").GetComponent<TextMeshPro>();
        backgroundImage = transform.Find("background").GetComponent<SpriteRenderer>();

        upAtkSprite = upAtkTransform.GetComponent<SpriteRenderer>();
        upSpdSprite = upSpdTransform.GetComponent<SpriteRenderer>();

        upAtkText = upAtkTransform.Find("text").GetComponent<TextMeshPro>();
        upSpdText = upSpdTransform.Find("text").GetComponent<TextMeshPro>();
        upAtkLvlText = upAtkTransform.Find("atkLvl").GetComponent<TextMeshPro>();
        upSpdLvlText = upSpdTransform.Find("spdLvl").GetComponent<TextMeshPro>();

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
        string dps = $"{damage:F0} dmg per atk";
        if (!isTower && buildingTypeHolder.buildingType.nameString == "Onigiri HQ") {
            description.text = $"{buildingTypeSO.nameString}\nAtk: {dps}\nAtk Spd: {buildingTypeSO.shootTimerMax}s\nResources: {resourceGenerator.GetAmountGeneratedPerSecond():F1} {buildingTypeHolder.buildingType.resourceName}/s";
        } else if (isTower) {
            description.text = $"{buildingTypeSO.nameString}\nAtk: {dps}\nAtk Spd: {buildingTypeSO.shootTimerMax}s";
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

    
    private void SetAlpha(SpriteRenderer spriteRenderer, float alpha) {
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    private void SetAlpha(TextMeshPro textMesh, float alpha) {
        if (textMesh != null) {
            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
    }
}
