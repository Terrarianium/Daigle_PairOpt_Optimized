using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    public ObjectPool<Bullet> _pool;
    private Gun gun;

    void Start()
    {
        gun = GetComponent<Gun>();
        _pool = new ObjectPool<Bullet>(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 100, 1000);
    }

    private Bullet CreateBullet()
    {
        //spawn new instance of the bullet
        Bullet bullet = Instantiate(gun.bullet, gun.spawnPoint.position, gun.spawnPoint.rotation);
        bullet.SetPool(_pool);
        return bullet;
    }

    private void OnTakeBulletFromPool(Bullet bullet)
    {
        //set transform
        bullet.transform.position = gun.spawnPoint.position;
        bullet.transform.right = gun.transform.right;

        //activate
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBulletToPool(Bullet bullet)
    {
        //deactivate
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
