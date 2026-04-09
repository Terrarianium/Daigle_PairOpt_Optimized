using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    public ObjectPool<Bullet> _pool;
    private Gun gun;

    void Start()
    {
        gun = GetComponent<Gun>();
        _pool = new ObjectPool<Bullet>(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 100, 200);

        InitiateBullets(30);
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

        bullet.SetActiveState(true);

        //activate
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBulletToPool(Bullet bullet)
    {
        bullet.SetActiveState(false);

        //deactivate
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    void InitiateBullets(int count)
    {
        Bullet[] temp = new Bullet[count];

        //create temp bullets
        for (int i = 0; i < count; i++)
        {
            temp[i] = _pool.Get();
        }

        //remove temp bullets
        for (int i = 0; i < count; i++)
        {
            _pool.Release(temp[i]);
        }
    }
}