using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserShooting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer laserRender; 
    [SerializeField] private float maxDistance;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeTimeDelta;
    [SerializeField] private float shootingTime;
    [SerializeField] private float timeBetweenTakeDamage;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask whatIsSolid;
    
    private bool isHit = false;
    private bool canDamage = true;
    private bool rechargeTimeLockedOut = true;
    private float timer = 0f;

    private Enemy enemyStats;

    void Start()
    {
        
        timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        enemyStats = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        if(rechargeTimeLockedOut)
        {
            timer -= Time.fixedDeltaTime;
            if ( timer <= 0 )
            {
                timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
                rechargeTimeLockedOut = false;
                laserRender.gameObject.SetActive(true);
                StartCoroutine(EndShooting(shootingTime));
            }
        }

        if(!rechargeTimeLockedOut){
            

            Vector2 startPoint = new Vector2(firePoint.position.x, firePoint.position.y);
            Vector2 endPoint = new Vector2(firePoint.position.x, firePoint.position.y - maxDistance);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsSolid);

            isHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                
                if (hits[i].collider != null) 
                {
                    if (hits[i].collider.CompareTag("Player") && canDamage) {
                        hits[i].collider.GetComponent<Player>().TakeDamage(enemyStats.damage); 
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));  
                    }
                    if (hits[i].collider.CompareTag("Shield") && canDamage) {
                        hits[i].collider.GetComponent<Shield>().TakeDamage(enemyStats.damage);
                        StartCoroutine(CanLaserDamage(timeBetweenTakeDamage)); 
                    }
                    
                    laserRender.size = new Vector2(laserRender.size.x, firePoint.position.y - hits[i].point.y);
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
    

    private IEnumerator EndShooting(float interval)
    {
        yield return new WaitForSeconds(interval);
        timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        rechargeTimeLockedOut = true;
        laserRender.gameObject.SetActive(false);
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        canDamage = false;
        yield return new WaitForSeconds(interval);
        canDamage = true;
    }



}
