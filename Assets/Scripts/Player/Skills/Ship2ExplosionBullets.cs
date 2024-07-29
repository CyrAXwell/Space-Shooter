using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship2ExplosionBullets : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private int additionalDamage;
    public GameObject bulletPrefab;
    private GameObject skillDisplay;
    private GameObject displayTimer;

    private bool skillActive = false;
    private float skillTimer = 0f;
    [NonSerialized] public bool skillTimerLocked = false;
    public float skillCoolDown;
    [SerializeField] private float shootingTime;
    private bool canShoot = false;
    private bool reload = false;
    [SerializeField] float timeBetweenShot;
    private ExplosionBullet bulletStats;
    private Player playerStats;
    private ShootingShip2 shooting;
    

    public string skillColor;
    [SerializeField] AudioSource shootSound;

    void Start()
    {
        skillTimer = skillCoolDown;
        playerStats = GetComponent<Player>();
        skillDisplay = GameObject.Find("Skill 2");
        displayTimer = skillDisplay.transform.GetChild(2).gameObject;
        skillDisplay.GetComponent<SkillDisplay>().SkillCharge();
        shooting = GetComponent<ShootingShip2>();
        displayTimer.GetComponent<SkillTimer>().DisplayTime(skillTimer);
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(skillTimerLocked && !skillActive)
            {
                canShoot = true;
                skillActive = true;
                shooting.skillActive = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillActive();
                StartCoroutine(ShootSkillEnd(shootingTime));
            }
        
        }
        
    }
    void FixedUpdate()
    {
        if(skillTimerLocked == false  && StateNameController.startTimers)
        {
            skillTimer -= Time.fixedDeltaTime;
            displayTimer.GetComponent<SkillTimer>().DisplayTime(skillTimer);
            if(skillTimer <= 0)
            {
                skillTimer = 0;
                skillTimerLocked = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillReady();
                
            }
        }

        if(canShoot && !reload)
        {
            PlayShootSound();
            Shoot();
            reload = true;
            StartCoroutine(ReloadShot(timeBetweenShot));
        }
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletStats = bullet.GetComponent<ExplosionBullet>();
        bulletStats.damage = playerStats._activeATK + additionalDamage;
        bulletStats.critChance = playerStats._activeCRITRate;
        bulletStats.critDamage = playerStats._activeCRITDMG;
    }

    private IEnumerator ReloadShot(float interval)
    {
        yield return new WaitForSeconds(interval);
        reload = false;
    }

    private IEnumerator ShootSkillEnd(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(skillActive)
        {
            StopSkill();
        }
        
    }

    public void ResetSkill()
    {
        if(skillActive)
        {
            StopSkill();
        }else{
            skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            StopSkill();
        }
    }

    void StopSkill()
    {
        canShoot = false;
        skillActive = false;
        skillTimerLocked = false;
        shooting.skillActive = false;
        skillTimer = skillCoolDown;
    }

    public void ActionTimeUpgarde(float time)
    {
        shootingTime += time;

    }

    public void CooldownUpgarde(float time)
    {
        skillCoolDown -= time;

    }

    public void DamageUpgarde(int addDamage)
    {
        additionalDamage += addDamage;

    }

    public void RateUpgarde(float time)
    {
        timeBetweenShot -= time;

    }

    void PlayShootSound()
    {
        shootSound.Play();
    }
}
