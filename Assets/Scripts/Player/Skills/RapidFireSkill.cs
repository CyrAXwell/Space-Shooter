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

    private bool isTimerLocked = false;
    private bool isSkillActive = false;

    private float _cooldownTimer;
    private Shooting _shooting;
    
    void Start()
    {
        _cooldownTimer = cooldown;
        _shooting = GetComponent<Shooting>();

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
        _cooldownTimer = _shooting.GetCooldown();
        _shooting.UpdateCooldown(timeBetweenShot);
        _shooting.UpdateSkillBonusDamage(skillBonusDamage);
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
            UseSkill();
            StopRapidFireSkill();
            OnResetSkill?.Invoke();
            Debug.Log("Reset");
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
        _shooting.UpdateCooldown(_cooldownTimer);
        _shooting.UpdateSkillBonusDamage(0);

        isTimerLocked = false;
        isSkillActive = false;
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
        Debug.Log(timeBetweenShot);

    }
}
