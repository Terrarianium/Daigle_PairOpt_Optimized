using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    public Transform spawnPoint;
    public Transform enemy;
    private BulletSpawner bulletSpawner;

    private void Start()
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Shoot()
    {
        //GameObject bulletInst = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        bulletSpawner._pool.Get();
    }
}