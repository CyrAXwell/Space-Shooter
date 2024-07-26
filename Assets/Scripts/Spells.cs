using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public GameObject spellsManager;
    // [Header("Ship 1 - Rapid Fire Skill parameters")]
    // public float rapidFireCoolDown;
    // public float rapidFireActionTime;

    // private float rapidFirerTimer = 0f;
    // private bool rapidFirerTimerLocked = false;
    
    // private float timerCooldown;
    // private float newTimerCooldown = 0.1f;

    // [Header("Ship 1 - Shield Skill")]
    // public float shieldCoolDown;
    // public int shieldHP;
    // private float shieldTimer = 0f;
    // private bool shieldTimerLocked = false;
    // private GameObject shield;

    void Start()
    {
        // shield = transform.GetChild(2).gameObject;
        //rapidFirerTimer = rapidFireCoolDown;
        // shieldTimer = shieldCoolDown;
    }

    void FixedUpdate()
    {
        // if(rapidFirerTimerLocked == false)
        // {
        //     rapidFirerTimer -= Time.fixedDeltaTime;
        //     if(rapidFirerTimer <= 0)
        //     {
        //         rapidFirerTimerLocked = true;
                
        //     }
        // }

        // if(shieldTimerLocked == false)
        // {
        //     shieldTimer -= Time.fixedDeltaTime;
        //     if(shieldTimer <= 0)
        //     {
        //         shieldTimerLocked = true;
                
        //     }
        // }

        //Debug.Log(rapidFirerTimer);
        //Debug.Log(shieldTimer);
        
    }

    void Update()
    {
        // if(Input.GetKeyDown("e"))
        // {
        //     ShieldSkill()
        // }

        // if(Input.GetKeyDown("r"))
        // {
        //     if(rapidFirerTimerLocked)
        //     {
        //         RapidFireSkill();
                
        //     }
        
        // }

        // if(Input.GetKeyDown("e"))
        // {
        //     if(shieldTimerLocked)
        //     {
        //         ShieldSkill();
                
        //     }
        
        // }
        
    }

    // void RapidFireSkill()
    // {
    //     Debug.Log("RapidFireSkill");
    //     timerCooldown = GetComponent<Shooting>().timer_cooldown;
    //     GetComponent<Shooting>().timer_cooldown = newTimerCooldown;
    //     StartCoroutine(EndRapidFireSkill(rapidFireActionTime));
        

    // }

    // private IEnumerator EndRapidFireSkill(float interval)
    // {
    //     yield return new WaitForSeconds(interval);
    //     GetComponent<Shooting>().timer_cooldown = timerCooldown;
    //     rapidFirerTimerLocked = false;
    //     rapidFirerTimer = rapidFireCoolDown;
    //     //Debug.Log("RapidFireSkillEnd");

    // }

    // void ShieldSkill()
    // {
    //     shield.SetActive(true);
    //     shieldTimerLocked = false;
    //     shieldTimer = shieldCoolDown;
    // }
}
