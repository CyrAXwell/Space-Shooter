using System;
using System.Collections;
using UnityEngine;

public class LaserSkill : MonoBehaviour, ISkillDisplayable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private SpriteRenderer laserSprite; 
    [SerializeField] private int skillBonusDamage;
    [SerializeField] private float maxLaserDistance;
    [SerializeField] private float duration;
    [SerializeField] private float timeBetweenDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float cooldown;
    [SerializeField] AudioSource shootSound;

    private bool isHit = false;
    private bool canDamage = true;
    private bool canShoot = false;
    private float _cooldownTimer;
    private bool isTimerLocked;
    private bool isSkillActive = false;
    private Player player;
    private ShootingShip3 shooting;

    void Start()
    {
        _cooldownTimer = cooldown;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shooting = GetComponent<ShootingShip3>();

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer); 
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(isTimerLocked && !isSkillActive)
            {
                isSkillActive = true;
                canShoot = true;
                StartCoroutine(LaserSkillEnd(duration));
                laserSprite.gameObject.SetActive(true);
                laserSprite.size = new Vector2(laserSprite.size.x, 0); 
                shooting.laserActive = true;

                OnUseSkill?.Invoke();
            }
        }
    }

    void FixedUpdate()
    {
        if(isTimerLocked == false && StateNameController.startTimers)
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

        if(canShoot)
        {
            UseSkill();
        }
        
    }

    void UseSkill()
    {
        Vector2 startPoint = new Vector2(laserSprite.transform.position.x, laserSprite.transform.position.y);
        Vector2 endPoint = new Vector2(laserSprite.transform.position.x, laserSprite.transform.position.y + maxLaserDistance);
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsEnemy);
        Debug.Log((endPoint - startPoint).normalized);
        Debug.Log((endPoint - startPoint).magnitude);

        isHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            
            if (hits[i].collider != null) 
            {
                if (hits[i].collider.CompareTag("Enemy") && canDamage) {
                    hits[i].collider.GetComponent<Enemy>().TakeDamage(player._activeATK + skillBonusDamage, player._activeCRITRate, player._activeCRITDMG);
                    StartCoroutine(CanLaserDamage(timeBetweenDamage)); 
                }
                if (hits[i].collider.CompareTag("EnemyShield") && canDamage) {
                    hits[i].collider.GetComponent<EnemyShieldStats>().TakeDamage(player._activeATK + skillBonusDamage);
                    StartCoroutine(CanLaserDamage(timeBetweenDamage));
                }
                if (hits[i].collider.CompareTag("Boss") && canDamage) {
                    hits[i].collider.GetComponent<Boss>().TakeDamage(player._activeATK + skillBonusDamage, player._activeCRITRate, player._activeCRITDMG);
                    StartCoroutine(CanLaserDamage(timeBetweenDamage));
                }
                
                laserSprite.size = new Vector2(laserSprite.size.x, hits[i].point.y - laserSprite.transform.position.y);
                isHit = true;
                break;
            }
        }
        if(!isHit)
        {
            laserSprite.size = new Vector2(laserSprite.size.x, maxLaserDistance); 
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
        //StopShootSound();
        canShoot = false;
        laserSprite.gameObject.SetActive(false);
        isSkillActive = false;
        isTimerLocked = false;
        shooting.laserActive = false;
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
        timeBetweenDamage -= time;

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
