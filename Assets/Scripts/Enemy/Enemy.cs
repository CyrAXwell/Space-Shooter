using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Enemy base stats")]
    [SerializeField] private int maxHp = 100;
    public int maxDef = 5;
    public int damage = 10;
    private int recDamage;
    
    private int health;
    private int def;
    private bool powerUp = false;
    public GameObject deathEffect;

    //public GameObject exp;

    private GameObject target;
    public int dropXP;

    [SerializeField] private GameObject powerUpIcon;

    private int wave = 1;
    [SerializeField] private int addHp;
    [SerializeField] private int addDef;
    [SerializeField] private int addDamage;
    //private WaveManager waveManager;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;
    private bool isCrit = false;

    [SerializeField] AudioSource hitSound;

    void Start()
    {
        wave = GameObject.Find("Wave panel").GetComponent<WaveManager>().waveCounter;
        //health = maxHp;
        //def = maxDef;
        GetStatsByWave();
        GetTarget();
        //recDamage = damage;
    }

    void GetStatsByWave()
    {
        maxHp = maxHp + addHp * (wave-1);
        maxDef = maxDef + addDef * (wave-1);
        damage = damage + addDamage * (wave-1);
        recDamage = damage;
        health = maxHp;
        def = maxDef;

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
    
    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        PlayHitSound();
        if(Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }else
        {
            isCrit = false;
        }

        if(damage <= def)
        {
            health -= 1;
            DisplayTakenDamage("1", isCrit);
        }else{
            health -= damage - def;
            DisplayTakenDamage((damage - def).ToString(), isCrit);
        }
        
        if(health <= 0)
        {
            Death();
            
        }
    }

    void Death()
    {
        target.GetComponent<Player>().SetXP(dropXP);
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
        //Instantiate(exp, transform.position, Quaternion.identity);
    }

    void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
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
           def = (int)Mathf.Round(maxDef * collider.transform.parent.GetComponent<PowerUp>().multiplier); 
           recDamage = damage;
           damage = (int)Mathf.Round(damage * collider.transform.parent.GetComponent<PowerUp>().multiplier);
           powerUp = true; 
        }
    }

    public void DeActivatePowerUp(Collider2D collider)
    {
        if (collider.gameObject.tag == "PowerUp")
        {
            powerUpIcon.SetActive(false);
            def = maxDef; 
            damage = recDamage; 
            powerUp = false; 
        }
    }

    void PlayHitSound()
    {
        hitSound.Play();
    }


    
}
