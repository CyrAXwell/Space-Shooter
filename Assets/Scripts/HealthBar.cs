using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI maxHealthTMP;
    public TextMeshProUGUI healthTMP;

    public void SetMaxHealt(int health)
    {
        healthBar.fillAmount = 1f;
        maxHealthTMP.text = health.ToString();
        //slider.maxValue = health;
        //slider.value = health;
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / maxHealth;
        maxHealthTMP.text = maxHealth.ToString();
        healthTMP.text = health.ToString();
        //slider.value = health;
    }
    
}
