using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int shieldHp;
    private int healHP;
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        shieldHp = transform.parent.gameObject.GetComponent<ShieldSkill>().GetShieldHealth();
        healHP = transform.parent.gameObject.GetComponent<ShieldSkill>().GetShieldHeal();
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        _audioManager.PlaySFX(_audioManager.PlayerHit, 0.7f);
        shieldHp -= damage;
        if(shieldHp <= 0)
        {
            DestroyShield();
        }
    }

    public void DestroyShield()
    {
        shieldHp = transform.parent.gameObject.GetComponent<ShieldSkill>().GetShieldHealth();
        healHP = transform.parent.gameObject.GetComponent<ShieldSkill>().GetShieldHeal();
        transform.parent.gameObject.GetComponent<ShieldSkill>().isTimerLocked = false;
        transform.parent.gameObject.GetComponent<Player>().Heal(healHP);
        GameObject.Find("Skill 1").GetComponent<SkillDisplay>().DisplaySkillCharge();
        gameObject.SetActive(false);

        // gameObject.SetActive(false);
        // OnDestroy?.Invoke();
        
    }
}
