using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int health;
    [SerializeField] private int maxDef;
    //[SerializeField] private int laserDamage = 10;
    //[SerializeField] private int plasmaDamage = 20;

    [SerializeField] private GameObject deathEffect;
    private BossHealthBar bossHealthBarUI;

    [SerializeField] private GameObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;
    private bool isCrit = false;

    void Start()
    {
        GameObject bossHPBar = GameObject.Find("Boss HP Bar");
        bossHPBar.SetActive(true);
        bossHealthBarUI = bossHPBar.GetComponent<BossHealthBar>();
        health = maxHp;
        bossHealthBarUI.SetHealth(health, maxHp);
    }

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        if(Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }else
        {
            isCrit = false;
        }
        if(damage <= maxDef)
        {
            health -= 1;
            DisplayTakenDamage("1", isCrit);
        }else{
            health -= damage - maxDef;
            DisplayTakenDamage((damage - maxDef).ToString(), isCrit);
        }
        bossHealthBarUI.SetHealth(health, maxHp);
        if(health <= 0)
        {
            Death();
        }
        
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
        //StartCoroutine(DislayGameOverScreen(0f));
        Destroy(gameObject);
    }

    private IEnumerator DislayGameOverScreen(float interval)
    {
        
        yield return new WaitForSeconds(interval);
        GameObject.Find("Wave panel").GetComponent<WaveManager>().ClearObjects();
        GameObject.Find("GameManager").GetComponent<GameOverScreen>().GameWin();
    }
}
