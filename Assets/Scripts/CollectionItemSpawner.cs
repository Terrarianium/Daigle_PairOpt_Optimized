using System.Collections.Generic;
using UnityEngine;

public class CollectionItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int spawnCount;

    private List<Transform> availablePoints = new List<Transform>();

    void Start()
    {
        // Copy all spawn points into a usable list
        availablePoints = new List<Transform>(spawnPoints);

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
        Transform selectedPoint = availablePoints[randomIndex];
        Instantiate(objectToSpawn, selectedPoint.position, selectedPoint.rotation);
        availablePoints.RemoveAt(randomIndex);
    }
}