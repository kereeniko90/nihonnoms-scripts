using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStone : MonoBehaviour {

    [SerializeField] private GameObject stoneGenerator;
    [SerializeField] private GameObject gameMap;
    [SerializeField] private GameObject stonePrefabs;

    [SerializeField] private GameObject mainBase;
    [SerializeField] private float minBaseDistance = 20.0f;
    [SerializeField] private int numberOfStones = 50;
    [SerializeField] private float mapBuffer = 50.0f;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private int maxAttempts = 30;
    [SerializeField] private int numberOfClusters;

    [SerializeField] private int minNumberOfClusters = 10;
    [SerializeField] private int maxNumberOfClusters = 30;

    [SerializeField] private int minStonesPerCluster = 5;

    [SerializeField] private int maxStonesPerCluster = 20;
    private int stonesPerCluster;
    [SerializeField] private float clusterRadius = 15.0f;

    private Vector2 mapSize;
    private Vector2 mapCenter;

    private Vector2 basePosition;

    private int remainingStones;
    private List<Vector2> stonePositions = new List<Vector2>();
    private void Awake() {  
        remainingStones = numberOfStones;
        CalculateMapBounds();
        Randomizestones();
        
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

    private void Randomizestones() {
        
        numberOfClusters = Random.Range(minNumberOfClusters, maxNumberOfClusters);

        // Generate clusters of stones

        if (remainingStones > 0) {
            for (int i = 0; i < numberOfClusters; i++)
        {
            Vector2 clusterCenter = GetValidstonePosition(); // Create a cluster center
            if (clusterCenter != Vector2.zero)
            {
                GenerateCluster(clusterCenter);
            }
        }
        }
        

        // Generate scattered stones
        
        for (int i = 0; i < remainingStones; i++)
        {
            Vector2 position = GetValidstonePosition();
            if (position != Vector2.zero)
            {
                GameObject newstone = Instantiate(stonePrefabs, position, Quaternion.identity);
                remainingStones--;
                newstone.transform.SetParent(stoneGenerator.transform);
                stonePositions.Add(position);
            }
        }
    }

    private void GenerateCluster(Vector2 clusterCenter)
    {
        stonesPerCluster = Random.Range(minStonesPerCluster, maxStonesPerCluster);

        for (int i = 0; i < stonesPerCluster; i++)
        {
            Vector2 position;
            int attempt = 0;
            do
            {
                // Create stones within the clusterRadius of the cluster center
                Vector2 offset = Random.insideUnitCircle * clusterRadius;
                position = clusterCenter + offset;
                attempt++;
            }
            while (!IsPositionValid(position) && attempt < maxAttempts);

            if (position != Vector2.zero)
            {
                GameObject newstone = Instantiate(stonePrefabs, position, Quaternion.identity);
                remainingStones--;
                newstone.transform.SetParent(stoneGenerator.transform);
                stonePositions.Add(position);
            }
        }
    }

    private Vector2 GetValidstonePosition() {
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
