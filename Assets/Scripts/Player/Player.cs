using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    private int _activeMaxHP;
    private int _activeHP;
    private int _activeATK;
    private int _activeDEF;
    private int _activeCRITDMG;
    private int _activeCRITRate;

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

        _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
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

        _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
    }

    public void GetXP(int addXP)
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
            GetXP(0);
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

    public void MaxHeal()
    {
        _activeHP = _activeMaxHP;
        _healthBar.SetHealth(_activeHP, _activeMaxHP);
    }

    public void ChangeHp(int addHp)
        {
            _maxHealth += addHp;
            _health += addHp;
            _activeHP += addHp;
            _activeMaxHP += addHp;

            //HealthBar healthBar = uiHealthBar.GetComponent<HealthBar>();
            //healthBar.SetMaxHealt(maxHealth);
            _healthBar.SetHealth(_activeHP, _activeMaxHP);
            _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
        }

    public void Heal(int healHP)
        {
            if(_activeHP + healHP >= _activeMaxHP)
            {
                _activeHP = _activeMaxHP;
            }else
            {
                _activeHP += healHP;
            }

            if(_health + healHP >= _maxHealth)
            {
                _health = _maxHealth;
            }else
            {
                _health += healHP;
            }

            //HealthBar healthBar = uiHealthBar.GetComponent<HealthBar>();
            //healthBar.SetMaxHealt(maxHealth);
            _healthBar.SetHealth(_activeHP, _activeMaxHP);
        }

    public void UpgradeArmor(int addArmor)
        {
            _def += addArmor;
            _activeDEF += addArmor;
            _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);

        }

    public void UpgradeDamage(int addDamage)
        {
            _damage += addDamage;
            _activeATK += addDamage;
            _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
        }

    public void UpgradeCritDamage(int addCritDamage)
        {
            _critDamage += addCritDamage;
            _activeCRITDMG += addCritDamage;
            _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
        }
    
    public void UpgradeCritRate(int addCritRate)
        {
            _critChance += addCritRate;
            _activeCRITRate += addCritRate;
            _shipParameters.GetComponent<ShipParameters>().PrintStats(_activeMaxHP, _activeATK, _activeDEF, _activeCRITDMG, _activeCRITRate);
        }
}
