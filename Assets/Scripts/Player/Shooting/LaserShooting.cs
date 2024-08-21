using System.Collections;
using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer laserRender;
    [SerializeField] private float maxDistance;
    [SerializeField] private float timeBetweenTakeDamage;
    [SerializeField] private LayerMask whatIsSolid;

    private bool _canDamage = true;
    private bool _canShoot = false;
    private EnemyBossShooting _bossStats;

    private void Start()
    {
        _bossStats = transform.parent.parent.GetComponent<EnemyBossShooting>();
    }

    private void FixedUpdate()
    {
        if(_canShoot)
        {
            Vector2 startPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y);
            Vector2 endPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y - maxDistance);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsSolid);

            bool isHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null) 
                {
                    if (hits[i].collider.CompareTag("Player") && _canDamage) {
                        hits[i].collider.GetComponent<Player>().TakeDamage(_bossStats.GetLaserDamage()); 
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));  
                    }
                    if (hits[i].collider.CompareTag("Shield") && _canDamage) {
                        hits[i].collider.GetComponent<Shield>().TakeDamage(_bossStats.GetLaserDamage());
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage)); 
                    }
                    
                    laserRender.size = new Vector2(laserRender.size.x, laserRender.transform.position.y - hits[i].point.y);
                    isHit = true;
                    break;
                }
            }
            if(!isHit)
               laserRender.size = new Vector2(laserRender.size.x, maxDistance); 
        }
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        _canDamage = false;
        yield return new WaitForSeconds(interval);
        _canDamage = true;
    }

    public void StartAttack()
    {
        laserRender.gameObject.SetActive(true);
        laserRender.size = new Vector2(laserRender.size.x, 0f); 
        _canShoot = true;
        _canDamage = true;
    }

    public void StopAttack()
    {
        laserRender.gameObject.SetActive(false);
        _canShoot = false;
    }
}
