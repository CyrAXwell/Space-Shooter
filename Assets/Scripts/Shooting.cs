using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePointOne;
    public Transform firePointTwo;
    public GameObject bulletPrefab;

    
    public float timer_cooldown;
    private float timer = 0f;
    private bool timer_locked_out = false;
    
    private Player playerStats;
    private Bullet bulletStats;
    
    //private Animator anim;

    public Animator[] animator;

    //[HideInInspector] public bool isRapidFire = false;
    [HideInInspector] public int rapidFireDamage = 0;

    [SerializeField] AudioSource shootSound;

    void Start()
    {
        timer = timer_cooldown;
        //anim = GetComponent<Animator>();
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
        // Fire1
        if(Input.GetButton("Jump"))
        {
            Shoot();
            //anim.SetTrigger("attack");
        }
        
    }
    void Shoot()
    {
        if ( timer_locked_out == false )
        {
            PlayShootSound();
            timer_locked_out = true;
            playerStats = GetComponent<Player>();

            //Debug.Log(animator[1]);
            animator[2].SetTrigger("Shoot");
            
            //GameObject shootEffect = Instantiate(shootEffectPrefab, transform.position, transform.rotation);
            //Destroy(shootEffect, 0.05f);

            GameObject bulletOne = Instantiate(bulletPrefab, firePointOne.position, transform.rotation);
            bulletStats = bulletOne.GetComponent<Bullet>();
            bulletStats.damage = playerStats.activeATK + rapidFireDamage;
            bulletStats.critChance = playerStats.activeCRITRate;
            bulletStats.critDamage = playerStats.activeCRITDMG;
            //Debug.Log("damage - " + bulletStats.damage);

            GameObject bulletTwo = Instantiate(bulletPrefab, firePointTwo.position, transform.rotation);
            bulletStats = bulletTwo.GetComponent<Bullet>();
            bulletStats.damage = playerStats.activeATK + rapidFireDamage;
            bulletStats.critChance = playerStats.activeCRITRate;
            bulletStats.critDamage = playerStats.activeCRITDMG;

            //Instantiate(bulletPrefab, firePoint1.position, transform.rotation);


            // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            // rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

            // GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
            // Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
            // rb1.AddForce(firePoint1.up * bulletForce, ForceMode2D.Impulse);
        }
        
    }

    void PlayShootSound()
    {
        shootSound.Play();
    }
}
