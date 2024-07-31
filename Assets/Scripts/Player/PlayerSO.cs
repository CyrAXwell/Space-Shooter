using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerSO",menuName = "ScriptableObjects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private int health;
    [SerializeField] private int defense;
    [SerializeField] private int damage;
    [SerializeField] private int critDamage;
    [SerializeField] private int critChance;

    public int Health => health;
    public int Defense => defense;
    public int Damage => damage;
    public int CritDamage => critDamage;
    public int CritChance => critChance;

}
