using System.Collections;
using UnityEngine;

public class PlasmaShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    private EnemyBossShooting _bossStats;
    private ObjectPoolManager _objectPool;
    private bool _canShoot = false;
    private bool _reload = false;
    private float _timeBetweenShot = 0f;

    public void Initialize(ObjectPoolManager objectPool)
    {
        _objectPool = objectPool;
    }

    private void Start()
    {
        _bossStats = transform.parent.parent.GetComponent<EnemyBossShooting>();
    }

    private void Update()
    {
        if(_canShoot && !_reload)
        {
            Shoot();
            _reload = true;
            StartCoroutine(ReloadShot(_timeBetweenShot));
        }
    }

    private void Shoot()
    {
        Bullet bullet = _objectPool.GetObject(bulletPrefab).GetComponent<Bullet>();
        bullet.gameObject.name = bulletPrefab.name.ToString();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Initialize(_objectPool, _bossStats.GetPlasmaDamage());
        _objectPool.ReleaseObject(bullet, 3f);
    }

    private IEnumerator ReloadShot(float interval)
    {
        yield return new WaitForSeconds(interval);
        _reload = false;
    }

    public void StartAttack(float time)
    {
        _timeBetweenShot = time;
        _canShoot = true;
    }

    public void StopAttack()
    {
        _canShoot = false;
    }
}
