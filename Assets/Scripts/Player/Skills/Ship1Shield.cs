using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship1Shield : MonoBehaviour
{
    [Header("Ship 1 - Shield Skill")]
    public float shieldCoolDown;
    public int shieldHP;
    public int healHP;
    private GameObject shield;

    public string skillColor;

    private GameObject skillDisplay;
    //private GameObject skillIcon;
    private GameObject displayTimer;

    private float shieldTimer = 0f;
    [NonSerialized] public bool shieldTimerLocked = false;
    [NonSerialized] public bool shieldActive = false;  

    void Start()
    {
        shieldTimer = shieldCoolDown;
        if(StateNameController.character == "Character 2")
        {
            shield = transform.GetChild(4).gameObject;
        }else{
            shield = transform.GetChild(2).gameObject;
        }
        skillDisplay = GameObject.Find("Skill 1");
        //skillIcon = skillDisplay.transform.GetChild(1).gameObject;
        displayTimer = skillDisplay.transform.GetChild(2).gameObject;
        skillDisplay.GetComponent<SkillDisplay>().SkillCharge();
        displayTimer.GetComponent<SkillTimer>().DisplayTime(shieldTimer);    
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(shieldTimerLocked)
            {
                ShieldSkill();
                skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            }
        
        }
        
    }

    void FixedUpdate()
    {
        if(shieldTimerLocked == false  && StateNameController.startTimers)
        {
            shieldTimer -= Time.fixedDeltaTime;
            displayTimer.GetComponent<SkillTimer>().DisplayTime(shieldTimer);
            if(shieldTimer <= 0)
            {
                shieldTimer = 0;
                shieldTimerLocked = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillReady();
                
            }
        }
        
    }

    void ShieldSkill()
    {
        shield.SetActive(true);
        
        //shieldTimerLocked = false;
        shieldTimer = shieldCoolDown;
    }

    public void ResetShieldSkill()
    {
        shieldTimer = shieldCoolDown;
        shield.GetComponent<Shield>().DestroyShield();
    }

    public void ChangeShieldCD(float cooldown)
    {
        shieldCoolDown -= cooldown;
        Debug.Log(shieldCoolDown);
    }

    public void ChangeShieldHealing(int hp)
    {
        healHP += hp;
        Debug.Log(healHP);
    }

    public void ChangeShieldHP(int hp)
    {
        shieldHP += hp;
        Debug.Log(shieldHP);
    }

}
