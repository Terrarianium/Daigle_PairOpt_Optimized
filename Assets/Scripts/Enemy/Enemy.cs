using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Range(0, 50)][SerializeField] float attackRange = 2, sightRange = 20, timeBetweenAttacks = 3;
    [Range(0, 20)][SerializeField] int atkDamage = 5;

    private NavMeshAgent navEnemy;
    private Transform playerPos;
    private bool isAttacking;

    private Health health;
    [SerializeField] PlayerController playerController;
    public GameObject bullet;

    private enum EnemyState
    {
        Chase,
        Attack,
        Idle,
        Death
    }

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        navEnemy = GetComponent<NavMeshAgent>();
        playerPos = playerController.transform;
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
                StartCoroutine(AttackPlayer());
                break;

            case EnemyState.Idle:
                navEnemy.isStopped = true;
                break;

            case EnemyState.Death:
                Debug.Log("I am Dead makdfad");
                Destroy(gameObject);
                break;
        }
    }

    private EnemyState GetState(float distance)
    {
        if (health.isDead)
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
            health.TakeDamage(5);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        yield return new WaitForSeconds(timeBetweenAttacks);

        Debug.Log("hit Player");
        health.TakeDamage(atkDamage);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
