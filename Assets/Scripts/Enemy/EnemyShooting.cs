using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float numbersOfShots;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeTimeDelta;
    [SerializeField] private Animator animator;

    private Enemy _enemyStats;
    private float _timer;
    private float _shotCounter;

    void Start()
    {
        _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        _enemyStats = GetComponent<Enemy>();
    }

    void Update()
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletStats = bullet.GetComponent<Bullet>();
        bulletStats.Initialize(_enemyStats.GetDamage());
        
        animator.SetTrigger("Shoot");
    }
}
