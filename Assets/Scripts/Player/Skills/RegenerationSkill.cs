using System;
using System.Collections;
using UnityEngine;

public class RegenerationSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private int regenerationValue;
    [SerializeField] private float healInterval;
    [SerializeField] private float duration;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private bool isSkillActive;
    private float _cooldownTimer;
    private bool isTimerLocked = false;
    private Player player;

    void Start()
    {
        _cooldownTimer = cooldown;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        OnStartWave?.Invoke();
        OnTimerUpdate?.Invoke(_cooldownTimer); 
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(isTimerLocked && !isSkillActive)
            {
                UseSkill();
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
    }

    void UseSkill()
    {
        player.Heal(regenerationValue);
        isSkillActive = true;
        StartCoroutine(Regen(healInterval));
        StartCoroutine(RegenEnd(duration));
    }

    private IEnumerator Regen(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(isSkillActive)
        {
            player.Heal(regenerationValue);
            StartCoroutine(Regen(healInterval));
        }
    }

    private IEnumerator RegenEnd(float interval)
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
            UseSkill();
            StopSkill();
        }
    }

    void StopSkill()
    {
        isSkillActive = false;
        isTimerLocked = false;
        _cooldownTimer = cooldown;
    }

    public UpgradeSO[] GetUpgrades() => upgrades;

    public Sprite GetSkillIcon() => icon;

    public void UpgradeCooldown(float time)
    {
        cooldown -= time;
        Debug.Log(cooldown);
    }

    public void UpgradeHealing(int hp)
    {
        regenerationValue += hp;
        Debug.Log(regenerationValue);
    }

    public void UpgradeDurtion(float time)
    {
        duration += time;
        Debug.Log(duration);
    }
}
