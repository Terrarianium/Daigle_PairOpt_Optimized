using System.Collections.Generic;
using UnityEngine;

public class CollectionItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Vector3[] spawnPoints; // Change to array of Vector3
    [SerializeField] int spawnCount;

    private List<Vector3> availablePoints; // Changed to list of Vector 3; removed list creation due to initialization in Start()

    void Start()
    {
        // Copy all spawn points into a usable list
        availablePoints = new List<Vector3>(spawnPoints); // change list to Vector3

        spawnCount = Mathf.Min(spawnCount, availablePoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnAtRandomPoint();
        }
    }

    public void SpawnAtRandomPoint()
    {
        if (availablePoints.Count == 0) return;

        int randomIndex = Random.Range(0, availablePoints.Count);
        Vector3 selectedPoint = availablePoints[randomIndex]; // Change selectedPoint to Vector3
        Instantiate(objectToSpawn, selectedPoint, Quaternion.identity); // Use selectedPoint directly as the position with Quaternion.identity for rotation
        availablePoints.RemoveAt(randomIndex);
    }
}