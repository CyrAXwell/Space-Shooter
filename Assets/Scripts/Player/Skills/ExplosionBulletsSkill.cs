using System;
using System.Collections;
using UnityEngine;

public class ExplosionBulletsSkill : MonoBehaviour, ISkillDisplayable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int skillBonusDamage;
    [SerializeField] float timeBetweenShot;
    [SerializeField] private float cooldown;
    [SerializeField] private float duration;
    [SerializeField] AudioSource shootSound;

    private bool isSkillActive = false;
    private float _cooldownTimer;
    private bool isTimerLocked;
    private bool canShoot = false;
    private bool reload = false;
    private ExplosionBullet bulletStats;
    private Player playerStats;
    private ShootingShip2 shooting;

    void Start()
    {
        _cooldownTimer = cooldown;
        playerStats = GetComponent<Player>();
        shooting = GetComponent<ShootingShip2>();

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer); 
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(isTimerLocked && !isSkillActive)
            {
                canShoot = true;
                isSkillActive = true;
                shooting.skillActive = true;
                OnUseSkill?.Invoke();
                StartCoroutine(ShootSkillEnd(duration));
            }
        } 
    }

    void FixedUpdate()
    {
        if(isTimerLocked == false  && StateNameController.startTimers)
        {
            _cooldownTimer -= Time.fixedDeltaTime;
            OnTimerUpdate?.Invoke(_cooldownTimer); 
            if(_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
                isTimerLocked = true;
                OnSkillCooldown?.Invoke();
                
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
        bulletStats.damage = playerStats._activeATK + skillBonusDamage;
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
        if(isSkillActive)
        {
            StopSkill();
        }
        
    }

    public void ResetSkill()
    {
        if(isSkillActive)
        {
            StopSkill();
        }else{
            OnUseSkill?.Invoke();
            StopSkill();
        }
    }

    void StopSkill()
    {
        canShoot = false;
        isSkillActive = false;
        isTimerLocked = false;
        shooting.skillActive = false;
        _cooldownTimer = cooldown;
    }

    public void ActionTimeUpgarde(float time)
    {
        duration += time;

    }

    public void CooldownUpgarde(float time)
    {
        cooldown -= time;

    }

    public void DamageUpgarde(int addDamage)
    {
        skillBonusDamage += addDamage;

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
