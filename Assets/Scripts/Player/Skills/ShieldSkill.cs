using System;
using UnityEngine;

public class ShieldSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private int health;
    [SerializeField] private int heal;
    [SerializeField] private string iconColor;
    [SerializeField] private Shield shield;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private float _cooldownTimer = 0f;

    [NonSerialized] public bool isTimerLocked = false;
    [NonSerialized] public bool isSkillActive = false;  

    void Start()
    {
        _cooldownTimer = cooldown;

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer);  
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(isTimerLocked)
            {
                UseSkill();
                OnUseSkill?.Invoke();
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
        
    }

    public int GetShieldHealth() => health;
    public int GetShieldHeal() => heal;
    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;
    

    void UseSkill()
    {
        shield.gameObject.SetActive(true);
        
        _cooldownTimer = cooldown;
    }

    public void ResetShieldSkill()
    {
        _cooldownTimer = cooldown;
        shield.DestroyShield();
        OnUseSkill?.Invoke();
    }

    public void UpgradeCooldown(float cooldown)
    {
        this.cooldown -= cooldown;
        Debug.Log(this.cooldown);
    }

    public void UpgradeHealing(int hp)
    {
        heal += hp;
        Debug.Log(heal);
    }

    public void UpgradeHealth(int hp)
    {
        health += hp;
        Debug.Log(health);
    }

}
