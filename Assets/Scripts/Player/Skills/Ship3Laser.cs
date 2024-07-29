using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship3Laser : MonoBehaviour
{
    [Header("Ship 1 - Laser Skill")]
    [SerializeField] private SpriteRenderer laserRender; 
    [SerializeField] private int additionalDamage;
    [SerializeField] private float maxDistance;
    //[SerializeField] private float rechargeTime;
    [SerializeField] private float shootingTime;
    [SerializeField] private float timeBetweenTakeDamage;
    //[SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask whatIsSolid;

    private bool isHit = false;
    private bool canDamage = true;
    //private bool rechargeTimeLockedOut = true;
    //private float timer = 0f;

    //private Enemy enemyStats;
    private bool canShoot = false;

    private float laserTimer = 0f;
    [NonSerialized] public bool laserTimerLocked = false;
    public float laserCoolDown;

    public string skillColor;

    private GameObject skillDisplay;
    private GameObject displayTimer;

    private bool laserActive = false;
    private Player player;

    private ShootingShip3 shooting;
    [SerializeField] AudioSource shootSound;

    void Start()
    {
        laserTimer = laserCoolDown;
        skillDisplay = GameObject.Find("Skill 2");
        displayTimer = skillDisplay.transform.GetChild(2).gameObject;
        skillDisplay.GetComponent<SkillDisplay>().SkillCharge();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shooting = GetComponent<ShootingShip3>();
        displayTimer.GetComponent<SkillTimer>().DisplayTime(laserTimer);
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(laserTimerLocked && !laserActive)
            {
                //LaserSkill();
                laserActive = true;
                canShoot = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillActive();
                StartCoroutine(LaserSkillEnd(shootingTime));
                laserRender.gameObject.SetActive(true);
                laserRender.size = new Vector2(laserRender.size.x, 0); 
                shooting.laserActive = true;
                //PlayShootSound();
            }
        
        }
        
    }

    void FixedUpdate()
    {
        if(laserTimerLocked == false && StateNameController.startTimers)
        {
            laserTimer -= Time.fixedDeltaTime;
            displayTimer.GetComponent<SkillTimer>().DisplayTime(laserTimer);
            if(laserTimer <= 0)
            {
                laserTimer = 0;
                laserTimerLocked = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillReady();
                
            }
        }

        if(canShoot)
        {
            LaserSkill();
        }
        
    }

    void LaserSkill()
    {
        Vector2 startPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y);
        Vector2 endPoint = new Vector2(laserRender.transform.position.x, laserRender.transform.position.y + maxDistance);
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsSolid);
        Debug.Log((endPoint - startPoint).normalized);
        Debug.Log((endPoint - startPoint).magnitude);

        isHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            
            if (hits[i].collider != null) 
            {
                if (hits[i].collider.CompareTag("Enemy") && canDamage) {
                    hits[i].collider.GetComponent<Enemy>().TakeDamage(player._activeATK + additionalDamage, player._activeCRITRate, player._activeCRITDMG);
                    StartCoroutine(CanLaserDamage(timeBetweenTakeDamage)); 
                }
                if (hits[i].collider.CompareTag("EnemyShield") && canDamage) {
                    hits[i].collider.GetComponent<EnemyShieldStats>().TakeDamage(player._activeATK + additionalDamage);
                    StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));
                }
                if (hits[i].collider.CompareTag("Boss") && canDamage) {
                    hits[i].collider.GetComponent<Boss>().TakeDamage(player._activeATK + additionalDamage, player._activeCRITRate, player._activeCRITDMG);
                    StartCoroutine(CanLaserDamage(timeBetweenTakeDamage));
                }
                
                laserRender.size = new Vector2(laserRender.size.x, hits[i].point.y - laserRender.transform.position.y);
                isHit = true;
                break;
            }
        }
        if(!isHit)
        {
            laserRender.size = new Vector2(laserRender.size.x, maxDistance); 
        }
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        canDamage = false;
        yield return new WaitForSeconds(interval);
        canDamage = true;
    }

    private IEnumerator LaserSkillEnd(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(laserActive)
        {
            StopSkill();
        }
    }

    public void ResetSkill()
    {
        if(laserActive)
        {
            StopSkill();
        }else{
            skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            StopSkill();
        }
    }

    void StopSkill()
    {
        //StopShootSound();
        canShoot = false;
        laserRender.gameObject.SetActive(false);
        laserActive = false;
        laserTimerLocked = false;
        shooting.laserActive = false;
        laserTimer = laserCoolDown;
    }

    public void ActionTimeUpgarde(float time)
    {
        shootingTime += time;

    }

    public void CooldownUpgarde(float time)
    {
        laserCoolDown -= time;

    }

    public void DamageUpgarde(int addDamage)
    {
        additionalDamage += addDamage;

    }

    public void RateUpgarde(float time)
    {
        timeBetweenTakeDamage -= time;

    }

    void PlayShootSound()
    {
        shootSound.Play();
    }

    void StopShootSound()
    {
        shootSound.Stop();
    }
}
