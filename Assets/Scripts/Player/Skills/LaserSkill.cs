using System;
using System.Collections;
using UnityEngine;

public class LaserSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private SpriteRenderer laserSprite; 
    [SerializeField] private int skillBonusDamage;
    [SerializeField] private float maxLaserDistance;
    [SerializeField] private float duration;
    [SerializeField] private float timeBetweenDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float cooldown;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private bool _canDamage = true;
    private bool _canShoot = false;
    private float _cooldownTimer;
    private bool _isTimerLocked;
    private bool _isSkillActive = false;
    private Shooting _shooting;
    private Player _player;

    private void Start()
    {
        _cooldownTimer = cooldown;
        
        _player = GetComponent<Player>();
        _shooting = GetComponent<Shooting>();

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer); 
    }

    private void Update()
    {
        if(!_isTimerLocked && StateNameController.startTimers)
        {
            _cooldownTimer -= Time.deltaTime;
            OnTimerUpdate?.Invoke(_cooldownTimer);
            if(_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
                _isTimerLocked = true;
                OnSkillCooldown?.Invoke();  
            }
        }

        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(_isTimerLocked && !_isSkillActive)
            {
                _isSkillActive = true;
                _canShoot = true;
                StartCoroutine(LaserSkillEnd(duration));
                laserSprite.gameObject.SetActive(true);
                laserSprite.size = new Vector2(laserSprite.size.x, 0); 
                _shooting.StopShooting();

                OnUseSkill?.Invoke();
            }
        }

        if(_canShoot)
            UseSkill();
    }

    private void UseSkill()
    {
        Vector2 startPoint = new Vector2(laserSprite.transform.position.x, laserSprite.transform.position.y);
        Vector2 endPoint = new Vector2(laserSprite.transform.position.x, laserSprite.transform.position.y + maxLaserDistance);
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, (endPoint - startPoint).normalized, (endPoint - startPoint).magnitude, whatIsEnemy);
        Debug.Log((endPoint - startPoint).normalized);
        Debug.Log((endPoint - startPoint).magnitude);

        bool isHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            
            if (hits[i].collider != null && _canDamage) 
            {
                switch (hits[i].collider.tag)
                {
                    case "Enemy": 
                        hits[i].collider.GetComponent<Enemy>().TakeDamage(_player.GetActiveATK() + skillBonusDamage, _player.GetActiveCRITRate(), _player.GetActiveCRITDMG());
                        StartCoroutine(CanLaserDamage(timeBetweenDamage)); 
                        break;
                    case "EnemyShield": 
                        hits[i].collider.GetComponent<EnemyShieldStats>().TakeDamage(_player.GetActiveATK() + skillBonusDamage);
                        StartCoroutine(CanLaserDamage(timeBetweenDamage)); 
                        break;
                    case "Boss": 
                        hits[i].collider.GetComponent<Boss>().TakeDamage(_player.GetActiveATK() + skillBonusDamage, _player.GetActiveCRITRate(), _player.GetActiveCRITDMG());
                        StartCoroutine(CanLaserDamage(timeBetweenDamage)); 
                        break;
                }
                
                laserSprite.size = new Vector2(laserSprite.size.x, hits[i].point.y - laserSprite.transform.position.y);
                isHit = true;
                break;
            }
        }
        
        if(!isHit)
            laserSprite.size = new Vector2(laserSprite.size.x, maxLaserDistance); 
    }

    private IEnumerator CanLaserDamage(float interval)
    {
        _canDamage = false;
        yield return new WaitForSeconds(interval);
        _canDamage = true;
    }

    private IEnumerator LaserSkillEnd(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(_isSkillActive)
            StopSkill();
    }

    public void ResetSkill()
    {
        if(!_isSkillActive)
            OnResetSkill?.Invoke();

        StopSkill();
    }

    private void StopSkill()
    {
        _canShoot = false;
        laserSprite.gameObject.SetActive(false);
        _isSkillActive = false;
        _isTimerLocked = false;
        _shooting.ResumeShooting();
        _cooldownTimer = cooldown;
    }

    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;

    public void UpgradeDuration(float time)
    {
        duration += time;
    }

    public void UpgradeCooldown(float time)
    {
        cooldown -= time;
    }

    public void UpgradeDamage(int addDamage)
    {
        skillBonusDamage += addDamage;
    }

    public void UpgradeFireRate(float time)
    {
        timeBetweenDamage -= time;
    }
}
