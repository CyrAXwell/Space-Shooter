using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnStatsChange;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioSource hitSound;

    private Ship1Stats ShipStats;
    private int _level = 1;
    private int _exp = 0;
    private int _maxExp = 10;

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

    private GameObject _uiHealthBar;
    private HealthBar _healthBar;
    private GameObject _shipParameters;
    private GameObject _uiXPBar;
    private XPBar _xPBar;
    private GameObject levelUpMenu;

    void Start()
    {
        ShipStats = GetComponent<Ship1Stats>();

        _maxHealth = ShipStats.basemMaxHp;
        _health = _maxHealth;
        _def = ShipStats.baseDef;
        _damage = ShipStats.baseDamage;
        _critChance = ShipStats.baseCritChance;
        _critDamage = ShipStats.baseCritDamage;

        _activeHP = _health;
        _activeMaxHP = _maxHealth;
        _activeATK = _damage;
        _activeDEF = _def;
        _activeCRITDMG = _critDamage;
        _activeCRITRate = _critChance;
        
        
        _uiHealthBar = GameObject.Find("HP");
        _uiXPBar = GameObject.Find("XP");
        levelUpMenu = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        
        _shipParameters = GameObject.Find("Ship parameters");
        _healthBar = _uiHealthBar.GetComponent<HealthBar>();
        _xPBar = _uiXPBar.GetComponent<XPBar>();

        _xPBar.SetXP(_exp, _maxExp);
        _xPBar.SetLvl(_level);
        _healthBar.SetHealth(_activeHP, _activeMaxHP);

        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
        OnStatsChange?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        hitSound.Play();

        _activeHP -= damage <= _activeDEF ? 1 : damage - _activeDEF;
        
        if(_activeHP <= _health)
            _health = _activeHP;

        _healthBar.SetHealth(_activeHP, _activeMaxHP);

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

        _healthBar.SetHealth(_activeHP, _activeMaxHP);

        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }

    public int GetActiveMaxHp() => _activeMaxHP;
    public int GetActiveATK() => _activeATK;
    public int GetActiveDEF() => _activeDEF;
    public int GetActiveCRITDMG() => _activeCRITDMG;
    public int GetActiveCRITRate() => _activeCRITRate;

    public void SetXP(int addXP)
    {
        _exp += addXP;
        if(_exp >= _maxExp)
            lvlUP();

        _xPBar.SetXP(_exp, _maxExp);
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
        _xPBar.SetXP(_exp, _maxExp);
        _xPBar.SetLvl(_level);
        
        levelUpMenu.GetComponent<LevelUpMenu>().OpenLevelUpMenu();
    }

    public void Heal(int healHP)
    {
        int tempHP = _activeHP + healHP;
        _activeHP = tempHP > _activeMaxHP ? _activeMaxHP : tempHP;

        tempHP = _health + healHP;
        _health = tempHP > _maxHealth ? _maxHealth : tempHP;

        _healthBar.SetHealth(_activeHP, _activeMaxHP);
    }

    public void FullHeal()
    {
        _activeHP = _activeMaxHP;
        _healthBar.SetHealth(_activeHP, _activeMaxHP);
    }

    public void UpgradeHP(int addHp)
    {
        _maxHealth += addHp;
        _health += addHp;
        _activeHP += addHp;
        _activeMaxHP += addHp;

        _healthBar.SetHealth(_activeHP, _activeMaxHP);
        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }

    public void UpgradeArmor(int addArmor)
    {
        _def += addArmor;
        _activeDEF += addArmor;
        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }

    public void UpgradeDamage(int addDamage)
    {
        _damage += addDamage;
        _activeATK += addDamage;
        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }

    public void UpgradeCritDamage(int addCritDamage)
    {
        _critDamage += addCritDamage;
        _activeCRITDMG += addCritDamage;
        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }
    
    public void UpgradeCritRate(int addCritRate)
    {
        _critChance += addCritRate;
        _activeCRITRate += addCritRate;
        _shipParameters.GetComponent<UIShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }
}