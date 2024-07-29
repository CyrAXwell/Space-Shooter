using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private float numbersOfShots;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeTimeDelta;

    private bool rechargeTimeLockedOut = true;
    private float timer = 0f;
    private float shotCount = 0f;
    
    
    public Transform firePoint;
    public GameObject bulletPrefab;

    private Rigidbody2D rb;

    private Enemy enemyStats;
    private Bullet bulletStats;


    public Animator[] animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
        animator = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        if( rechargeTimeLockedOut == false)
        {
            Shoot();
            if(numbersOfShots == 1)
            {
                timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
                rechargeTimeLockedOut = true;
            } else
            {
                shotCount ++;
                timer = timeBetweenShots;
                if(shotCount == numbersOfShots)
                {
                    shotCount = 0;
                    timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
                }
                rechargeTimeLockedOut = true;
            }

            
        }


    }

    void FixedUpdate()
    {
        if ( rechargeTimeLockedOut == true )
        {
            timer -= Time.fixedDeltaTime;
        
            if ( timer <= 0 )
            {
                timer = rechargeTime + Random.Range(0f, rechargeTimeDelta);
                rechargeTimeLockedOut = false;
                
            }
        } 

        
    }

    // void GetTarget()
    // {
    //     target = GameObject.FindGameObjectWithTag("Player");
    // }

    void Shoot()
    {

        animator[1].SetTrigger("Shoot");
        enemyStats = GetComponent<Enemy>();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletStats = bullet.GetComponent<Bullet>();
        bulletStats.damage = enemyStats.damage;


    }
}
