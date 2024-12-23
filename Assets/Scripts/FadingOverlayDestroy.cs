using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingOverlayDestroy : MonoBehaviour {

    private float killTimer = 0f;
    private float killDuration = 1f;

    private Transform fadingSpriteTransform;
    private Transform fadingTextTransform;
    private TextMeshPro fadingText;
    private SpriteRenderer fadingSprite;

    private void Awake() {
        fadingSpriteTransform = transform.Find("fadingIcon");
        fadingTextTransform = transform.Find("fadingText");
        fadingText = fadingTextTransform.GetComponent<TextMeshPro>();
        fadingSprite = fadingSpriteTransform.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        
    }
    
    private void Update() {
        transform.position += new Vector3(0, 3f * Time.deltaTime, 0);
        killTimer += Time.deltaTime;

        fadingText.color = new Color(fadingText.color.r, fadingText.color.g, fadingText.color.b, 1f - killTimer);
        fadingSprite.color = new Color(fadingSprite.color.r, fadingSprite.color.g, fadingSprite.color.b, 1f - killTimer);

        if (killTimer > killDuration) {
            Destroy(gameObject);
        }

    }
}
