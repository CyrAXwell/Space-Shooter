using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingShip2 : MonoBehaviour
{
    public Transform firePointOne;
    public Transform firePointTwo;
    public GameObject bulletPrefab;

    public float timer_cooldown;
    private float timer = 0f;
    private bool timer_locked_out = false;
    
    private Player playerStats;
    private Bullet bulletStats;
    //private ExplosionBullet bulletStats;

    public Animator[] animator;

    public bool skillActive = false;
    
    [SerializeField] AudioSource shootSound;

    void Start()
    {
        timer = timer_cooldown;
        animator = GetComponentsInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if ( timer_locked_out == true )
        {
            timer -= Time.fixedDeltaTime;
        
            if ( timer <= 0 )
            {
                timer = timer_cooldown;
                timer_locked_out = false;
                
            }
        }

        if(Input.GetButton("Jump"))
        {
            Shoot();
        }
        
    }

    void Shoot()
    {
        if ( timer_locked_out == false &&  !skillActive)
        {
            PlayShootSound();
            timer_locked_out = true;
            playerStats = GetComponent<Player>();

            animator[1].SetTrigger("Shoot");
  
            GameObject bulletOne = Instantiate(bulletPrefab, firePointOne.position, transform.rotation);
            bulletStats = bulletOne.GetComponent<Bullet>();
            bulletStats.damage = playerStats._activeATK;
            bulletStats.critChance = playerStats._activeCRITRate;
            bulletStats.critDamage = playerStats._activeCRITDMG;

            GameObject bulletTwo = Instantiate(bulletPrefab, firePointTwo.position, transform.rotation);
            bulletStats = bulletTwo.GetComponent<Bullet>();
            bulletStats.damage = playerStats._activeATK;
            bulletStats.critChance = playerStats._activeCRITRate;
            bulletStats.critDamage = playerStats._activeCRITDMG;
        }
        
    }

    void PlayShootSound()
    {
        shootSound.Play();
    }
}
