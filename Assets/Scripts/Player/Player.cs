using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IUpgradeable
{
    public event Action OnStatsChange;
    public event Action OnHealthChange;
    public event Action OnXPChange;
    public event Action OnLevelUp;
    
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private UpgradeSO[] upgrades;

    private ISkillDisplayable[] _skills;
    private Ship1Stats ShipStats;
    private int _level = 1;
    private int _exp = 0;
    private int _maxExp = 1;

    private int _health;
    private int _maxHealth;
    private int _def;
    private int _damage;
    private int _critDamage;
    private int _critChance;

    public int _activeMaxHP;
    public int _activeHP;
    public int _activeATK;
    public int _activeDEF;
    public int _activeCRITDMG;
    public int _activeCRITRate;

    public void Initialize()
    {
        _skills = GetComponents<ISkillDisplayable>();
    }
    
    void Start()
    {
        ShipStats = GetComponent<Ship1Stats>();

        // _maxHealth = ShipStats.basemMaxHp;
        // _health = _maxHealth;
        // _def = ShipStats.baseDef;
        // _damage = ShipStats.baseDamage;
        // _critChance = ShipStats.baseCritChance;
        // _critDamage = ShipStats.baseCritDamage;

        _maxHealth = playerSO.Health;
        _health = _maxHealth;
        _def = playerSO.Defense;
        _damage = playerSO.Damage;
        _critChance = playerSO.CritDamage;
        _critDamage = playerSO.CritChance;

        _activeHP = _health;
        _activeMaxHP = _maxHealth;
        _activeATK = _damage;
        _activeDEF = _def;
        _activeCRITDMG = _critDamage;
        _activeCRITRate = _critChance;

        OnHealthChange?.Invoke();
        OnXPChange?.Invoke();
        OnStatsChange?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        hitSound.Play();

        _activeHP -= damage <= _activeDEF ? 1 : damage - _activeDEF;
        
        if(_activeHP <= _health)
            _health = _activeHP;

        OnHealthChange?.Invoke();

        if(_activeHP <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        GameObject.Find("GameManager").GetComponent<GameOverScreen>().GameOver();
    }

    public void UpdateGemStats(int hpGem, int atkGem, int defGem, int critdmgGem, int critrateGem)
    {
        _activeMaxHP = _maxHealth + hpGem;
        _activeHP = _activeMaxHP;
        _activeATK = _damage + atkGem;
        _activeDEF = _def + defGem;
        _activeCRITDMG = _critDamage + critdmgGem;
        _activeCRITRate = _critChance + critrateGem;

        OnHealthChange?.Invoke();
        OnStatsChange?.Invoke();
    }

    public int GetXP() => _exp;
    public int GetTargetXP() => _maxExp;
    public int GetLevel() => _level;
    public int GetActiveHp() => _activeHP;
    public int GetActiveMaxHp() => _activeMaxHP;
    public int GetActiveATK() => _activeATK;
    public int GetActiveDEF() => _activeDEF;
    public int GetActiveCRITDMG() => _activeCRITDMG;
    public int GetActiveCRITRate() => _activeCRITRate;
    public ISkillDisplayable[] GetSkills() => _skills;
    public UpgradeSO[] GetUpgrades() => upgrades;
    public Sprite GetUpgradesPanelSprite() => playerSO.UpgradesPanelSprite;

    public void SetXP(int addXP)
    {
        _exp += addXP;
        if(_exp >= _maxExp)
            lvlUP();

        OnXPChange?.Invoke();
    }

    private int GetTargetExperience(int level)
    {
        return (_level + 3) * (_level + 3);
    }

    public void lvlUP()
    {
        _level ++;
        if(_exp > _maxExp)
        {
            _exp -=  _maxExp;
            _maxExp = GetTargetExperience(_level);
            SetXP(0);
        } 
        else
        {
            _exp = 0;
            _maxExp = GetTargetExperience(_level);
        }
        OnXPChange?.Invoke();
        OnLevelUp?.Invoke();
    }

    public void Heal(int healHP)
    {
        int tempHP = _activeHP + healHP;
        _activeHP = tempHP > _activeMaxHP ? _activeMaxHP : tempHP;

        tempHP = _health + healHP;
        _health = tempHP > _maxHealth ? _maxHealth : tempHP;

        OnHealthChange?.Invoke();
    }

    public void FullHeal()
    {
        _activeHP = _activeMaxHP;
        OnHealthChange?.Invoke();
    }

    public void UpgradeHP(int addHp)
    {
        _maxHealth += addHp;
        _health += addHp;
        _activeHP += addHp;
        _activeMaxHP += addHp;

        OnHealthChange?.Invoke();
        OnStatsChange?.Invoke();
    }

    public void UpgradeArmor(int addArmor)
    {
        _def += addArmor;
        _activeDEF += addArmor;
        OnStatsChange?.Invoke();
    }

    public void UpgradeDamage(int addDamage)
    {
        _damage += addDamage;
        _activeATK += addDamage;
        OnStatsChange?.Invoke();
    }

    public void UpgradeCritDamage(int addCritDamage)
    {
        _critDamage += addCritDamage;
        _activeCRITDMG += addCritDamage;
        OnStatsChange?.Invoke();
    }
    
    public void UpgradeCritRate(int addCritRate)
    {
        _critChance += addCritRate;
        _activeCRITRate += addCritRate;
        OnStatsChange?.Invoke();
    }
}