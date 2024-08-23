using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private ObjectPoolObject damageText;
    [SerializeField] private Vector3 offsetTextPosition;
    [SerializeField] private Color critColor;
    [SerializeField] private GameObject powerUpIcon;
    [SerializeField] private ObjectPoolObject deathEffect;
    [SerializeField] private int dropXP;
    
    private Player _player;
    private EnemySpawner _spawner;
    private ObjectPoolManager _objectPool;
    private int _health;
    private int _defense;
    private int _damage;
    private int _wave;
    private bool _powerUp = false;
    private List<Collider2D> powerUpAreas = new List<Collider2D>();
    private AudioManager _audioManager;

    public void Initialize(Player player, int wave, AudioManager audioManager, EnemySpawner spawner, ObjectPoolManager objectPool)
    {
        _player = player;
        _wave = wave;
        _spawner = spawner;
        _objectPool = objectPool;
        _audioManager = audioManager;
        GetStatsByWave(wave);
        powerUpIcon.SetActive(false);
        powerUpAreas.Clear();
        _powerUp = false; 
    }

    public int GetDamage() => _damage;
    public int GetWave() => _wave;

    public void TakeDamage(int damage, int critChance, int critDamage)
    {
        bool isCrit = false;
        _audioManager.PlaySFX(_audioManager.EnemyHit, 0.3f);
        if(UnityEngine.Random.Range(0,10001) <= critChance)
        {
            damage = damage + Mathf.RoundToInt(damage * critDamage / 10000);
            isCrit = true;
        }

        int takenDamage = damage <= _defense ? 1 : damage - _defense;
        _health -= takenDamage;
        DisplayTakenDamage(takenDamage.ToString(), isCrit);
        
        if(_health <= 0)
            Death();
    }

    private void GetStatsByWave(int wave)
    {
        _health = enemySO.Health + enemySO.HealthIncrease * (wave - 1);
        _defense = enemySO.Defense + enemySO.DefenseIncrease * (wave - 1);
        _damage = enemySO.Damage + enemySO.DamageIncrease * (wave - 1);
    }

    private void Death()
    {
        _player.SetXP(dropXP);

        ObjectPoolObject effect = _objectPool.GetObject(deathEffect).GetComponent<ObjectPoolObject>();
        effect.gameObject.name = deathEffect.name.ToString();
        effect.transform.position = transform.position;
        _objectPool.ReleaseObject(effect, 0.5f);

        _spawner.OnEnemyDeath(this);
    }

    public void DisplayTakenDamage(string text, bool crit)
    {
        ObjectPoolObject displayText = _objectPool.GetObject(damageText).GetComponent<ObjectPoolObject>();
        displayText.transform.position = transform.position + offsetTextPosition;
        displayText.gameObject.name = damageText.name.ToString();

        displayText.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        displayText.transform.GetChild(0).GetComponent<TMP_Text>().color = crit ? critColor : Color.white;  

        _objectPool.ReleaseObject(displayText, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        powerUpAreas.Add(collider);
        ActivatePowerUp(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        powerUpAreas.Remove(collider);
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
        if (collider.gameObject.tag == "PowerUp" && powerUpAreas.Count == 0)
        {
            powerUpIcon.SetActive(false);
            _defense = enemySO.Defense + enemySO.DefenseIncrease * (_wave - 1);
            _damage = enemySO.Damage + enemySO.DamageIncrease * (_wave - 1);
            _powerUp = false; 
        }
    }
}
