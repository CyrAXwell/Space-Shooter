using UnityEngine;
using TMPro;
using System;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private ExplosionEffect damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;
    [SerializeField] private GameObject powerUpIcon;
    [SerializeField] private ExplosionEffect deathEffect;
    [SerializeField] private int dropXP;
    
    private Player _player;
    private EnemySpawner _spawner;
    private ObjectPoolManager _objectPool;
    private int _health;
    private int _defense;
    private int _damage;
    private int _wave;
    private bool _powerUp = false;
    private AudioManager _audioManager;

    public void Initialize(Player player, int wave, AudioManager audioManager, EnemySpawner spawner, ObjectPoolManager objectPool)
    {
        _player = player;
        _wave = wave;
        _spawner = spawner;
        _objectPool = objectPool;
        _audioManager = audioManager;
        GetStatsByWave(wave);
    }

    public int GetDamage() => _damage;
    public int GetWave() => _wave;

    private void GetStatsByWave(int wave)
    {
        _health = enemySO.Health + enemySO.HealthIncrease * (wave - 1);
        _defense = enemySO.Defense + enemySO.DefenseIncrease * (wave - 1);
        _damage = enemySO.Damage + enemySO.DamageIncrease * (wave - 1);
    }

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        bool isCrit = false;
        _audioManager.PlaySFX(_audioManager.EnemyHit, 0.3f);
        if(UnityEngine.Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }

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

    private void Death()
    {
        _player.SetXP(dropXP);
        ExplosionEffect effect = _objectPool.GetObject(deathEffect).GetComponent<ExplosionEffect>();
        effect.transform.position = transform.position;
        effect.gameObject.name = deathEffect.name.ToString();
        _objectPool.ReleaseObject(effect, 0.5f);
        _spawner.OnEnemyDeath(this);
    }

    public void DisplayTakenDamage(string text, bool crit)
    {
        ExplosionEffect effect = _objectPool.GetObject(damageText).GetComponent<ExplosionEffect>();
        effect.transform.position = transform.position + offsetTextPosition;
        effect.gameObject.name = damageText.name.ToString();
        _objectPool.ReleaseObject(effect, 0.5f);
        effect.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        if(crit)
            effect.transform.GetChild(0).GetComponent<TMP_Text>().color = critColor;   
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ActivatePowerUp(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        DeActivatePowerUp(collider);
    }

    public void ActivatePowerUp(Collider2D collider)
    {
        if (collider.gameObject.tag == "PowerUp" && !_powerUp)
        {
           powerUpIcon.SetActive(true);
           _defense = (int)Mathf.Round(_defense * collider.transform.parent.GetComponent<PowerUp>().GetPowerMultiplier()); 
           _damage = (int)Mathf.Round(_damage * collider.transform.parent.GetComponent<PowerUp>().GetPowerMultiplier());
           _powerUp = true; 
        }
    }

    private void DeActivatePowerUp(Collider2D collider)
    {
        if (collider.gameObject.tag == "PowerUp")
        {
            powerUpIcon.SetActive(false);
            _defense = enemySO.Defense + enemySO.DefenseIncrease * (_wave - 1);
            _damage = enemySO.Damage + enemySO.DamageIncrease * (_wave - 1);
            _powerUp = false; 
        }
    }
}
