using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship1RapidFire : MonoBehaviour
{
    [Header("Ship 1 - Rapid Fire Skill parameters")]
    public float rapidFireCoolDown;
    public float rapidFireActionTime;

    private float rapidFirerTimer = 0f;
    private bool rapidFirerTimerLocked = false;
    private bool rapidFireActive = false;

    private float timerCooldown;
    public float newTimerCooldown;
    [SerializeField] private int rapidFireDamage;

    private GameObject skillDisplay;
    private GameObject displayTimer;


    void Start()
    {
        rapidFirerTimer = rapidFireCoolDown;
        skillDisplay = GameObject.Find("Skill 2");
        displayTimer = skillDisplay.transform.GetChild(2).gameObject;
        skillDisplay.GetComponent<SkillDisplay>().SkillCharge();
        displayTimer.GetComponent<SkillTimer>().DisplayTime(rapidFirerTimer);
    }

    void FixedUpdate()
    {
        if(rapidFirerTimerLocked == false && StateNameController.startTimers)
        {
            rapidFirerTimer -= Time.fixedDeltaTime;
            displayTimer.GetComponent<SkillTimer>().DisplayTime(rapidFirerTimer);
            if(rapidFirerTimer <= 0)
            {
                rapidFirerTimerLocked = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillReady();

            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("r") && !StateNameController.isPaused)
        {
            if(rapidFirerTimerLocked & !rapidFireActive)
            {
                RapidFireSkill();
                skillDisplay.GetComponent<SkillDisplay>().SkillActive();


            }

        }
    }

    void RapidFireSkill()
    {
        rapidFireActive = true;
        timerCooldown = GetComponent<Shooting>().timer_cooldown;
        GetComponent<Shooting>().timer_cooldown = newTimerCooldown;
        GetComponent<Shooting>().rapidFireDamage = rapidFireDamage;
        //damage = GetComponent<Player>().damage;
        //GetComponent<Player>().damage += newDamage;
        StartCoroutine(EndRapidFireSkill(rapidFireActionTime));


    }

    public void ResetRapidFireSkill()
    {
        if(rapidFireActive)
        {
            StopRapidFireSkill();
        }else{
            skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            RapidFireSkill();
            StopRapidFireSkill();
        }

    }

    private IEnumerator EndRapidFireSkill(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(rapidFireActive)
        {
            StopRapidFireSkill();
        }
    }

    public void StopRapidFireSkill()
    {
        GetComponent<Shooting>().timer_cooldown = timerCooldown;
        GetComponent<Shooting>().rapidFireDamage = 0;
        //GetComponent<Player>().damage = damage;
        rapidFirerTimerLocked = false;
        rapidFireActive = false;
        rapidFirerTimer = rapidFireCoolDown;
    }


    public void ActionTimeUpgarde(float time)
    {
        rapidFireActionTime += time;

    }

    public void CooldownUpgarde(float time)
    {
        rapidFireCoolDown -= time;

    }

    public void DamageUpgarde(int addDamage)
    {
        rapidFireDamage += addDamage;

    }

    public void RateUpgarde(float time)
    {
        newTimerCooldown -= time;
        Debug.Log(newTimerCooldown);

    }



}
