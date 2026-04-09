using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private float speed = 20f;
    private float destroyTime = 3f;

    private Rigidbody rb;
    private ObjectPool<Bullet> _pool;

    //State
    private bool isActiveBullet = false;
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (!isActiveBullet) return;

        // Reset lifetime
        lifeTimer = destroyTime;

        // Fire forward
        rb.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        if (!isActiveBullet) return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            _pool.Release(this);
        }
    }

    private void OnDisable()
    {
        // Reset velocity (important for pooling)
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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

    public void SetActiveState(bool state)
    {
        isActiveBullet = state;
    }
}