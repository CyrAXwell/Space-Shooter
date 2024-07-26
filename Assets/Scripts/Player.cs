using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject deathEffect;


    private Ship1Stats ShipStats;
    [Header("Player stats")]
    private int health;
    private int maxHealth;
    private int def;
    [HideInInspector] public int damage;
    [HideInInspector] public int critDamage;
    [HideInInspector] public int critChance;
    
    private int lvl = 1;
    private int xP = 0;
    private int maxXP = 10;
    //[HideInInspector]


    //[SerializeField] private GameObject gemManager;
    [HideInInspector] public int activeMaxHP;
    [HideInInspector] public int activeHP;
    [HideInInspector] public int activeATK;
    [HideInInspector] public int activeDEF;
    [HideInInspector] public int activeCRITDMG;
    [HideInInspector] public int activeCRITRate;

    


    

    //[SerializeField] private GameObject uiHealthBar;

    [Header("UI parameters")]
    private GameObject uiHealthBar;
    private HealthBar healthBar;
    private GameObject shipParameters;
    private GameObject uiXPBar;
    private XPBar xPBar;
    private GameObject levelUpMenu;

    [SerializeField] AudioSource hitSound;


    void Start()
    {
        ShipStats = GetComponent<Ship1Stats>();

        maxHealth = ShipStats.basemMaxHp;
        health = maxHealth;
        def = ShipStats.baseDef;
        damage = ShipStats.baseDamage;
        critChance = ShipStats.baseCritChance;
        critDamage = ShipStats.baseCritDamage;

        //gemManager.GetComponent<GemManager>().CalculeteStats();
        activeHP = health;
        activeMaxHP = maxHealth;
        activeATK = damage;
        activeDEF = def;
        activeCRITDMG = critDamage;
        activeCRITRate = critChance;
        //UpdateGemStats(0,0,0,0,0);
        
        
        uiHealthBar = GameObject.Find("HP");
        uiXPBar = GameObject.Find("XP");
        levelUpMenu = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        
        shipParameters = GameObject.Find("Ship parameters");
        healthBar = uiHealthBar.GetComponent<HealthBar>();
        xPBar = uiXPBar.GetComponent<XPBar>();
        //healthBar.SetMaxHealt(maxHealth);
        xPBar.SetXP(xP, maxXP);
        xPBar.SetLvl(lvl);
        healthBar.SetHealth(activeHP, activeMaxHP);

        shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);
    }

    // void Update()
    // {
    //     if(Input.GetKeyDown("x"))
    //     {
    //         GetXP(5);
    //     }
    // }

    public void TakeDamage(int dmg)
    {
        hitSound.Play();
        if(dmg <= activeDEF)
        {
            activeHP -= 1;
        }else{
            activeHP -= dmg - activeDEF;
        }
        
        if(activeHP <= health)
        {
            health = activeHP;
        }
        //HealthBar healthBar = uiHealthBar.GetComponent<HealthBar>();
        healthBar.SetHealth(activeHP, activeMaxHP);
        if(activeHP <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        GameObject.Find("GameManager").GetComponent<GameOverScreen>().GameOver();
        //Destroy(gameObject);
    }


    public void GetXP(int addXP)
    {
        xP += addXP;
        if(xP >= maxXP)
        {

            lvlUP();
        }
        xPBar.SetXP(xP, maxXP);
    }

    public void UpdateGemStats(int hpGem, int atkGem, int defGem, int critdmgGem, int critrateGem)
    {
        Debug.Log("input atk " +atkGem);
        Debug.Log("damage " +damage);
        activeMaxHP = maxHealth + hpGem;
        activeHP = activeMaxHP;
        //activeHP = health + hpGem;
        activeATK = damage + atkGem;
        Debug.Log("activeATK " + activeATK);
        activeDEF = def + defGem;
        activeCRITDMG = critDamage + critdmgGem;
        activeCRITRate = critChance + critrateGem;
        //FullHeal();
        healthBar.SetHealth(activeHP, activeMaxHP);
        Debug.Log("activeATK+ " + activeATK);
        shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);

    }

    public void lvlUP()
    {
        lvl ++;
        if(xP > maxXP)
        {
            xP -=  maxXP;
            maxXP = (lvl + 3) * (lvl + 3);
            GetXP(0);
        } else
        {
            xP = 0;
            maxXP = (lvl + 3) * (lvl + 3);
        }
        xPBar.SetXP(xP, maxXP);
        xPBar.SetLvl(lvl);
        
        levelUpMenu.GetComponent<LevelUpMenu>().OpenLevelUpMenu();

    }

    public void FullHeal()
    {
        activeHP = activeMaxHP;
        healthBar.SetHealth(activeHP, activeMaxHP);
    }

    public void ChangeHp(int addHp)
        {
            maxHealth += addHp;
            health += addHp;
            activeHP += addHp;
            activeMaxHP += addHp;

            //HealthBar healthBar = uiHealthBar.GetComponent<HealthBar>();
            //healthBar.SetMaxHealt(maxHealth);
            healthBar.SetHealth(activeHP, activeMaxHP);
            shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);
        }

    public void Heal(int healHP)
        {
            if(activeHP + healHP >= activeMaxHP)
            {
                activeHP = activeMaxHP;
            }else
            {
                activeHP += healHP;
            }

            if(health + healHP >= maxHealth)
            {
                health = maxHealth;
            }else
            {
                health += healHP;
            }

            //HealthBar healthBar = uiHealthBar.GetComponent<HealthBar>();
            //healthBar.SetMaxHealt(maxHealth);
            healthBar.SetHealth(activeHP, activeMaxHP);
        }

    public void UpgradeArmor(int addArmor)
        {
            def += addArmor;
            activeDEF += addArmor;
            shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);

        }

    public void UpgradeDamage(int addDamage)
        {
            damage += addDamage;
            activeATK += addDamage;
            shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);
        }

    public void UpgradeCritDamage(int addCritDamage)
        {
            critDamage += addCritDamage;
            activeCRITDMG += addCritDamage;
            shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);
        }
    
    public void UpgradeCritRate(int addCritRate)
        {
            critChance += addCritRate;
            activeCRITRate += addCritRate;
            shipParameters.GetComponent<ShipParameters>().PrintStats(activeMaxHP, activeATK, activeDEF, activeCRITDMG, activeCRITRate);
        }
}
