using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldStats : MonoBehaviour
{
    public int  shieldDurability;
    public int addHp;
    private int shieldHp;
    private Animator animator;
    [SerializeField] GameObject shieldEffect;
    private int wave = 1;
    [SerializeField] AudioSource hitSound;

    void Start()
    {
        wave = GameObject.Find("Wave panel").GetComponent<WaveManager>().waveCounter;
        gameObject.SetActive(false);
        shieldHp = shieldDurability + addHp * (wave-1);
        animator = shieldEffect.GetComponent<Animator>(); 
    }

    public void TakeDamage(int damage)
    {
        hitSound.Play();
        shieldHp -= damage;
        transform.parent.GetComponent<Enemy>().DisplayTakenDamage(damage.ToString(), false);
        if(shieldHp <= 0)
        {
            DestroyShield();
        }
    }

    void DestroyShield()
    {
        animator.SetBool("ShieldActive", false);
        shieldHp = shieldDurability;
        gameObject.SetActive(false);
        
    }


}
