using System.Collections;
using UnityEngine;

public class PlasmaShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private EnemyBossShooting bossStats;
    private bool canShoot = false;
    private bool reload = false;
    private float timeBetweenShot = 0f;
    private Bullet bulletStats;

    void Start()
    {
        bossStats = transform.parent.parent.GetComponent<EnemyBossShooting>();
    }
    void FixedUpdate()
    {
        if(canShoot && !reload)
        {
            Shoot();
            reload = true;
            StartCoroutine(ReloadShot(timeBetweenShot));
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletStats = bullet.GetComponent<Bullet>();
        bulletStats.Initialize(bossStats.plasmaDamage);
    }

    private IEnumerator ReloadShot(float interval)
    {
        yield return new WaitForSeconds(interval);
        reload = false;
    }

    public void StartAttack(float time)
    {
        timeBetweenShot = time;
        canShoot = true;
    }

    public void StopAttack()
    {
        canShoot = false;
    }
}
