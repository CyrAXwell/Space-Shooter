using System;
using System.Collections;
using UnityEngine;

public class RegenerationSkill : MonoBehaviour, ISkillDisplayable, IUpgradeable
{
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    [SerializeField] private float cooldown;
    [SerializeField] private int regenerationValue;
    [SerializeField] private float healInterval;
    [SerializeField] private float duration;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private Sprite icon;

    private bool _isSkillActive;
    private float _cooldownTimer;
    private bool _isTimerLocked = false;
    private Player _player;

    private void Start()
    {
        _cooldownTimer = cooldown;

        _player = GetComponent<Player>();

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

        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(_isTimerLocked && !_isSkillActive)
            {
                UseSkill();
                OnUseSkill?.Invoke();
            }
        }
    }

    private void UseSkill()
    {
        _player.Heal(regenerationValue);
        _isSkillActive = true;
        StartCoroutine(Regen(healInterval));
        StartCoroutine(RegenEnd(duration));
    }

    private IEnumerator Regen(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(_isSkillActive)
        {
            _player.Heal(regenerationValue);
            StartCoroutine(Regen(healInterval));
        }
    }

    private IEnumerator RegenEnd(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(_isSkillActive)
            StopSkill();
    }

    public void ResetSkill()
    {
        if(!_isSkillActive)
        {
           OnResetSkill?.Invoke();
            UseSkill();
        }

        StopSkill();
    }

    private void StopSkill()
    {
        _isSkillActive = false;
        _isTimerLocked = false;
        _cooldownTimer = cooldown;
    }

    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetSkillIcon() => icon;

    public void UpgradeCooldown(float time)
    {
        cooldown -= time;
    }

    public void UpgradeHealing(int hp)
    {
        regenerationValue += hp;
    }

    public void UpgradeDurtion(float time)
    {
        duration += time;
    }
}
