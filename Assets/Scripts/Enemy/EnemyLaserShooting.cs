using System.Collections;
using UnityEngine;

public class EnemyLaserShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer laserLine; 
    [SerializeField] private float maxDistance;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeTimeDelta;
    [SerializeField] private float shootingTime;
    [SerializeField] private float timeBetweenTakeDamage;
    [SerializeField] private LayerMask whatIsSolid;

    private Enemy _enemyStats;
    private float _timer;
    private bool _canDamage = true;

    private void OnEnable()
    {
        _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        _enemyStats = GetComponent<Enemy>();
    }

    private void Update()
    {
        if ( _timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            laserLine.gameObject.SetActive(true);
            StartCoroutine(EndShooting(shootingTime));
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector2 startPoint = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 endPoint = new Vector2(firePoint.position.x, firePoint.position.y - maxDistance);
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsSolid);

        bool isHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            
            if (hits[i].collider != null) 
            {
                if (hits[i].collider.CompareTag("Player") && _canDamage) {
                    hits[i].collider.GetComponent<Player>().TakeDamage(_enemyStats.GetDamage()); 
                    StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));  
                }
                if (hits[i].collider.CompareTag("Shield") && _canDamage) {
                    hits[i].collider.GetComponent<Shield>().TakeDamage(_enemyStats.GetDamage());
                    StartCoroutine(CanLaserDamage(timeBetweenTakeDamage)); 
                }
                
                laserLine.size = new Vector2(laserLine.size.x, firePoint.position.y - hits[i].point.y);
                isHit = true;
                break;
            }
        }

        if (!isHit)
            laserLine.size = new Vector2(laserLine.size.x, maxDistance); 
    }
    

    private IEnumerator EndShooting(float interval)
    {
        yield return new WaitForSeconds(interval);
        _timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        laserLine.gameObject.SetActive(false);
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        _canDamage = false;
        yield return new WaitForSeconds(interval);
        _canDamage = true;
    }



}
