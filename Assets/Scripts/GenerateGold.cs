using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGold : MonoBehaviour {
    [SerializeField] private GameObject goldGenerator;
    [SerializeField] private GameObject gameMap;
    [SerializeField] private GameObject goldPrefabs;

    [SerializeField] private GameObject mainBase;
    [SerializeField] private float minBaseDistance = 20.0f;
    [SerializeField] private int numberOfGolds = 50;
    [SerializeField] private float mapBuffer = 50.0f;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private int maxAttempts = 30;
    [SerializeField] private int numberOfClusters = 5;
    [SerializeField] private int goldsPerCluster = 5;
    [SerializeField] private float clusterRadius = 15.0f;

    private Vector2 mapSize;
    private Vector2 mapCenter;

    private Vector2 basePosition;

    private int remainingGold;
    private List<Vector2> goldPositions = new List<Vector2>();
    private void Awake() {  
        remainingGold = numberOfGolds;
        CalculateMapBounds();
        Randomizegolds();
        
    }

   

    private void CalculateMapBounds()
    {
        // Get the size of the background map
        SpriteRenderer mapRenderer = gameMap.GetComponent<SpriteRenderer>();
        if (mapRenderer == null)
        {
            Debug.LogError("Background map must have a SpriteRenderer component!");
            return;
        }

        mapSize = mapRenderer.bounds.size;
        mapCenter = mapRenderer.bounds.center;
    }

    private void Randomizegolds() {
        
        numberOfClusters = Random.Range(5, 20);

        // Generate clusters of golds
        if (remainingGold > 0) {
            for (int i = 0; i < numberOfClusters; i++)
        {
            Vector2 clusterCenter = GetValidgoldPosition(); // Create a cluster center
            if (clusterCenter != Vector2.zero)
            {
                GenerateCluster(clusterCenter);
            }
        }
        }
        

        // Generate scattered golds
        
        for (int i = 0; i < remainingGold; i++)
        {
            Vector2 position = GetValidgoldPosition();
            if (position != Vector2.zero)
            {
                GameObject newgold = Instantiate(goldPrefabs, position, Quaternion.identity);
                remainingGold--;
                newgold.transform.SetParent(goldGenerator.transform);
                goldPositions.Add(position);
            }
        }
    }

    private void GenerateCluster(Vector2 clusterCenter)
    {
        goldsPerCluster = Random.Range(1, 2);

        for (int i = 0; i < goldsPerCluster; i++)
        {
            Vector2 position;
            int attempt = 0;
            do
            {
                // Create golds within the clusterRadius of the cluster center
                Vector2 offset = Random.insideUnitCircle * clusterRadius;
                position = clusterCenter + offset;
                attempt++;
            }
            while (!IsPositionValid(position) && attempt < maxAttempts);

            if (position != Vector2.zero)
            {
                GameObject newgold = Instantiate(goldPrefabs, position, Quaternion.identity);
                remainingGold--;
                newgold.transform.SetParent(goldGenerator.transform);
                goldPositions.Add(position);
            }
        }
    }

    private Vector2 GetValidgoldPosition() {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(mapCenter.x - mapSize.x / 2 + mapBuffer, mapCenter.x + mapSize.x / 2 - mapBuffer),
                Random.Range(mapCenter.y - mapSize.y / 2 + mapBuffer, mapCenter.y + mapSize.y / 2 - mapBuffer)
            );

            if (IsPositionValid(randomPosition))
            {
                return randomPosition;
            }
        }
        return Vector2.zero;
    }

    private bool IsPositionValid(Vector2 position) {

        
        float checkRadius = minDistance / 2.0f;  

        // Check for overlap using the position and radius of the trees
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, checkRadius);
        if (hitColliders.Length > 0) {
            return false;  // If there's anything in the circle, position is invalid
        }

        // Check distance from the main base
        if (Vector2.Distance(position, basePosition) < minBaseDistance) {
            return false;
        }

        // If position is valid
        return true;
    }
    
}
