using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship3Regen : MonoBehaviour
{
    [Header("Ship 1 - Regen Skill")]
    public float regenCoolDown;
    public int regenHP;
    public float regenInterval;
    public float regenActiveTime;
    private bool regenActive;

    public string skillColor;

    private GameObject skillDisplay;
    private GameObject displayTimer;

    private float regenTimer = 0f;
    [NonSerialized] public bool regenTimerLocked = false;
    private Player player;

    void Start()
    {
        regenTimer = regenCoolDown;
        skillDisplay = GameObject.Find("Skill 1");
        displayTimer = skillDisplay.transform.GetChild(2).gameObject;
        skillDisplay.GetComponent<SkillDisplay>().SkillCharge();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        displayTimer.GetComponent<SkillTimer>().DisplayTime(regenTimer);
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !StateNameController.isPaused)
        {
            if(regenTimerLocked && !regenActive)
            {
                RegenSkill();
                skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            }
        
        }
        
    }

    void FixedUpdate()
    {
        if(regenTimerLocked == false && StateNameController.startTimers)
        {
            regenTimer -= Time.fixedDeltaTime;
            displayTimer.GetComponent<SkillTimer>().DisplayTime(regenTimer);
            if(regenTimer <= 0)
            {
                regenTimer = 0;
                regenTimerLocked = true;
                skillDisplay.GetComponent<SkillDisplay>().SkillReady();
                
            }
        }
        
    }

    void RegenSkill()
    {
        player.Heal(regenHP);
        regenActive = true;
        StartCoroutine(Regen(regenInterval));
        StartCoroutine(RegenEnd(regenActiveTime));
    }

    private IEnumerator Regen(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(regenActive)
        {
            player.Heal(regenHP);
            StartCoroutine(Regen(regenInterval));
        }
    }

    private IEnumerator RegenEnd(float interval)
    {
        yield return new WaitForSeconds(interval);
        if(regenActive)
        {
            StopSkill();
        }

    }

    public void ResetSkill()
    {
        if(regenActive)
        {
            StopSkill();
        }else{
            skillDisplay.GetComponent<SkillDisplay>().SkillActive();
            RegenSkill();
            StopSkill();
        }
    }

    void StopSkill()
    {
        regenActive = false;
        regenTimerLocked = false;
        regenTimer = regenCoolDown;
    }

    public void CooldownUpgarde(float time)
    {
        regenCoolDown -= time;
        Debug.Log(regenCoolDown);
    }

    public void HealingUpgarde(int hp)
    {
        regenHP += hp;
        Debug.Log(regenHP);
    }

    public void ActionTimeUpgarde(float time)
    {
        regenActiveTime += time;
        Debug.Log(regenActiveTime);
    }

}
