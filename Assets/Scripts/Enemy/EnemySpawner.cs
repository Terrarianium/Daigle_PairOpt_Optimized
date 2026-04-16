using System.Collections;
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
    public int currentAlive = 0;

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

        return enemy;
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    /* Subscribe and unsubscribe to the OnActiveGame event.
     */
    private void OnEnable()
    {
        UIManager.OnActiveGame += HandleActiveGame;
    }
    private void OnDisable()
    {
        UIManager.OnActiveGame -= HandleActiveGame;
    }

    /* Start or stop spawning enemies based on the game state.
     */
    private void HandleActiveGame(bool isActive)
    {
        if (!isActive)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    /* Coroutine that spawns enemies at regular intervals while the game is active and the maximum number of alive enemies has not been reached.
     */
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentAlive < maxAlive)
            {
                enemyPool.Get();
                currentAlive++;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
