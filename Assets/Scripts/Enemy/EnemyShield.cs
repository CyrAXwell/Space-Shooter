using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private GameObject shield;
    [SerializeField] float shieldTimer;
    [SerializeField] float activationTime;
    private float timer;
    private bool shieldTimerLocked = false;
    private Animator[] animator;
    private bool shieldActive = false;
   
    void Start()
    {
        shield = transform.GetChild(4).gameObject;
        timer = shieldTimer;
        animator = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        if(shieldTimerLocked && !shieldActive)
        {
            shieldActive = true;
            animator[2].SetBool("ShieldActivation", true);
            StartCoroutine(ShieldActivation(activationTime));
        }
    }


    void FixedUpdate()
    {
        if(shieldTimerLocked == false)
        {
            shieldTimer -= Time.fixedDeltaTime;
            if(shieldTimer <= 0)
            {
                shieldTimerLocked = true;
            }
        }
        
    }

    private IEnumerator ShieldActivation(float time)
    {   
        yield return new WaitForSeconds(time);
        animator[2].SetBool("ShieldActivation", false);
        animator[2].SetBool("ShieldActive", true);
        shield.SetActive(true);
    }
}
