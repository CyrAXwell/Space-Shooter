using System;
using System.Collections;
using UnityEngine;

public class RapidFireSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private float durartion;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private int skillBonusDamage;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private bool _isTimerLocked = false;
    private bool _isSkillActive = false;
    private float _cooldownTimer;
    private Shooting _shooting;
    
    private void Start()
    {
        _cooldownTimer = cooldown;
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
                _isTimerLocked = true;
                OnSkillCooldown?.Invoke();
            }
        }

        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(_isTimerLocked & !_isSkillActive)
            {
                UseSkill();
                OnUseSkill?.Invoke();
            }
        }
    }

    private void UseSkill()
    {
        _isSkillActive = true;
        _cooldownTimer = _shooting.GetCooldown();
        _shooting.UpdateCooldown(timeBetweenShot);
        _shooting.UpdateSkillBonusDamage(skillBonusDamage);
        StartCoroutine(EndRapidFireSkill(durartion));
    }

    public void ResetSkill()
    {
        if(_isSkillActive)
        {
            StopRapidFireSkill();
        }
        else
        {
            UseSkill();
            StopRapidFireSkill();
            OnResetSkill?.Invoke();
        }
    }

    private IEnumerator EndRapidFireSkill(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(_isSkillActive)
            StopRapidFireSkill();
    }

    public void StopRapidFireSkill()
    {
        _shooting.UpdateCooldown(_cooldownTimer);
        _shooting.UpdateSkillBonusDamage(0);

        _isTimerLocked = false;
        _isSkillActive = false;
        _cooldownTimer = cooldown;
    }

    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;

    public void UpgradeDuration(float time)
    {
        durartion += time;
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
