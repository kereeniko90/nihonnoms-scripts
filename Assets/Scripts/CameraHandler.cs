using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    public static CameraHandler Instance { get; private set;}

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;
    private bool edgeScrolling;

    

    private void Awake() {
        Instance = this;
        edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 0) == 1;
    }

    private void Start() {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }
    private void Update(){
        HandleMovement();
        HandleZoom();

        
    }

    private void HandleMovement(){
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        

        if (edgeScrolling) {
        float edgeScrollingSize = 30f;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize) {
                x = 1f;
            }

            if (Input.mousePosition.x < edgeScrollingSize) {
                x = -1f;
            }

            if (Input.mousePosition.y > Screen.height - edgeScrollingSize) {
                y = 1f;
            }

            if (Input.mousePosition.y < edgeScrollingSize) {
                y = -1f;
            }
        }

        Vector3 moveDir = new Vector3(x,y).normalized;
        float moveSpeed = 35f;

        transform.position += moveSpeed * Time.deltaTime * moveDir;

        
    }

    private void HandleZoom(){
        float zoomAmount = 2f;

        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        float minOrthographicSize = 10;
        float maxOrthographicSize = 30;
        float zoomSpeed = 10f;

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize );
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    public void SetEdgeScrolling(bool edgeScrolling) {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling() {
        return edgeScrolling;
    }
    
}
