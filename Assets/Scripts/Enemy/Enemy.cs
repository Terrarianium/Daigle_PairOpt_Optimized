using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [Range(0, 50)][SerializeField] float attackRange = 2, sightRange = 20, timeBetweenAttacks = 3;
    [Range(0, 20)][SerializeField] int atkDamage = 5;

    private NavMeshAgent navEnemy;
    private Transform playerPos;
    private bool isAttacking;

    private Health playerHealth;
    private Health enemyHealth;
    private PlayerController playerController;

    private IObjectPool<Enemy> enemyPool;
    private bool isReleased;

    public System.Action OnDeath;

    private UIManager uiManager;

    public void SetPool(IObjectPool<Enemy> pool)
    {
        enemyPool = pool;
    }

    private enum EnemyState
    {
        Chase,
        Attack,
        Idle,
        Death
    }

    private void Awake()
    {
        navEnemy = GetComponent<NavMeshAgent>();
        enemyHealth = gameObject.GetComponent<Health>();
    }

    private void OnEnable()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();
        playerHealth = playerController.GetComponent<Health>();
        uiManager = Object.FindFirstObjectByType<UIManager>();
        playerPos = playerController.transform;

        isReleased = false;
        isAttacking = false;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, transform.position);

        EnemyState currentState = GetState(distanceFromPlayer);

        switch (currentState)
        {
            case EnemyState.Chase:
                isAttacking = false;
                navEnemy.isStopped = false;
                StopAllCoroutines();
                ChasePlayer();
                break;

            case EnemyState.Attack:
                navEnemy.isStopped = true;
                if (!isAttacking)
                    isAttacking = true;
                    uiManager.Restart();
                break;

            case EnemyState.Idle:
                navEnemy.isStopped = true;
                break;

            case EnemyState.Death:
                HandleDeath();
                break;
        }
    }

    private EnemyState GetState(float distance)
    {
        if (enemyHealth.isDead)
            return EnemyState.Death;

        if (distance <= attackRange && !isAttacking)
            return EnemyState.Attack;

        if (distance <= sightRange && distance > attackRange)
            return EnemyState.Chase;

        return EnemyState.Idle;
    }

    private void ChasePlayer()
    {
        navEnemy.SetDestination(playerPos.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit by: " + other.name);

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit enemy");
            enemyHealth.TakeDamage(5);
        }
    }

    private void HandleDeath()
    {
        if (isReleased) return;

        isReleased = true;

        StopAllCoroutines();
        navEnemy.isStopped = true;

        OnDeath?.Invoke();
        enemyPool.Release(this);
    }

    public void ResetEnemy()
    {
        enemyHealth.ResetHealth(); // you must implement or already have this
        isReleased = false;
        isAttacking = false;

        StopAllCoroutines();
        navEnemy.isStopped = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}