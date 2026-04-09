using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    public Transform spawnPoint;
    public Transform enemy;
    private BulletSpawner bulletSpawner;

    private float canShootTimer = .2f;

    private void Start()
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    private void Update()
    {
        canShootTimer -= Time.deltaTime;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(canShootTimer <= 0f)
        {
            Shoot();
            canShootTimer = .2f;
        }
    }

    void Shoot()
    {
        //GameObject bulletInst = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        bulletSpawner._pool.Get();
    }
}