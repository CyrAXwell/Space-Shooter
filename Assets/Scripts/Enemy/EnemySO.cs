using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy",menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private int health;
    [SerializeField] private int healthIncrease;
    [SerializeField] private int defense;
    [SerializeField] private int defenseIncrease;
    [SerializeField] private int damage;
    [SerializeField] private int damageIncrease;
    [SerializeField] private int expDrop;
    
    public int Health => health;
    public int Defense => defense;
    public int Damage => damage;
    public int HealthIncrease => healthIncrease;
    public int DefenseIncrease => defenseIncrease;
    public int DamageIncrease => damageIncrease;
    public int ExpDrop => expDrop;

}
