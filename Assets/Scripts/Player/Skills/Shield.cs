using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _health;
    private ShieldSkill _shieldSkill;
    private AudioManager _audioManager;

    public void Initialize(int health, ShieldSkill shieldSkill)
    {
        _health = health;
        _shieldSkill = shieldSkill;
    }

    void Start()
    {
        gameObject.SetActive(false);
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damage)
    {
        _audioManager.PlaySFX(_audioManager.PlayerHit, 0.7f);
        _health -= damage;
        if(_health <= 0)
            DestroyShield();
    }

    public void DestroyShield()
    {
        _shieldSkill.OnShieldDestroy();
    }
}
