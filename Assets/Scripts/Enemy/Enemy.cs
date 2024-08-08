using UnityEngine;
using TMPro;
using System;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;
    [SerializeField] private GameObject powerUpIcon;
    
    private bool powerUp = false;
    public GameObject deathEffect;

    public int dropXP;

    private bool isCrit = false;

    [SerializeField] AudioSource hitSound;

    private Player _player;
    private int _health;
    private int _defense;
    private int _damage;
    private int _wave;

    public void Initialize(Player player, int wave)
    {
        _player = player;
        _wave = wave;
        GetStatsByWave(wave);
    }

    public int GetDamage() => _damage;

    private void GetStatsByWave(int wave)
    {
        _health = enemySO.Health + enemySO.HealthIncrease * (wave - 1);
        _defense = enemySO.Defense + enemySO.DefenseIncrease * (wave - 1);
        _damage = enemySO.Damage + enemySO.DamageIncrease * (wave - 1);
    }

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        PlayHitSound();
        if(UnityEngine.Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }
        else
            isCrit = false;

        if(damage <= _defense)
        {
            _health -= 1;
            DisplayTakenDamage("1", isCrit);
        }
        else
        {
            _health -= damage - _defense;
            DisplayTakenDamage((damage - _defense).ToString(), isCrit);
        }
        
        if(_health <= 0)
            Death();
    }

    void Death()
    {
        _player.SetXP(dropXP);
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }

    public void DisplayTakenDamage(string text, bool crit)
    {
        GameObject displayText = Instantiate(damageText, transform.position + offsetTextPosition, Quaternion.identity);
        Destroy(displayText,0.5f);
        displayText.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        if(crit)
            displayText.transform.GetChild(0).GetComponent<TMP_Text>().color = critColor;   
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        ActivatePowerUp(collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        DeActivatePowerUp(collider);
    }

    public void ActivatePowerUp(Collider2D collider)
    {
        if (collider.gameObject.tag == "PowerUp" && !powerUp)
        {
           powerUpIcon.SetActive(true);
           _defense = (int)Mathf.Round(_defense * collider.transform.parent.GetComponent<PowerUp>().multiplier); 
           _damage = (int)Mathf.Round(_damage * collider.transform.parent.GetComponent<PowerUp>().multiplier);
           powerUp = true; 
        }
    }

    public void DeActivatePowerUp(Collider2D collider)
    {
        if (collider.gameObject.tag == "PowerUp")
        {
            powerUpIcon.SetActive(false);
            _defense = enemySO.Defense + enemySO.DefenseIncrease * (_wave - 1);
            _damage = enemySO.Damage + enemySO.DamageIncrease * (_wave - 1);
            powerUp = false; 
        }
    }

    void PlayHitSound()
    {
        hitSound.Play();
    }
}
