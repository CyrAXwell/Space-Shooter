using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float numbersOfShots;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeTimeDelta;
    [SerializeField] private Animator animator;

    private Enemy _enemyStats;
    private float _timer;
    private float _shotCounter;
    private ObjectPoolManager _objectPool;

    public void Initialize(ObjectPoolManager objectPool)
    {
        _objectPool = objectPool;
    }

    private void OnEnable()
    {
        _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        _enemyStats = GetComponent<Enemy>();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Shoot();

            if(numbersOfShots == 1)
            {
                _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
            } 
            else
            {
                _shotCounter ++;
                _timer = timeBetweenShots;
                if (_shotCounter == numbersOfShots)
                {
                    _shotCounter = 0;
                    _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
                }
            }
        }
    }

    void Shoot()
    {
        Bullet bullet = _objectPool.GetBullet(bulletPrefab);

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.gameObject.name = bulletPrefab.name.ToString();
        _objectPool.ReleaseBullet(bullet, 2f);

        bullet.Initialize(_objectPool, _enemyStats.GetDamage());
        
        animator.SetTrigger("Shoot");
    }
}
