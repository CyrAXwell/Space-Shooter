using System;
using UnityEngine;

public class ShieldSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private int health;
    [SerializeField] private int heal;
    [SerializeField] private string iconColor;
    [SerializeField] private Shield shield;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private float _cooldownTimer;
    private bool _isTimerLocked = false;

    void Start()
    {
        _cooldownTimer = cooldown;

        shield.Initialize(health, this);

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer);  
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(_isTimerLocked)
            {
                UseSkill();
                OnUseSkill?.Invoke();
            }
        }
    }

    void FixedUpdate()
    {
        if(_isTimerLocked == false  && StateNameController.startTimers)
        {
            _cooldownTimer -= Time.fixedDeltaTime;
            OnTimerUpdate?.Invoke(_cooldownTimer); 

            if(_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
                _isTimerLocked = true;
                OnSkillCooldown?.Invoke(); 
            }
        }
        
    }

    public int GetShieldHealth() => health;
    public int GetShieldHeal() => heal;
    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;
    
    private void UseSkill()
    {
        shield.gameObject.SetActive(true);
        shield.Initialize(health, this);

        _cooldownTimer = cooldown;
    }

    public void OnShieldDestroy()
    {
        shield.gameObject.SetActive(false);
        _isTimerLocked = false;
        gameObject.GetComponent<Player>().Heal(heal);
        OnResetSkill?.Invoke();
    }

    public void ResetShieldSkill()
    {
        _cooldownTimer = cooldown;
        shield.DestroyShield();
        OnResetSkill?.Invoke();
    }

    public void UpgradeCooldown(float cooldown)
    {
        this.cooldown -= cooldown;
    }

    public void UpgradeHealing(int hp)
    {
        heal += hp;
    }

    public void UpgradeHealth(int hp)
    {
        health += hp;
    }

}
