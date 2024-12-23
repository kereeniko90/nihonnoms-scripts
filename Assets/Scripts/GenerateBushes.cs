using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBushes : MonoBehaviour {
    
    [SerializeField] private GameObject bushGenerator;
    [SerializeField] private GameObject gameMap;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private int numberOfBushes = 50;
    [SerializeField] private float mapBuffer = 50.0f;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private int maxAttempts = 30;
    [SerializeField] private int numberOfClusters = 5;
    [SerializeField] private int bushsPerCluster = 5;
    [SerializeField] private float clusterRadius = 15.0f;

    private Vector2 mapSize;
    private Vector2 mapCenter;
    private List<Vector2> bushPositions = new List<Vector2>();
    private void Awake() {  
        
        CalculateMapBounds();
        Randomizebushs();
        
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

    private void Randomizebushs() {
        
        numberOfClusters = Random.Range(10, 60);

        // Generate clusters of bushs
        for (int i = 0; i < numberOfClusters; i++)
        {
            Vector2 clusterCenter = GetValidbushPosition(); // Create a cluster center
            if (clusterCenter != Vector2.zero)
            {
                GenerateCluster(clusterCenter);
            }
        }

        // Generate scattered bushs
        int remainingbushes = numberOfBushes - (numberOfClusters * bushsPerCluster);
        for (int i = 0; i < remainingbushes; i++)
        {
            Vector2 position = GetValidbushPosition();
            if (position != Vector2.zero)
            {
                GameObject bushPrefab = bushPrefabs[Random.Range(0, bushPrefabs.Length)];
                GameObject newbush = Instantiate(bushPrefab, position, Quaternion.identity);
                newbush.transform.SetParent(bushGenerator.transform);
                bushPositions.Add(position);
            }
        }
    }

    private void GenerateCluster(Vector2 clusterCenter)
    {
        bushsPerCluster = Random.Range(5, 10);

        for (int i = 0; i < bushsPerCluster; i++)
        {
            Vector2 position;
            int attempt = 0;
            do
            {
                // Create bushs within the clusterRadius of the cluster center
                Vector2 offset = Random.insideUnitCircle * clusterRadius;
                position = clusterCenter + offset;
                attempt++;
            }
            while (!IsPositionValid(position) && attempt < maxAttempts);

            if (position != Vector2.zero)
            {
                GameObject bushPrefab = bushPrefabs[Random.Range(0, bushPrefabs.Length)];
                GameObject newbush = Instantiate(bushPrefab, position, Quaternion.identity);
                newbush.transform.SetParent(bushGenerator.transform);
                bushPositions.Add(position);
            }
        }
    }

    private Vector2 GetValidbushPosition() {
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

        


        foreach (Vector2 bushPos in bushPositions)
        {
            if (Vector2.Distance(position, bushPos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }



}
