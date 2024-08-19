using System;
using System.Collections;
using UnityEngine;

public class ExplosionBulletsSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private Transform firePoint;
    [SerializeField] private ExplosionBullet bulletPrefab;
    [SerializeField] private int skillBonusDamage;
    [SerializeField] float timeBetweenShot;
    [SerializeField] private float cooldown;
    [SerializeField] private float duration;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private bool _isSkillActive = false;
    private float _cooldownTimer;
    private bool _isTimerLocked;
    private bool _canShoot = false;
    private bool _reload = false;
    private Player _playerStats;
    private Shooting _shooting;
    private ObjectPoolManager _objectPool;
    private AudioManager _audioManager;

    public void Initialize(ObjectPoolManager objectPool)
    {
        _objectPool = objectPool;
    }

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _cooldownTimer = cooldown;
        _playerStats = GetComponent<Player>();
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
                _canShoot = true;
                _isSkillActive = true;
                _shooting.StopShooting();
                OnUseSkill?.Invoke();
                StartCoroutine(ShootSkillEnd(duration));
            }
        } 

        if(_canShoot && !_reload)
        {
            _audioManager.PlaySFX(_audioManager.Shoot, 0.2f);
            Shoot();
            _reload = true;
            StartCoroutine(ReloadShot(timeBetweenShot));
        }
    }

    private void Shoot()
    {
        ExplosionBullet bullet = _objectPool.GetObject(bulletPrefab).GetComponent<ExplosionBullet>();
        bullet.gameObject.name = bulletPrefab.name.ToString();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Initialize(_objectPool, _playerStats.GetActiveATK() + skillBonusDamage, _playerStats.GetActiveCRITRate(), _playerStats.GetActiveCRITDMG());

        _objectPool.ReleaseObject(bullet, 2f);
    }

    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;

    private IEnumerator ReloadShot(float interval)
    {
        yield return new WaitForSeconds(interval);
        _reload = false;
    }

    private IEnumerator ShootSkillEnd(float interval)
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
        _isSkillActive = false;
        _isTimerLocked = false;
        _shooting.ResumeShooting();
        _cooldownTimer = cooldown;
    }

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
        timeBetweenShot -= time;
    }
}
