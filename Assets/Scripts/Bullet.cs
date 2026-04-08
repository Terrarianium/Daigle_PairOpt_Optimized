using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float turnRate = 720f;
    private float destroyTime = 3f;
    private Transform enemy;
    private Rigidbody rb;

    private ObjectPool<Bullet> _pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = transform.forward * speed;
        StartCoroutine(DeactivateBulletAfterTime());
    }

    private void FixedUpdate()
    {
        if (!enemy) return;

        //Turn toward Target
        /*Vector3 dir = (enemy.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, turnRate * Time.deltaTime));*/

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _pool.Release(this);
        }
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    private IEnumerator DeactivateBulletAfterTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime)
        { 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _pool.Release(this);
    }
}