using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamagedFadingOverlay : MonoBehaviour {

    public float floatSpeed = 2.0f;

    private RectTransform text;
    private TextMeshPro textFont;

    private Color defaultColor = Color.red;
    
    private Vector3 initialPosition;

    private void Awake() {
        text = GetComponentInChildren<RectTransform>();
        textFont = GetComponentInChildren<TextMeshPro>();
        
    }

    private void Start() {
        initialPosition = transform.position;
        textFont.color = defaultColor;
    }

    private void Update() {
        text.localScale = new Vector3(text.localScale.x - Time.deltaTime * 0.4f, text.localScale.y - Time.deltaTime * 0.4f, text.localScale.z);

        

        textFont.color = new Color(textFont.color.r, textFont.color.g, textFont.color.b, Mathf.Clamp01(1 - Time.deltaTime * 30f) );
        transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);
    }
}
