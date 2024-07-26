using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipParameters : MonoBehaviour

    
{
    public TextMeshProUGUI maxHealthTMP;
    public TextMeshProUGUI damageTMP;
    public TextMeshProUGUI armorTMP;
    public TextMeshProUGUI critDamageTMP;
    public TextMeshProUGUI critRateTMP;
    

    public void PrintStats(int health, int damage, int armor, int critDamage, int critChance)
    {
        maxHealthTMP.text = health.ToString();
        damageTMP.text = damage.ToString();
        armorTMP.text = armor.ToString();
        critDamageTMP.text = ((float)critDamage / 100).ToString() + "%";
        critRateTMP.text = ((float)critChance / 100).ToString() + "%";

    }
}
