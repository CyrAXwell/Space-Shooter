using UnityEngine;
using TMPro;
using System;

public class Boss : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDeath;

    [SerializeField] private int maxHp;
    [SerializeField] private int maxDef;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private ObjectPoolObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;

    private int _health;
    private ObjectPoolManager _objectPool;
    private AudioManager _audioManager;

    public void Initialize(ObjectPoolManager objectPoolManager)
    {
        _health = maxHp;
        _objectPool = objectPoolManager;
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public int GetHealth() => _health;
    public int GetMaxHealth() => maxHp;

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        _audioManager.PlaySFX(_audioManager.EnemyHit, 0.3f);
        bool isCrit = false;
        if(UnityEngine.Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }

        int takenDamage = damage <= maxDef ? 1 : damage - maxDef;
        _health -= takenDamage;
        DisplayTakenDamage(takenDamage.ToString(), isCrit);
        
        OnTakeDamage?.Invoke();
        if(_health <= 0)
            Death();
    }

    public void DisplayTakenDamage(string text, bool crit)
    {
        ObjectPoolObject displayText = _objectPool.GetObject(damageText).GetComponent<ObjectPoolObject>();
        displayText.gameObject.name = deathEffect.name.ToString();
        displayText.transform.position = transform.position;

        displayText.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        displayText.transform.GetChild(0).GetComponent<TMP_Text>().color = crit ? critColor : Color.white; 
        
        _objectPool.ReleaseObject(displayText, 0.5f);
    }

    public void Death()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
