using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBetweenSpawns = 5f;
    private float timeSinceLastSpawn;

    [SerializeField] Enemy enemyPrefab;
    private IObjectPool<Enemy> enemyPool;

    [SerializeField] int maxAlive = 5;
    private int currentAlive = 0;

    [SerializeField] UIManager ui;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, OnDestroyEnemy, collectionCheck: false, defaultCapacity: 5, maxSize: 5);
    }

    private void OnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;

        enemy.ResetEnemy();
    }

    private void OnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        currentAlive--;
    }


    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);

        enemy.OnDeath += HandleEnemyDeath;

        return enemy;
    }

    private void HandleEnemyDeath()
    {
        currentAlive--;
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    void Update()
    {
        timeSinceLastSpawn -= Time.deltaTime;

        if (ui.startGame && timeSinceLastSpawn <= 0f && currentAlive < maxAlive)
        {
            enemyPool.Get();
            currentAlive++;
            timeSinceLastSpawn = timeBetweenSpawns;
        }
    }
}
