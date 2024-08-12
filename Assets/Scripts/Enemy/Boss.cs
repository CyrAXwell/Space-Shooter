using UnityEngine;
using TMPro;
using System;

public class Boss : MonoBehaviour
{
    public event Action OnTakeDamage;
    [SerializeField] private int maxHp;
    [SerializeField] private int maxDef;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;

    private int health;

    public void Initialize()
    {
        health = maxHp;
    }

    public int GetHealth() => health;
    public int GetMaxHealth() => maxHp;

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        bool isCrit = false;
        if(UnityEngine.Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }

        if(damage <= maxDef)
        {
            health -= 1;
            DisplayTakenDamage("1", isCrit);
        }
        else
        {
            health -= damage - maxDef;
            DisplayTakenDamage((damage - maxDef).ToString(), isCrit);
        }
        
        OnTakeDamage?.Invoke();
        if(health <= 0)
            Death();
        
    }

    public void DisplayTakenDamage(string text, bool crit)
    {
        GameObject displayText = Instantiate(damageText, transform.position + offsetTextPosition, Quaternion.identity);
        Destroy(displayText,0.5f);
        displayText.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        if(crit)
        {
            displayText.transform.GetChild(0).GetComponent<TMP_Text>().color = critColor; 
        } 
        
    }

    public void Death()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        GameObject.Find("GameManager").GetComponent<GameOverScreen>().GameWin();
        Destroy(gameObject);
    }
}
