using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    //[SerializeField] private int laserDamage = 10;
    [SerializeField] private SpriteRenderer laserRender;
    [SerializeField] private float maxDistance;
    //[SerializeField] private float rechargeTime;
    //[SerializeField] private float shootingTime;
    [SerializeField] private float timeBetweenTakeDamage;
    [SerializeField] private LayerMask whatIsSolid;
    
    private bool isHit = false;
    private bool canDamage = true;
    //private bool rechargeTimeLockedOut = true;
    //private float timer = 0f;

    private bool canShoot = false;

    private EnemyBossShooting bossStats;

    void Start()
    {
        bossStats = transform.parent.parent.GetComponent<EnemyBossShooting>();
    }

    void FixedUpdate()
    {
        if(canShoot)
        {
            Vector2 startPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y);
            Vector2 endPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y - maxDistance);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsSolid);

            isHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                
                if (hits[i].collider != null) 
                {
                    if (hits[i].collider.CompareTag("Player") && canDamage) {
                        hits[i].collider.GetComponent<Player>().TakeDamage(bossStats.laserDamage); 
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));  
                    }
                    if (hits[i].collider.CompareTag("Shield") && canDamage) {
                        hits[i].collider.GetComponent<Shield>().TakeDamage(bossStats.laserDamage);
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage)); 
                    }
                    
                    laserRender.size = new Vector2(laserRender.size.x, laserRender.transform.position.y - hits[i].point.y);
                    isHit = true;
                    break;
                }
            }
            if(!isHit)
            {
               laserRender.size = new Vector2(laserRender.size.x, maxDistance); 
            }
        }
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        canDamage = false;
        yield return new WaitForSeconds(interval);
        canDamage = true;
    }

    public void StartAttack()
    {
        laserRender.gameObject.SetActive(true);
        laserRender.size = new Vector2(laserRender.size.x, 0f); 
        canShoot = true;
        canDamage = true;
    }

    public void StopAttack()
    {
        laserRender.gameObject.SetActive(false);
        canShoot = false;
        
    }
}
