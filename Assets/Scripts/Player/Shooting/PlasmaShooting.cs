using System.Collections;
using UnityEngine;

public class PlasmaShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private EnemyBossShooting _bossStats;
    private bool _canShoot = false;
    private bool _reload = false;
    private float _timeBetweenShot = 0f;

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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletStats = bullet.GetComponent<Bullet>();
        bulletStats.Initialize(_bossStats.GetPlasmaDamage());
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
