using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateTrees : MonoBehaviour {   
    [SerializeField] private GameObject treeGenerator;
    [SerializeField] private GameObject gameMap;
    [SerializeField] private GameObject[] treePrefabs;

    [SerializeField] private GameObject mainBase;
    [SerializeField] private float minBaseDistance = 20.0f;
    [SerializeField] private int numberOfTrees = 50;
    [SerializeField] private float mapBuffer = 50.0f;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private int maxAttempts = 30;
    [SerializeField] private int minNumberOfClusters = 10;
    [SerializeField] private int maxNumberOfClusters = 30;
    [SerializeField] private int minTreesPerCluster = 5;

    [SerializeField] private int maxTreesPerCluster = 20;
    [SerializeField] private float clusterRadius = 15.0f;

    private int treesPerCluster;

    private int remainingTrees;

    private Vector2 mapSize;
    private Vector2 mapCenter;

    private Vector2 basePosition;
    private List<Vector2> treePositions = new List<Vector2>();
    private void Awake() {  
        
        remainingTrees = numberOfTrees;
        CalculateMapBounds();
        RandomizeTrees();
        
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

    private void RandomizeTrees() {
        
        int numberOfClusters = Random.Range(minNumberOfClusters, maxNumberOfClusters);
        

        // Generate clusters of trees
        if (remainingTrees > 0) {
            for (int i = 0; i < numberOfClusters; i++) {

                Vector2 clusterCenter = GetValidTreePosition(); // Create a cluster center
                if (clusterCenter != Vector2.zero) {
                    GenerateCluster(clusterCenter);
                }
            }
        }
        

        // Generate scattered trees
        //int remainingTrees = numberOfTrees - (numberOfClusters * treesPerCluster);
        for (int i = 0; i < remainingTrees; i++)
        {
            Vector2 position = GetValidTreePosition();
            if (position != Vector2.zero)
            {
                float randomScaleX = Random.Range(0.50f, 1.00f);
                float randomScaleY = Random.Range(0.50f, 1.00f);
                GameObject newTree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], position, Quaternion.identity);
                remainingTrees--;
                newTree.transform.SetParent(treeGenerator.transform);
                newTree.transform.localScale = new Vector2(randomScaleX, randomScaleY);
                treePositions.Add(position);
            }
        }
    }

    private void GenerateCluster(Vector2 clusterCenter)
    {
        treesPerCluster = Random.Range(minTreesPerCluster, maxTreesPerCluster);

        for (int i = 0; i < treesPerCluster; i++)
        {
            Vector2 position;
            int attempt = 0;
            do
            {
                // Create trees within the clusterRadius of the cluster center
                Vector2 offset = Random.insideUnitCircle * clusterRadius;
                position = clusterCenter + offset;
                attempt++;
            }
            while (!IsPositionValid(position) && attempt < maxAttempts);

            if (position != Vector2.zero)
            {
                GameObject newTree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], position, Quaternion.identity);
                remainingTrees--;
                newTree.transform.SetParent(treeGenerator.transform);
                treePositions.Add(position);
            }
        }
    }

    private Vector2 GetValidTreePosition() {
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
