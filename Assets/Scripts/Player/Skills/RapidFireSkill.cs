using System;
using System.Collections;
using UnityEngine;

public class RapidFireSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private float durartion;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private int skillBonusDamage;
    [SerializeField] private UpgradeSO[] upgrades;

    private bool isTimerLocked = false;
    private bool isSkillActive = false;

    private float _cooldownTimer;
    
    void Start()
    {
        _cooldownTimer = cooldown;

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer); 
    }

    void FixedUpdate()
    {
        if(isTimerLocked == false && StateNameController.startTimers)
        {
            _cooldownTimer -= Time.fixedDeltaTime;
            OnTimerUpdate?.Invoke(_cooldownTimer);

            if(_cooldownTimer <= 0)
            {
                isTimerLocked = true;
                OnSkillCooldown?.Invoke();
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(isTimerLocked & !isSkillActive)
            {
                UseSkill();
                OnUseSkill?.Invoke();
            }
        }
    }

    void UseSkill()
    {
        isSkillActive = true;
        _cooldownTimer = GetComponent<Shooting>().timer_cooldown;
        GetComponent<Shooting>().timer_cooldown = timeBetweenShot;
        GetComponent<Shooting>().rapidFireDamage = skillBonusDamage;
        StartCoroutine(EndRapidFireSkill(durartion));
    }

    public void ResetRapidFireSkill()
    {
        if(isSkillActive)
        {
            StopRapidFireSkill();
        }
        else
        {
            OnUseSkill?.Invoke();
            UseSkill();
            StopRapidFireSkill();
        }
    }

    private IEnumerator EndRapidFireSkill(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(isSkillActive)
            StopRapidFireSkill();
    }

    public void StopRapidFireSkill()
    {
        GetComponent<Shooting>().timer_cooldown = _cooldownTimer;
        GetComponent<Shooting>().rapidFireDamage = 0;

        isTimerLocked = false;
        isSkillActive = false;
        _cooldownTimer = cooldown;
    }

    public UpgradeSO[] GetUpgrades() => upgrades;


    public void ActionTimeUpgarde(float time)
    {
        durartion += time;

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
        Debug.Log(timeBetweenShot);

    }
}
