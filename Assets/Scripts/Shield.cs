using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int shieldHp;
    private int healHP;
    [SerializeField] AudioSource hitSound;

    void Start()
    {
        shieldHp = transform.parent.gameObject.GetComponent<Ship1Shield>().shieldHP;
        healHP = transform.parent.gameObject.GetComponent<Ship1Shield>().healHP;
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        hitSound.Play();
        shieldHp -= damage;
        if(shieldHp <= 0)
        {
            DestroyShield();
        }
    }

    public void DestroyShield()
    {
        shieldHp = transform.parent.gameObject.GetComponent<Ship1Shield>().shieldHP;
        healHP = transform.parent.gameObject.GetComponent<Ship1Shield>().healHP;
        transform.parent.gameObject.GetComponent<Ship1Shield>().shieldTimerLocked = false;
        transform.parent.gameObject.GetComponent<Player>().Heal(healHP);
        GameObject.Find("Skill 1").GetComponent<SkillDisplay>().SkillCharge();
        gameObject.SetActive(false);
        
    }
}
