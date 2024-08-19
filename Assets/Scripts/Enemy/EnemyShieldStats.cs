using UnityEngine;

public class EnemyShieldStats : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int healthIncrease;
    [SerializeField] GameObject shieldEffect;

    private int _health;
    private EnemyShield _enemyShield;
    private Animator _animator;
    private AudioManager _audioManager;

    public void Initialize(int wave, EnemyShield enemyShield)
    {
        _health = baseHealth + healthIncrease * wave;
        _enemyShield = enemyShield;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damage)
    {
        _audioManager.PlaySFX(_audioManager.EnemyHit, 0.3f);

        _health -= damage;
        transform.parent.GetComponent<Enemy>().DisplayTakenDamage(damage.ToString(), false);
        if(_health <= 0)
            DestroyShield();
    }

    private void DestroyShield()
    {
        _enemyShield.OnShieldDestroy();
    }
}
