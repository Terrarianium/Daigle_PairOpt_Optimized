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

    private bool hasBeenReleased;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (!isActiveBullet) return;

        hasBeenReleased = false;

        // Reset lifetime
        lifeTimer = destroyTime;

        // Fire forward
        //rb.linearVelocity = transform.forward * speed;
        rb.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        if (!isActiveBullet) return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReleaseBullet();
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
            ReleaseBullet();
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

    private void ReleaseBullet()
    {
        if (hasBeenReleased) return;

        hasBeenReleased = true;
        _pool.Release(this);
    }
}